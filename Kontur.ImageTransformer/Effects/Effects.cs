using System.Drawing;

namespace Kontur.ImageTransformer
{
    class Effects
    {
        public Effects() { }

        public Bitmap ExecuteEffect(IEffect effect, Bitmap srcBmp)
        {
            return effect.ExecuteEffect(srcBmp);
        }
    }
}
