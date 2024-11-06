using System.Drawing;
using QRCoder;

#if NETFRAMEWORK || NETSTANDARD2_0 || NET5_0 || NET6_0_WINDOWS || NET8_0_OR_GREATER
namespace FluentAr.Services.QrSrv;

#if NET6_0_WINDOWS
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
public static class ArtQRCodeHelper
{
    /// <summary>
    /// Helper function to create an ArtQRCode graphic with a single function call
    /// </summary>
    /// <param name="plainText">Text/payload to be encoded inside the QR code</param>
    /// <param name="pixelsPerModule">Amount of px each dark/light module of the QR code shall take place in the final QR code image</param>
    /// <param name="darkColor">Color of the dark modules</param>
    /// <param name="lightColor">Color of the light modules</param>
    /// <param name="backgroundColor">Color of the background</param>
    /// <param name="eccLevel">The level of error correction data</param>
    /// <param name="forceUtf8">Shall the generator be forced to work in UTF-8 mode?</param>
    /// <param name="utf8BOM">Should the byte-order-mark be used?</param>
    /// <param name="eciMode">Which ECI mode shall be used?</param>
    /// <param name="requestedVersion">Set fixed QR code target version.</param>
    /// <param name="backgroundImage">A bitmap object that will be used as background picture</param>
    /// <param name="pixelSizeFactor">Value between 0.0 to 1.0 that defines how big the module dots are. The bigger the value, the less round the dots will be.</param>
    /// <param name="drawQuietZones">If true a white border is drawn around the whole QR Code</param>
    /// <param name="quietZoneRenderingStyle">Style of the quiet zones</param>
    /// <param name="backgroundImageStyle">Style of the background image (if set). Fill=spanning complete graphic; DataAreaOnly=Don't paint background into quietzone</param>
    /// <param name="finderPatternImage">Optional image that should be used instead of the default finder patterns</param>
    /// <returns>QRCode graphic as bitmap</returns>
    public static Bitmap GetQRCode(string plainText, int pixelsPerModule, Color darkColor, Color lightColor, Color backgroundColor, QRCodeGenerator.ECCLevel eccLevel, bool forceUtf8 = false,
        bool utf8BOM = false, QRCodeGenerator.EciMode eciMode = QRCodeGenerator.EciMode.Default, int requestedVersion = -1, Bitmap backgroundImage = null, double pixelSizeFactor = 0.8,
        bool drawQuietZones = true, ArtQRCode.QuietZoneStyle quietZoneRenderingStyle = ArtQRCode.QuietZoneStyle.Flat,
        ArtQRCode.BackgroundImageStyle backgroundImageStyle = ArtQRCode.BackgroundImageStyle.DataAreaOnly, Bitmap finderPatternImage = null)
    {
        using (var qrGenerator = new QRCodeGenerator())
        using (var qrCodeData = qrGenerator.CreateQrCode(plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion))
        using (var qrCode = new ArtQRCode(qrCodeData))
            return qrCode.GetGraphic(pixelsPerModule, darkColor, lightColor, backgroundColor, backgroundImage, pixelSizeFactor, drawQuietZones, quietZoneRenderingStyle, backgroundImageStyle, finderPatternImage);
    }
}
#endif