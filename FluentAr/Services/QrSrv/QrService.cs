using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;

namespace FluentAr.Services.QrSrv
{
    public class QrService
    {
        public Bitmap GenerateQRCodeWithLogo(string qrText, Image? logo = null)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new ArtQRCode(qrCodeData);
            // 生成QR码图像
            var qrCodeImage = qrCode.GetGraphic(20);

            if (logo != null)
            {
                // 加载并插入Logo
                var graphics = Graphics.FromImage(qrCodeImage);
                var left = qrCodeImage.Width / 2 - logo.Width / 2;
                var top = qrCodeImage.Height / 2 - logo.Height / 2;
                graphics.DrawImage(logo, new Point(left, top));
            }

            return qrCodeImage;
        }
        public Bitmap GenerateQRCodeWithBg(string qrText, Image? bg = null)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new ArtQRCode(qrCodeData);
            // 生成QR码图像
            Bitmap qrCodeImage;

            if (bg != null)
            {
                using var bgImage = new Bitmap(bg, qrCode.GetGraphic(20).Size);
                qrCodeImage = qrCode.GetGraphic(bgImage);
            }
            else qrCodeImage = qrCode.GetGraphic(20);

            return qrCodeImage;
        }

        public string GenerateBase64QRCode(string qrText)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new Base64QRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }

        public string ImageToBase64(Bitmap image)
        {
            using var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            var imageBytes = stream.ToArray();
            var base64String = Convert.ToBase64String(imageBytes);
            return $"data:image/png;base64,{base64String}";
        }
    }
}