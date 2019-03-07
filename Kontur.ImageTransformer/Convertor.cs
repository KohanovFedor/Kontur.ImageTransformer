using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Kontur.ImageTransformer
{
    public static class Convertor
    {

        public static Bitmap RequestToBytePNG(byte[] array)
        {
            int begin = -1, end = array.Length;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 71 && array[i - 1] == 78 && array[i - 2] == 80 && array[i - 3] == 137)
                {
                    begin = i - 3;
                    break;
                }
            }
            for (int i = array.Length - 1; i >= 0; i--)
            {
                if (array[i] == 73 && array[i + 1] == 69 && array[i + 2] == 78 && array[i + 3] == 68 && array[i + 4] == 174 && array[i + 5] == 66 && array[i + 6] == 96 && array[i + 7] == 130)
                {
                    end = i + 8;
                    break;
                }
            
            }
            byte[] res = new byte[end - begin];
            if (begin == -1)
            {
                Bitmap bmp = null;
                return bmp;
            }
            else
            {
                
                Array.Resize(ref array, end);
                Array.Copy(array, begin, res, 0, (end - begin));
                //res = array.Where((c, index) => index >= begin && index <= end).Select(n => n).ToArray();
                return ByteToBitmap(res);
            }
        }

        public static Bitmap ByteToBitmap(byte[] array)
        {
            Bitmap newBitmap;
            using (Stream memoryStream = new MemoryStream(array))
            {
                newBitmap = new Bitmap(memoryStream);
            }
            return newBitmap;
        }

        public static byte[] ImageToByte(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

    }
}
