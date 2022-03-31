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
            Generate(diameter, address);
        }

        public Color GenColor(List<Color> colors)
        {
            var idx = Generator.Next(0, colors.Count);
            var color = colors[idx];
            colors.RemoveAt(idx);
            return color;
        }

        public static (float h, float s, float v) RGBToHSB(float r, float g, float b)
        {
            /* preconvert r, g, and b to 0-1 range */
            r /= 255.5f;
            g /= 255.5f;
            b /= 255.5f;

            double min = r < g ? r : g;
            min = min < b ? min : b;

            double max = r > g ? r : g;
            max = max > b ? max : b;

            double v = max;
            double h = 0f;
            double s = 0f;
            double delta = max - min;

            if (delta < 0.00001f)
            {
                return ((float)h, (float)s, (float)v);
            }

            if (max > 0)
            {
                s = delta / max;
            }
            else
            {
                return (float.NaN, 0f, (float)v);
            }

            if (r >= max)
            {
                h = (g - b) / delta;
            }
            else if (g >= max)
            {
                h = 2.0 + (b - r) / delta;
            }
            else
            {
                h = 4.0 + (r - g) / delta;
            }

            h *= 60;

            if (h < 0)
            {
                h += 360;
            }

            return ((float)h, (float)s, (float)v);
        }

        public static (float r, float g, float b) HSBToRGB(float h, float s, float v)
        {
            double hh, p, q, t, ff;
            long i;

            double r, g, b;

            if (s <= 0.0)
            {
                r = v * 255.5;
                g = v * 255.5;
                b = v * 255.5;

                return ((float)r, (float)g, (float)b);
            }

            hh = h;
            if (hh >= 360) hh = 0;
            hh /= 60;

            i = (long)Math.Floor(hh);

            ff = hh - i;
            p = v * (1 - s);
            q = v * (1 - s * ff);
            t = v * (1 - (s * (1 - ff)));

            switch (i)
            {
                case 0:
                    r = v * 255 + 0.5;
                    g = t * 255 + 0.5;
                    b = p * 255 + 0.5;
                    break;
                case 1:
                    r = q * 255 + 0.5;
                    g = v * 255 + 0.5;
                    b = p * 255 + 0.5;
                    break;
                case 2:
                    r = p * 255 + 0.5;
                    g = v * 255 + 0.5;
                    b = t * 255 + 0.5;
                    break;
                case 3:
                    r = p * 255 + 0.5;
                    g = q * 255 + 0.5;
                    b = v * 255 + 0.5;
                    break;
                case 4:
                    r = t * 255 + 0.5;
                    g = p * 255 + 0.5;
                    b = v * 255 + 0.5;
                    break;
                case 5:
                default:
                    r = v * 255 + 0.5;
                    g = p * 255 + 0.5;
                    b = q * 255 + 0.5;
                    break;
            }

            return ((float)r, (float)g, (float)b);
        }

        public List<Color> HueShift(List<Color> colors)
        {
            const double wobble = 30;

            var amount = Generator.NextDouble() * wobble - wobble / 2;

            return colors.Select(x =>
            {
                ColorSpaceConverter converter = new ColorSpaceConverter();
                var rgb = x.ToPixel<Rgba32>();
                (var h, var s, var v) = RGBToHSB((float)rgb.R, (float)rgb.G, (float)rgb.B);
                h = (h + (float)amount) % 360f;
                if (h < 0) h += 360f;

                (var r, var g, var b) = HSBToRGB(h, s, v);
                return Color.FromRgb(Convert.ToByte((int)Math.Floor(r)), Convert.ToByte((int)Math.Floor(g)), Convert.ToByte((int)Math.Floor(b)));
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
            var colors = HueShift(new List<Color>(Colors));

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
