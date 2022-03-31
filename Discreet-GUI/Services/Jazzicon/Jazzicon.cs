using static Services.Jazzicon.RoundExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.ColorSpaces;

namespace Services.Jazzicon
{
    /// <summary>
    /// Implements Jazzicons (https://github.com/danfinlay/jazzicon)
    /// </summary>
    public class Jazzicon
    {
        public static Color[] Colors = new Color[]
        {
            Color.ParseHex("#01888C"), // teal
            Color.ParseHex("#FC7500"), // bright orange
            Color.ParseHex("#034F5D"), // dark teal
            Color.ParseHex("#F73F01"), // orangered
            Color.ParseHex("#FC1960"), // magenta
            Color.ParseHex("#C7144C"), // raspberry
            Color.ParseHex("#F3C100"), // goldenrod
            Color.ParseHex("#1598F2"), // lightning blue
            Color.ParseHex("#2465E1"), // sail blue
            Color.ParseHex("#F19E02"), // gold
        };

        public static int AddressToNumber(string address)
        {
            return BitConverter.ToInt32(SHA256.HashData(SHA256.HashData(Encoding.UTF8.GetBytes(address))), 0);
        }

        public Random Generator { get; private set; }
        public int Seed { get; private set; }

        public Image<Rgba32> Identicon { get; private set; }

        public Jazzicon(int diameter, string address)
        {
            Generate(160, address);
        }

        public Color GenColor(List<Color> colors)
        {
            var idx = Generator.Next(0, colors.Count);
            var color = colors[idx];
            colors.RemoveAt(idx);
            return color;
        }

        public static Color HSBtoRGB(float hue, float saturation, float brightness)
        {
            int r = 0, g = 0, b = 0;
            if (saturation == 0)
            {
                r = g = b = (int)(brightness * 255.0f + 0.5f);
            }
            else
            {
                float h = (hue - (float)Math.Floor(hue)) * 6.0f;
                float f = h - (float)Math.Floor(h);
                float p = brightness * (1.0f - saturation);
                float q = brightness * (1.0f - saturation * f);
                float t = brightness * (1.0f - (saturation * (1.0f - f)));
                switch ((int)h)
                {
                    case 0:
                        r = (int)(brightness * 255.0f + 0.5f);
                        g = (int)(t * 255.0f + 0.5f);
                        b = (int)(p * 255.0f + 0.5f);
                        break;
                    case 1:
                        r = (int)(q * 255.0f + 0.5f);
                        g = (int)(brightness * 255.0f + 0.5f);
                        b = (int)(p * 255.0f + 0.5f);
                        break;
                    case 2:
                        r = (int)(p * 255.0f + 0.5f);
                        g = (int)(brightness * 255.0f + 0.5f);
                        b = (int)(t * 255.0f + 0.5f);
                        break;
                    case 3:
                        r = (int)(p * 255.0f + 0.5f);
                        g = (int)(q * 255.0f + 0.5f);
                        b = (int)(brightness * 255.0f + 0.5f);
                        break;
                    case 4:
                        r = (int)(t * 255.0f + 0.5f);
                        g = (int)(p * 255.0f + 0.5f);
                        b = (int)(brightness * 255.0f + 0.5f);
                        break;
                    case 5:
                        r = (int)(brightness * 255.0f + 0.5f);
                        g = (int)(p * 255.0f + 0.5f);
                        b = (int)(q * 255.0f + 0.5f);
                        break;
                }
            }
            return Color.FromRgb(Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b));
        }

        public List<Color> HueShift(List<Color> colors)
        {
            const double wobble = 30;

            var amount = Generator.NextDouble() * 30 - wobble / 2;

            return colors.Select(x =>
            {
                ColorSpaceConverter converter = new ColorSpaceConverter();
                var rgbPixel = x.ToPixel<Rgb24>();
                var rgb = new Rgb(rgbPixel.R, rgbPixel.G, rgbPixel.B);
                var hsv = converter.ToHsv(rgb);
                var v = hsv.V;
                var s = hsv.S;
                var h = (hsv.H + (float)amount) % 360;
                if (h < 0) h += 360f;

                return HSBtoRGB(h, s, v);
            }).ToList();
        }

        public void GenShape(List<Color> colors, int diameter, int i, int total, Image<Rgba32> img)
        {
            var center = diameter / 2;

            var rect = new RectangularPolygon(0, 0, diameter, diameter);
            var shape = new PathCollection(rect.AsClosedPath());
            var firstRot = Generator.NextDouble();
            var angle = Math.PI * 2 * firstRot;
            var velocity = diameter / total * Generator.NextDouble() + (i * diameter / total);

            var tx = (Math.Cos(angle) * velocity);
            var ty = (Math.Sin(angle) * velocity);
            var translatedShape = shape.Translate((float)tx, (float)ty);

            var secondRot = Generator.NextDouble();
            var rot = (firstRot * 360) + (secondRot * 180);

            var rotatedShape = translatedShape.RotateDegree((float)rot);

            img.Mutate(i => i.Fill(GenColor(colors), rotatedShape));
        }

        public void Generate(int diameter, string address)
        {
            Seed = AddressToNumber(address);
            Generator = new Random(Seed);

            Identicon = new Image<Rgba32>(diameter, diameter);
            var colors = new List<Color>(Colors);

            /* first fill */
            Identicon.Mutate(i => i.Fill(GenColor(colors)));

            /* now perform writing of squares */
            const int shapeCount = 8;

            for (int i = 0; i < shapeCount - 1; i++)
            {
                GenShape(colors, diameter, i, shapeCount - 1, Identicon);
            }

            /* apply blur */
            Identicon.Mutate(i => i.GaussianBlur(0.5f));

            /* finish with crop into circle */
            Identicon.Mutate(i => i.ConvertToAvatar(new Size(diameter, diameter), diameter / 2));
        }

        public void Save(string path)
        {
            Identicon.Save(path);
        }

        public static void GenerateAndSave(int diameter, string address, string path)
        {
            Jazzicon icon = new Jazzicon(diameter, address);

            icon.Identicon.Save(path);
        }
    }
}
