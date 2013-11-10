using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using VisualEntropy.BinaryToBitmap;

namespace C7Theory.VisualEntropy
{
	class Program
	{
		// Parameters that are one character long can use only '-' as a leading character.
		// Otherwise, 'word-length' paramters follow the convention of '--' beforehand.
		// The last parameter must always be the output file name/path.
		public static int Main(string[] args)
		{
			int exitCode = 0;
			if (args.Contains("--help"))
			{
				Display_Help();
				exitCode = 1;
			}
			else try
			{
				Validate_Parameters(args);
				int width = 256;
				if (args.Contains("-w") || args.Contains("--width"))
				{
					width = Int32.Parse(args[Array.IndexOf(args, "-w") + 1]);
				}
				int height = 256;
				if (args.Contains("-h") || args.Contains("--height"))
				{
					height = Int32.Parse(args[Array.IndexOf(args, "-h") + 1]);
				}
#if DEBUG
				string path = "temp.png";
				if (args.Length > 0)
				{
					path = args[args.Length - 1];
				}
#else
				if (args.Length < 1)
				{
					Display_Help();
					return 1;
				}
				string path = args[args.Length - 1];
#endif
				// Converts the path to a universal, absolute path that can be used on both Windows and Unix-based paltforms.
				path = new Uri(Path.GetFullPath(path)).LocalPath;

				Console.WriteLine("Visual Entropy");
				Console.WriteLine("Copyright (c) 2013 Christopher Robert Philabaum");
				Console.WriteLine();

				Random rand = new Random();
				Console.Write("Setting the values... ");
				int[,] pixels = Get_Pixels(rand, width, height);
				Console.WriteLine("done!");

				Console.Write("Saving the picture to {0}... ", path);
				BinaryToBitmap.ToPNG(width, height, pixels, "temp.png");
				Console.WriteLine("done!");
#if DEBUG
				System.Diagnostics.Process.Start(path);
#endif
			}
			catch (Exception error)
			{
				Console.WriteLine();
				Console.WriteLine(error.Message);
				exitCode = 1;
#if DEBUG
				Console.WriteLine(error.StackTrace);
			}
			finally
			{
				Console.WriteLine();
				Console.Write("Press ENTER to continue... ");
				Console.ReadKey();
#endif
			}

			return exitCode;
		}

		public static void Validate_Parameters(string[] args)
		{
			for (int index = 0; index < args.Length; index++)
			{
				string param = args[index];
				if (param[0] == '-')
				{
					switch (param[1])
					{
						case ('w'):
						case ('h'):
							if (index != args.Length - 1 && (args[index + 1][0] != '-' || args[index + 1][0] != '\\'))
							{
								index++;
							}
							else
							{
								throw new FormatException(String.Format("Missing value for argument '{0}'.",
									param[1]));
							}
							break;
						case ('-'):
							if (index != args.Length - 1 && String.Format("{0}{1}", param[0], param[1]) != "--")
							{
								throw new FormatException("Invalid argument.");
							}
							break;
						default:
							if (index != args.Length - 1)
								throw new FormatException("Invalid argument.");
							break;
					}
				}

				if (!args[args.Length - 1].EndsWith(".png") && !args[args.Length - 1].EndsWith(".PNG"))
				{
					throw new FormatException("The file doesn't end with '.png' or '.PNG'.");
				}
			}
		}

		public static void Display_Help()
		{
			Console.WriteLine("Visual Entropy");
			Console.WriteLine("Copyright (c) 2013 Christopehr Robert Philabaum");
			Console.WriteLine();
			Console.WriteLine("Usage: Visual Entropy [options] path/filename.png");
			Console.WriteLine("Options:");
			Console.WriteLine("  --help\t\t\tThe help display.");
			Console.WriteLine("  -w pixels | --width pixels\tPicture width. Defaults to 256.");
			Console.WriteLine("  -h pixels | --height pixels\tPicture height. Defaults to 256.");

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
