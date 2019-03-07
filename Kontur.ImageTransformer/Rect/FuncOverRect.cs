using System;

namespace Kontur.ImageTransformer
{
    public static class FuncOverRect
    {
        // Общий прямоугольник при пересечении
        public static bool Intersect(Rect rect1, Rect rect2, ref Rect resultRect)
        {
            if (AreIntersected(rect1, rect2))
            {
                int rIntSquareLeft = Math.Max(Math.Min(rect1.Left, rect1.Right), Math.Min(rect2.Left, rect2.Right));
                int rIntSquareRight = Math.Min(Math.Max(rect1.Left, rect1.Right), Math.Max(rect2.Left, rect2.Right));
                int rIntSquareTop = Math.Max(Math.Min(rect1.Top, rect1.Bottom), Math.Min(rect2.Top, rect2.Bottom));
                int rIntSquareBottom = Math.Min(Math.Max(rect1.Top, rect1.Bottom), Math.Max(rect2.Top, rect2.Bottom));
                if (rIntSquareRight - rIntSquareLeft == 0 || rIntSquareBottom - rIntSquareTop == 0)
                    return false;
                resultRect = new Rect(rIntSquareLeft, rIntSquareTop, rIntSquareRight - rIntSquareLeft, rIntSquareBottom - rIntSquareTop);
                return true;
            }
            else
            {
                //resultRect = new Rectangle();
                return false;
            }
        }

        // Пересекаются ли два прямоугольника (пересечение только по границе НЕ считается пересечением)
        public static bool AreIntersected(Rect rect1, Rect rect2)
        {
            return !((rect1.Right <= rect2.Left) || (rect1.Left >= rect2.Right) || (rect1.Bottom <= rect2.Top) || (rect1.Top >= rect2.Bottom));
        }
    }
}
