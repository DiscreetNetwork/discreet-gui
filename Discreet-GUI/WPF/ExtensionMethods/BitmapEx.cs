﻿using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WPF.ExtensionMethods
{
    /// <summary>
    /// Bitmap class to extend static methods
    /// </summary>
    public static class BitmapEx
    {
        /// <summary>
        /// Creates a QRCode System.Drawing.Bitmap and converts it to a Avalonia.Media.Imaging.Bitmap to be used for Avalonia.Controls.Image controls
        /// </summary>
        /// <param name="text">The content to be encoded into the QR Code</param>
        /// <returns>Avalonia specific Bitmap</returns>
        public static Avalonia.Media.Imaging.Bitmap CreateQRCode(string text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            /*QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            //QRC qrCode = new QRCode(qrCodeData);
            System.Drawing.Bitmap qrCodeImage = qrCodeData.GetGraphic(20);

            using MemoryStream ms = new MemoryStream();
            qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Position = 0;*/

            using var ms = new MemoryStream();

            Avalonia.Media.Imaging.Bitmap avaloniaBitmap = new Avalonia.Media.Imaging.Bitmap(ms);
            return avaloniaBitmap;
        }
    }
}
