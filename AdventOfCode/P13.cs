using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class P13 : Problem
	{
		public void SolveA()
		{
			var lines = this.ReadInput().ToList();
			var numPoints = lines.IndexOf("");
			var points = lines
				.Take(numPoints)
				.Select(a =>
				{
					var tokens = a.Split(new[] { ',' }).Select(t => int.Parse(t)).ToList();
					return (X: tokens[0], Y: tokens[1]);
				})
				.ToList();
			var folds = lines
				.Skip(numPoints)
				.Where(a => !string.IsNullOrWhiteSpace(a))
				.Select(a =>
				{
					var tokens = a.Split(new[] { "fold along ", "=" }, StringSplitOptions.RemoveEmptyEntries);
					return (Axis: tokens[0], Coord: int.Parse(tokens[1]));
				})
				.ToList();

			List<(int X, int Y)> Fold((string Axis, int Coord) fold)
			{
				return points.Select(p =>
				{
					if( fold.Axis == "x" )
					{
						if( p.X > fold.Coord )
							return (X: 2 * fold.Coord - p.X, p.Y);
					}
					else if( fold.Axis == "y" )
					{
						if( p.Y > fold.Coord )
							return (p.X, Y: 2 * fold.Coord - p.Y);
					}

					return (p.X, p.Y);
				})
					.Distinct()
					.ToList();
			}

			// part 1
			points = Fold(folds[0]);
			Console.WriteLine(points.Count);

			// folding is idempotent, folding by the first fold twice does not make a difference
			foreach( var fold in folds )
			{
				points = Fold(fold);
			}

			var maxX = points.Max(p => p.X);
			var maxY = points.Max(p => p.Y);
			var minX = points.Min(p => p.X);
			var minY = points.Min(p => p.Y);

			points = points.Select(p => (X: p.X - minX, Y: p.Y - minY)).ToList();
			var bmp = new Bitmap(maxX - minX + 1, maxY - minY + 1);
			var gfx = Graphics.FromImage(bmp);
			gfx.Clear(Color.White);
			gfx.Dispose();
			foreach( var p in points )
			{
				bmp.SetPixel(p.X, p.Y, Color.Black);
			}
			bmp.Save("C:/temp/out.bmp");

			Console.WriteLine(points.Count);
		}
	}
}
