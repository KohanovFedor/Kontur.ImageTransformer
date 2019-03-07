using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Kontur.ImageTransformer
{
    internal class AsyncHttpServer : IDisposable
    {
        public AsyncHttpServer()
        {
            listener = new HttpListener();
        }
        
        public void Start(string prefix)
        {
            lock (listener)
            {
                if (!isRunning)
                {
                    listener.Prefixes.Clear();
                    listener.Prefixes.Add(prefix);
                    listener.Start();

                    listenerThread = new Thread(Listen)
                    {
                        IsBackground = true,
                        Priority = ThreadPriority.Highest
                    };
                    listenerThread.Start();

                    isRunning = true;
                }
            }
        }

        public void Stop()
        {
            lock (listener)
            {
                if (!isRunning)
                    return;

                listener.Stop();

                listenerThread.Abort();
                listenerThread.Join();
                
                isRunning = false;
            }
        }

        public void Dispose()
        {
            if (disposed)
                return;

            disposed = true;

            Stop();

            listener.Close();
        }
        
        private async void Listen()
        {
            while (true)
            {
                try
                {
                    if (listener.IsListening)
                    {
                        var context = await listener.GetContextAsync();
                        if (taskCount > taskBound)
                        {
                            await HandleResponseErrorAsync(context.Response, 429);
                        }
                        else
                        {
                            Interlocked.Increment(ref taskCount);
                            Task.Run(async () =>
                            {
                                await HandleContextAsync(context);
                            }).ContinueWith((lastTask) => Interlocked.Decrement(ref taskCount));
                        }
                    }
                    else Thread.Sleep(0);
                }
                catch (ThreadAbortException error)
                {
                    Console.WriteLine(error.ToString());
                }
                catch (Exception error)
                {
                    Console.WriteLine(error.ToString());
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async Task HandleResponseErrorAsync(HttpListenerResponse response, int status)
        {
            response.StatusCode = status;
            response.Close();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private async Task HandleResponseOKtAsync(HttpListenerResponse response, byte[] resArray)
        {
            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentType = "multipart/form-data";
            using (Stream output = response.OutputStream)
                await output.WriteAsync(resArray, 0, resArray.Length);
            response.Close();
        }

        private async Task HandleContextAsync(HttpListenerContext listenerContext)
        {
            HttpListenerRequest request = listenerContext.Request;
            HttpListenerResponse response = listenerContext.Response;
            RequestUrl req = new RequestUrl(request.RawUrl.ToLower());
            if (request.HttpMethod == "POST" && (request.ContentLength64 <= 102400 || request.ContentLength64 >8) && req.StatusResponse == HttpStatusCode.OK)
            {
                byte[] byteContent = new byte[request.ContentLength64];
                using (Stream body = request.InputStream)
                    await body.ReadAsync(byteContent, 0, byteContent.Length);
                byte[] resArray = WorkWithImages.SwitchEffectAndTrim(byteContent, ref req);
                if (req.StatusResponse != HttpStatusCode.OK)
                {
                    await HandleResponseErrorAsync(response, (int)req.StatusResponse);
                    return;
                }
                await HandleResponseOKtAsync(response, resArray);
            }
            else
            {
                await HandleResponseErrorAsync(response, (int)HttpStatusCode.BadRequest);
            }
        }

        private const int RATE_QUEUE = 7; // коэффициент, от него зависят результаты. Настраивается под железо
        private int taskBound = RATE_QUEUE * Environment.ProcessorCount-1;
        private int taskCount = 0;

        private readonly HttpListener listener;

        private Thread listenerThread;
        private bool disposed;
        private volatile bool isRunning;
    }
}