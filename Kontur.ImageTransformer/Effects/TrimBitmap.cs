using System.Drawing;
using System.Drawing.Imaging;

namespace Kontur.ImageTransformer
{
    public static class TrimBitmap
    {
        public unsafe static Bitmap Trim(Bitmap srcBmp, Rect rect)
        {
            Bitmap dstBmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppRgb);

            BitmapData srcBmpData = srcBmp.LockBits(new System.Drawing.Rectangle(rect.Left, rect.Top, rect.Width, rect.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
            BitmapData dstBmpData = dstBmp.LockBits(new System.Drawing.Rectangle(0, 0, rect.Width, rect.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
            try
            {
                byte* curposSrcBmp;
                byte* curposDstBmp;
                for (int i = 0; i < rect.Height; i++)
                {
                    curposSrcBmp = ((byte*)srcBmpData.Scan0) + i * srcBmpData.Stride;
                    curposDstBmp = ((byte*)dstBmpData.Scan0) + i * dstBmpData.Stride;
                    for (int k = 0; k < rect.Width; k++)
                    {
                        byte* b = &*(curposSrcBmp++);
                        byte* g = &*(curposSrcBmp++);
                        byte* r = &*(curposSrcBmp++);
                        curposSrcBmp++;
                        byte B = *b;
                        byte G = *g;
                        byte R = *r;
                        *curposDstBmp = B;
                        curposDstBmp++;
                        *curposDstBmp = G;
                        curposDstBmp++;
                        *curposDstBmp = R;
                        curposDstBmp++;
                        curposDstBmp++;
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
