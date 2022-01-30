using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
	class Problem
	{
		public const string Root = "C:/projektek/sajat/adventofcode/problems/";

		protected string[] ReadInput(string file) => File.ReadAllLines(Root + file);
		protected string[] ReadInput() => this.ReadInput(this.FileName);
		protected string[] ReadInputEx() => this.ReadInput(this.ExampleFileName);

		protected string FileName => this.GetType().Name.ToLower() + ".txt";
		protected string ExampleFileName => this.GetType().Name.ToLower() + "ex.txt";

		protected void PrintArray<T>(T[,] array, Func<T, string> transform = null, Func<T, ConsoleColor> color = null, string separator = "")
		{
			this.PrintArray2(array, (c, i, j) => transform(c), color, separator);
		}

		protected void PrintArray2<T>(T[,] array, Func<T, int, int, string> transform = null, Func<T, ConsoleColor> color = null, string separator = "")
		{
			for( int i = 0; i < array.GetLength(0); i++ )
			{
				for( int j = 0; j < array.GetLength(1); j++ )
				{
					var el = array[i, j];
					var s = transform?.Invoke(el, i, j) ?? el.ToString();
					if( color != null )
						Console.ForegroundColor = color(el);
					Console.Write(s);
					Console.Write(separator);
				}
				Console.WriteLine();
			}
			Console.WriteLine();
		}

		protected bool InRange<T>(T[,] map, int i, int j) => i >= 0 && j >= 0 && map.GetLength(0) > i && map.GetLength(1) > j;
		protected IEnumerable<(int I, int J)> GetNeighbors<T>(T[,] map, int i, int j)
		{
			return this.GetNeighborsInner(map, i, j).Where(a => this.InRange(map, a.I, a.J));
		}
		private IEnumerable<(int I, int J)> GetNeighborsInner<T>(T[,] map, int i, int j)
		{
			yield return (i + 1, j);
			yield return (i - 1, j);
			yield return (i, j + 1);
			yield return (i, j - 1);
		}


	}
}
