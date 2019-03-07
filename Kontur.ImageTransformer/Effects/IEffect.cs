using System.Drawing;

namespace Kontur.ImageTransformer
{
    public interface IEffect
    {
        Bitmap ExecuteEffect(Bitmap srcBmp);
    }
}
