using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using BinaryToBitmap;

namespace Visual_Entropy
{
	class Program
	{
		// Parameters that are one character long can use '-' or '\' as a leading character.
		// Otherwise, 'word-length' paramters follow the convention of '--' beforehand.
		// The last parameter must always be the output file name/path.
		public static int Main(string[] args)
		{
			int exitCode = 0;
			try
			{
				Validate_Parameters(args);
				int width = Int32.Parse(args[Array.IndexOf(args, "-w") + 1]);
				int height = Int32.Parse(args[Array.IndexOf(args, "-h") + 1]);

				Console.WriteLine("Visual Entropy");
				Console.WriteLine("Copyright (c) 2013 Christopher Robert Philabaum");
				Console.WriteLine();

				string path = "temp.png";

				Random rand = new Random();
				Console.Write("Setting the values... ");
				int[,] pixels = Get_Pixels(rand, width, height);
				Console.WriteLine("done!");

				Console.Write("Saving the picture to {0}... ", path);
				BinaryToBitmap.BinaryToBitmap.ToPNG(width, height, pixels, "temp.png");
				Console.WriteLine("done!");

				Console.WriteLine();
			}
			catch (Exception error)
			{
				Console.WriteLine();
				Console.WriteLine(error.Message);
				Console.WriteLine(error.StackTrace);

				exitCode = 1;
			}
			finally
			{
				Console.WriteLine();
				Console.Write("Press ENTER to continue... ");
				Console.ReadKey();
			}

			return exitCode;
		}

		public static void Validate_Parameters(string[] args)
		{
			for (int index = 0; index < args.Length; index++)
			{
				string param = args[index];
				if (param.Length == 2)
				{
					if (param[0] != '-' && param[0] != '\\') throw new FormatException("Invalid argument.");
					else
					{
						switch (param[1])
						{
							case ('w'):
							case ('h'):
								if (index != args.Length - 1 && (args[index + 1][0] != '-' ||
									args[index + 1][0] != '\\'))
								{
									index++;
								}
								else throw new FormatException(String.Format("Missing value for argument '{0}'.",
									param[1]));
								break;
						}
					}
				}
				else
				{
					if (index != args.Length - 1 && String.Format("{0}{1}", param[0], param[1]) != "--")
					{
						throw new FormatException("Invalid argument.");
					}
				}
			}
		}



		public static int[,] Get_Pixels(Random rand, int width, int height, bool isAlpha = false)
		{
			// Width, Height, RGBa
			int[,] pixels = new int[width, height];
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					byte bit = 0;
					if ((uint)rand.Next(int.MinValue, int.MaxValue) >> 31 > 0) bit = 0xFF;
					pixels[x, y] = 0;
					pixels[x, y] = !isAlpha ? Set_Alpha(pixels[x, y], 255) : Set_Alpha(pixels[x, y], bit);
					pixels[x, y] = Set_Red(pixels[x, y], bit);
					pixels[x, y] = Set_Green(pixels[x, y], bit);
					pixels[x, y] = Set_Blue(pixels[x, y], bit);
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

		public static int Set_Green(int color, byte green)
		{
			byte[] cBytes = Get_RGBa(color);
			color = Convert.ToInt32(cBytes[3]); // Alpha
			color = Convert.ToInt32((color << 8) + cBytes[0]);
			color = Convert.ToInt32((color << 8) + green); // Green
			color = Convert.ToInt32((color << 8) + cBytes[2]); // Blue

			return color;
		}

		public static int Set_Blue(int color, byte blue)
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
