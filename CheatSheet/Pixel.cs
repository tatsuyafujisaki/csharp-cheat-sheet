using System.Drawing;

namespace CheatSheet
{
    static class Pixel
    {
        static readonly Graphics G = Graphics.FromImage(new Bitmap(1, 1));

        static string PadLeftInPixel(Font f, string s, float targetWidth)
        {
            while (GetWidth(f, s) < targetWidth)
            {
                s = ' ' + s;
            }

            return s;
        }

        static string PadRightInPixel(Font f, string s, float targetWidth)
        {
            while (GetWidth(f, s) < targetWidth)
            {
                s += ' ';
            }

            return s;
        }

        static float GetWidth(Font f, string s)
        {
            using (var sf = new StringFormat(StringFormatFlags.MeasureTrailingSpaces))
            {
                return G.MeasureString(s, f, 0, sf).Width;
            }
        }
    }
}
