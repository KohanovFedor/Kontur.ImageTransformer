using System.Drawing;
using System.Net;


namespace Kontur.ImageTransformer
{
    public static class WorkWithImages
    {
        public static byte[] SwitchEffectAndTrim(byte[] byteContent, ref RequestUrl req)
        {
            Bitmap img = Convertor.RequestToBytePNG(byteContent);
            if(img==null || img.Height>1000 || img.Width>1000)
            {
                req.StatusResponse = HttpStatusCode.BadRequest;
                return new byte[0];
            }
            img = SwitchEffect(img, req.Effect);
            Rect rectResponse = new Rect();
            if (!FuncOverRect.Intersect(req.Rect, new Rect(0, 0, img.Width, img.Height), ref rectResponse))
            {
                req.StatusResponse = HttpStatusCode.NoContent;
                return new byte[0];
            }
            img = TrimBitmap.Trim(img, rectResponse);
            return Convertor.ImageToByte(img);
        }

        private static Bitmap SwitchEffect(Bitmap srcBmp, IEffect effect)
        {
            Effects effectClass = new Effects();
            return effectClass.ExecuteEffect(effect, srcBmp);
        }


        

        

    }
}
