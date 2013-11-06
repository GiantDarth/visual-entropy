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
					Console.Write("{0}", ColorTranslator.ToHtml(Color.FromArgb(pixels[x,y])));
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
					pixels[x,y] = rand.Next();
					/*if (!isAlpha)
					{
						colors[] = 255;
					}*/
				}
			}

			return pixels;
		}
	}
}
