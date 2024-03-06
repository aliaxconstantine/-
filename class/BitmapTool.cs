using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace 兵牌生成器
{
    internal static class BitmapTool
    {
        public static Regex colorRegex = new Regex("^#([0-9a-fA-F]{6}|[0-9a-fA-F]{3})$");

        public static string thisPath = Path.Combine(Application.StartupPath,"连级兵牌");


        public static void CreateDirectory(string path)
        {
            string directory = Path.GetDirectoryName(path);
            string fileName = Path.GetFileName(path);

            if (!Directory.Exists(directory))
            {
                CreateDirectory(directory);
            }

            if (!string.IsNullOrEmpty(fileName))
            {

                Directory.CreateDirectory(path);
            }
        }
        public static Bitmap ConvertTo24Bits(Bitmap bitmap)
        {
            if (bitmap.PixelFormat != PixelFormat.Format24bppRgb)
            {
                Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format24bppRgb);
                using (Graphics graphics = Graphics.FromImage(newBitmap))
                {
                    graphics.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                }

                return newBitmap;
            }

            return bitmap;
        }

        public static Bitmap ChangeColor(Bitmap bitmap,Color oldcolor,Color newcolor)
        {

            for (int i = 0; i < bitmap.Width; i++)
            {
                for(int j = 0;j<bitmap.Height;j++)
                {
                    if (bitmap.GetPixel(i, j) == oldcolor)
                    {
                        bitmap.SetPixel(i, j, newcolor);
                    }
                }
            }
            return bitmap;
        }

        private static Image DrawBadge(string CountryName,string armyName, Image badgeImage, List<string> textList, Image flag,int size)
        {

            Graphics g = Graphics.FromImage(badgeImage);
            
            Font font = new Font("楷体", 6 * size);
            string s = CountryName.First() +" "+ armyName;
            SizeF sizef = g.MeasureString(s, font);
            float text_width = sizef.Width;
            float x = (badgeImage.Width - text_width) / 2;
            g.DrawString(s, font, Brushes.Black, x, 28 * size);

            font = new Font("楷体", 8 * size);
            string text = string.Join("-", textList);
            sizef = g.MeasureString(text, font);
            text_width = sizef.Width;
            x = (badgeImage.Width - text_width) / 2;
            g.DrawString(text, font, Brushes.Black, x, 38 * size);
            g.DrawImage(flag, 7 * size,0,46*size,28*size);

            return badgeImage;
        }

        private static Image DrawBadge(string CountryName, string armyName, Image badgeImage, List<string> textList, Image flag, int size,string direction)
        {

            Graphics g = Graphics.FromImage(badgeImage);

            Font font = new Font("楷体", 6 * size);
            string s = CountryName.First() + " " + armyName;
            SizeF sizef = g.MeasureString(s, font);
            float text_width = sizef.Width;
            float x = (badgeImage.Width - text_width) / 2;
            g.DrawString(s, font, Brushes.Black, x, 28 * size);

            font = new Font("楷体", 8 * size);
            string text = string.Join("-", textList);
            sizef = g.MeasureString(text, font);
            text_width = sizef.Width;
            x = (badgeImage.Width - text_width) / 2;
            g.DrawString(text, font, Brushes.Black, x, 38 * size);
            g.DrawImage(flag, 7 * size, 0, 46 * size, 28 * size);

            return badgeImage;
        }

        public static int StringToInt(string str)
        {
            int result;
            if (!int.TryParse(str, out result))
            {
                result = 0;
            }
            return result;
        }

        public static Bitmap Create3DBadge(Bitmap image, float depth)
        {
            // 创建一个临时位图，用于缓存阴影和高亮效果
            Bitmap temp = new Bitmap(image.Width, image.Height);

            using (Graphics g = Graphics.FromImage(temp))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;

                // 绘制阴影效果
                Rectangle shadowRect = new Rectangle((int)depth, (int)depth, image.Width - (int)depth, image.Height - (int)depth);
                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb((int)(depth * 0.5f), Color.Black)))
                {
                    g.FillRectangle(shadowBrush, shadowRect);
                }

                // 绘制高亮效果
                Rectangle highlightRect = new Rectangle(0, 0, image.Width, image.Height);
                using (LinearGradientBrush highlightBrush = new LinearGradientBrush(highlightRect, Color.FromArgb(128, Color.White), Color.Transparent, LinearGradientMode.Vertical))
                {
                    g.FillRectangle(highlightBrush, highlightRect);
                }
            }

            // 创建一个新位图，并将原始图像绘制到其中
            Bitmap result = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;

                // 绘制阴影效果
                Rectangle shadowRect = new Rectangle(0, 0, image.Width, image.Height);
                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb((int)(depth * 0.5f), Color.Black)))
                {
                    g.FillRectangle(shadowBrush, shadowRect);
                }

                // 绘制原始图像，并在其上叠加阴影和高亮效果
                g.DrawImage(image, 0, 0);
                g.DrawImage(temp, 0, 0);
            }

            // 释放临时位图的资源
            temp.Dispose();

            // 返回结果图像
            return result;
        }


        public static Bitmap DrawInfantryBadge(int width, int height, string color)
        {
            Color colorObj = ColorTranslator.FromHtml(color);

            // 创建一个初始图像，填充为指定的颜色
            Bitmap image = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.Clear(colorObj);
            }

            // 对该图像添加 3D 质感，并返回结果图像
            return Create3DBadge(image, 4f);
        }



        private static Image getFrag(string name)
        {
            string p = Path.Combine(thisPath, name+".jpg");
            var S = Image.FromFile(p);
            return S;
        }

        public static Image DrawArmy(Army army,int size)
        {
           return DrawBadge(army.Country,army.Code,DrawInfantryBadge(65*size,50*size,army.Color),
                new List<string>(){army.Attack.ToString(),army.Organization.ToString(),army.Speed.ToString()},
                getFrag(army.Name),size);
        }
    }
}
