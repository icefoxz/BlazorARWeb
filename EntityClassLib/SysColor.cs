using System.Drawing;
using System.Globalization;

namespace EntityClassLib;

public static class SysColor
{
    public static string ToHex(this Color color) => $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    public static string ToHex(this Color color, bool includeAlpha) => includeAlpha ? $"#{color.R:X2}{color.G:X2}{color.B:X2}{color.A:X2}" : ToHex(color);
    public static Color FromHex(string hex)
    {
        if (hex.StartsWith("#"))
        {
            hex = hex[1..];
        }
        if (hex.Length == 6)
        {
            hex = $"FF{hex}";
        }
        return Color.FromArgb(
            int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber),
            int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber),
            int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber),
            int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber)
        );
    }
}