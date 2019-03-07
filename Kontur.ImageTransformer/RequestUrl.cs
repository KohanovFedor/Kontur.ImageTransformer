using System;
using System.Net;

namespace Kontur.ImageTransformer
{
    public struct RequestUrl
    {
        public RequestUrl(string request)
            : this()
        {
            string[] arrayRequest = request.Split('/');
            if (arrayRequest.Length < 4 || (arrayRequest.Length==5 && !arrayRequest[4].Equals(String.Empty)) || (arrayRequest.Length>5))
            {
                statusResponse = HttpStatusCode.BadRequest;
                return;
            }
            if (!DefEffect(arrayRequest[2])) return;
            if (!DefCoords(arrayRequest[3])) return;
            statusResponse = HttpStatusCode.OK;
        }

        private bool DefEffect(string reqFiltr)
        {
            if (reqFiltr.Equals("rotate-cw"))
                effect = new RotateCwEff();
            else if (reqFiltr.Equals("rotate-ccw"))
                effect = new RotateCcwEff();
            else if (reqFiltr.Equals("flip-v"))
                effect = new FlipVEff();
            else if (reqFiltr.Equals("flip-h"))
                effect = new FlipHEff();
            else
            {
                return DoBadRequest();
            }
            return true;
        }

        private bool DefCoords(string reqCoords)
        {
            string[] tempArrayaStr = reqCoords.Split(',');
            if (tempArrayaStr.Length != 4)
            {
                return DoBadRequest();
            }
            int l, t, w, h;
            if (!int.TryParse(tempArrayaStr[0], out l))
            {
                return DoBadRequest();
            }
            if (!int.TryParse(tempArrayaStr[1], out t))
            {
                return DoBadRequest();
            }
            if (!int.TryParse(tempArrayaStr[2], out w))
            {
                return DoBadRequest();
            }
            if (!int.TryParse(tempArrayaStr[3], out h))
            {
                return DoBadRequest();
            }
            rect = new Rect(l,t,w,h);
            return true;
        }

        private bool DoBadRequest()
        {
            statusResponse = HttpStatusCode.BadRequest;
            return false;
        }

        public override string ToString()
        {
            if (StatusResponse == HttpStatusCode.OK)
                return String.Format("StatusResponse: {0}\nFiltr: {1}\nRect: {2}", StatusResponse, Effect, Rect.ToString());
            else return String.Format("StatusResponse: {0}", StatusResponse);

        }

        public HttpStatusCode StatusResponse { get { return statusResponse; } set { statusResponse = value; } }
        public IEffect Effect { get { return effect; } }
        public Rect Rect { get { return rect; } }

        private HttpStatusCode statusResponse;
        private IEffect effect;
        private Rect rect;
        //private string rRequestStr;
    }
}
