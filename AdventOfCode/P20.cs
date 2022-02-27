using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class P20 : Problem
	{
		private const char _dark = '.';
		private const char _light = '#';

		public void SolveA()
		{
			var lines = this.ReadInput();
			var enhancer = lines[0];
			var pixels = new char[lines.Length - 2, lines[2].Length];
			for( int i = 0; i < pixels.GetLength(0); i++ )
			{
				for( int j = 0; j < pixels.GetLength(1); j++ )
				{
					pixels[i, j] = lines.Skip(2 + i).First()[j];
				}
			}

			var image = new Image
			{
				Pixels = pixels,
				Padding = _dark,
			};

			for( int i = 0; i < 50; i++ )
			{
				image = this.Iterate(image, enhancer);
			}

			//this.Print(image);

			var sumLit = image.Pixels.Cast<char>().Count(a => a == _light);
			Console.WriteLine(sumLit);
		}

		private Image Iterate(Image prev, string enhancer)
		{
			var newPadding = prev.Padding == _dark ? enhancer[0] : enhancer.Last();
			var nh = prev.Height + 2;
			var nw = prev.Width + 2;
			var newPixels = new char[nh, nw];
			for( int i = 0; i < nh; i++ )
			{
				for( int j = 0; j < nw; j++ )
				{
					int pi = i - 1;
					int pj = j - 1;
					newPixels[i, j] = this.CalcValue(prev, enhancer, pi, pj);
				}
			}
			return new Image
			{
				Pixels = newPixels,
				Padding = newPadding
			};
		}

		private char CalcValue(Image prev, string enhancer, int pi, int pj)
		{
			var num = 0;
			for( int i = pi - 1; i <= pi + 1; i++ )
			{
				for( int j = pj - 1; j <= pj + 1; j++ )
				{
					var c = prev.InBounds(i, j) ? prev.Pixels[i, j] : prev.Padding;
					num <<= 1;
					if( c == _light )
						num += 1;
				}
			}
			return enhancer[num];
		}

		private void Print(Image i)
		{
			this.PrintArray(i.Pixels, a => a.ToString());
			Console.WriteLine(i.Padding);
		}

		struct Image
		{
			public char[,] Pixels { get; set; }
			public char Padding { get; set; }
			public int Height => this.Pixels.GetLength(0);
			public int Width => this.Pixels.GetLength(1);

			public bool InBounds(int i, int j) => i >= 0 && i < this.Height && j >= 0 && j < this.Width;
		}
	}
}
