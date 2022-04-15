using Services.Jazzicon;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Png;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Extensions
{
    public static class JazziconEx
    {
        public static Avalonia.Media.Imaging.Bitmap IdenticonToAvaloniaBitmap(int diameter, string content)
        {
            var icon = new Services.Jazzicon.Jazzicon(diameter, content);

            using var _ms = new System.IO.MemoryStream();

            var encoder = icon.Identicon.GetConfiguration().ImageFormatsManager.FindEncoder(PngFormat.Instance);
            icon.Identicon.Save(_ms, encoder);

            _ms.Seek(0, System.IO.SeekOrigin.Begin);

            return new Avalonia.Media.Imaging.Bitmap(_ms);
        }
    }
}
