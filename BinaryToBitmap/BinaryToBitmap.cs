using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace VisualEntropy.BinaryToBitmap
{
	public static class BinaryToBitmap
	{
		public static void ToPNG(int width, int height, int[,] pixels, string path = "temp.png")
		{
			Bitmap bitmap = new Bitmap(width, height);
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					bitmap.SetPixel(x, y, Color.FromArgb(pixels[x, y]));
				}
			}

			bitmap.Save(path, ImageFormat.Png);
		}

		private static void ConvertTo_Unix_Path(string path)
		{

		}
	}
}