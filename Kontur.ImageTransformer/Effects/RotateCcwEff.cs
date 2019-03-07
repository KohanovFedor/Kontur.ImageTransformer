using System.Drawing;
using System.Drawing.Imaging;

namespace Kontur.ImageTransformer
{
    class RotateCcwEff : IEffect
    {
        public RotateCcwEff()
        {

        }
        public unsafe Bitmap ExecuteEffect(Bitmap srcBmp)
        {
            int srcWidth = srcBmp.Width,
                 srcHeight = srcBmp.Height;

            Bitmap dstBmp = new Bitmap(srcHeight, srcWidth, PixelFormat.Format32bppRgb);

            BitmapData srcBmpData = srcBmp.LockBits(new Rectangle(0, 0, srcWidth, srcHeight), ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
            BitmapData dstBmpData = dstBmp.LockBits(new Rectangle(0, 0, srcHeight, srcWidth), ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
            try
            {
                byte* curposSrcBmp;
                byte* curposDstBmp;
                for (int i = 0; i < srcHeight; i++)
                {
                    curposSrcBmp = ((byte*)srcBmpData.Scan0) + i * srcBmpData.Stride;
                    for (int k = 0; k < srcWidth; k++)
                    {
                        curposDstBmp = (((byte*)dstBmpData.Scan0) + (srcWidth - 2 - k) * dstBmpData.Stride) + (dstBmpData.Stride + 4 * i);
                        byte* b = &*(curposSrcBmp++);
                        byte* g = &*(curposSrcBmp++);
                        byte* r = &*(curposSrcBmp++);
                        curposSrcBmp++;
                        byte B = *b;
                        byte G = *g;
                        byte R = *r;
                        *curposDstBmp = (byte)B;
                        curposDstBmp++;
                        *curposDstBmp = (byte)G;
                        curposDstBmp++;
                        *curposDstBmp = (byte)R;
                        curposDstBmp++;
                        //curposDstBmp++;
                    }
                }
            }
            finally
            {
                srcBmp.UnlockBits(srcBmpData);
                dstBmp.UnlockBits(dstBmpData);
            }
            return dstBmp;
        }
    }
}
