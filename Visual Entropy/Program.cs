using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Visual_Entropy
{
	class Program
	{
		public static void Main(string[] args)
		{
			int width = 50;
			int height = 50;

			Random rand = new Random();
			int[,] pixels = Get_Pixels(rand, width, height);

			for(int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					Console.Write("#{0}", Color.FromArgb(pixels[x,y]).Name);
					if (y < height - 1) Console.Write(",");
				}
				Console.WriteLine();
			}

			Console.WriteLine();
			Console.Write("Press ENTER to continue... ");
			Console.ReadKey();
		}



		public static int[,] Get_Pixels(Random rand, int width, int height, bool isAlpha = false)
		{
			// Width, Height, RGBa
			int[,] pixels = new int[width,height];
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					byte b = Convert.ToByte((uint)rand.Next() >> 24);
					pixels[x, y] = 0;
					pixels[x, y] = !isAlpha ? Set_Alpha(pixels[x, y], 255) : Set_Alpha(pixels[x,y], b);
					pixels[x, y] = Set_Red(pixels[x, y], b);
					pixels[x, y] = Set_Green(pixels[x, y], b);
					pixels[x, y] = Set_Blue(pixels[x, y], b);
				}
			}

			return pixels;
		}



		public static byte[] Get_RGBa(int color)
		{
			// Red, Green, Blue, Alpha
			byte[] cBytes = new byte[4];
			cBytes[0] = Convert.ToByte((color >> 16) & 0x0FF);
			cBytes[1] = Convert.ToByte((color >> 8) & 0x0FF);
			cBytes[2] = Convert.ToByte(color & 0x0FF);
			cBytes[3] = Convert.ToByte((color >> 24) & 0x0FF);

			return cBytes;
		}

		public static byte[] Get_RGBa(Color color)
		{
			return Get_RGBa(color.ToArgb());
		}

		public static int Set_Red(int color, byte red)
		{
			byte[] cBytes = Get_RGBa(color);
			color = Convert.ToInt32(cBytes[3]); // Alpha
			color = Convert.ToInt32((color << 8) + red);
			color = Convert.ToInt32((color << 8) + cBytes[1]); // Green
			color = Convert.ToInt32((color << 8) + cBytes[2]); // Blue

			return color;
		}

		public static int Set_Green(int color,  byte green)
		{
			byte[] cBytes = Get_RGBa(color);
			color = Convert.ToInt32(cBytes[3]); // Alpha
			color = Convert.ToInt32((color << 8) + cBytes[0]);
			color = Convert.ToInt32((color << 8) + green); // Green
			color = Convert.ToInt32((color << 8) + cBytes[2]); // Blue

			return color;
		}

		public static int Set_Blue(int color,  byte blue)
		{
			byte[] cBytes = Get_RGBa(color);
			color = Convert.ToInt32(cBytes[3]); // Alpha
			color = Convert.ToInt32((color << 8) + cBytes[0]); // Red
			color = Convert.ToInt32((color << 8) + cBytes[1]); // Green
			color = Convert.ToInt32((color << 8) + blue); // Blue

			return color;
		}

		public static int Set_Alpha(int color, byte alpha)
		{
			byte[] cBytes = Get_RGBa(color);
			color = Convert.ToInt32(alpha);
			color = Convert.ToInt32((color << 8) + cBytes[0]); // Red
			color = Convert.ToInt32((color << 8) + cBytes[1]); // Green
			color = Convert.ToInt32((color << 8) + cBytes[2]); // Blue

			return color;
		}
	}
}
