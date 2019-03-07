using System;

namespace Kontur.ImageTransformer
{
    public struct Rect
    {
        public Rect(int left, int top, int width, int height)
        {

            if (width<0)
            {
                int oldLeft = left;
                left = left + width;
                width = oldLeft - left;
            }
            if(height<0)
            {
                int oldTop = top;
                top = top + height;
                height = oldTop - top;
            }
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }
        
        public override string ToString()
        {
            return String.Format("Left: {0}, Top: {1}, Width: {2}, Height: {3}", Left, Top, Width, Height);
        }

        public readonly int Left, Top, Width, Height;
        public int Bottom { get { return Top + Height; } }
        public int Right { get { return Left + Width; } }
    }
}
