using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class P11 : Problem
	{
		public void SolveA()
		{
			var lines = this.ReadInput("p11.txt");
			var height = lines.Length;
			var width = lines[0].Length;
			var map = new int[height, width];
			for( int i = 0; i < height; i++ )
			{
				var line = lines[i];
				for( int j = 0; j < width; j++ )
				{
					map[i, j] = int.Parse(line[j].ToString());
				}
			}

			var flashQueue = new Queue<(int i, int j)>();
			var numFlashes = 0;
			for( int n = 0; n < 100; n++ )
			{
				this.PrintArray(map);

				for( int i = 0; i < height; i++ )
				{
					for( int j = 0; j < width; j++ )
					{
						map[i, j]++;
						if( map[i, j] > 9 )
						{
							map[i, j] = 0;
							numFlashes++;
							flashQueue.Enqueue((i, j));
						}
					}
				}

				while( flashQueue.Count > 0 )
				{
					var (ci, cj) = flashQueue.Dequeue();
					foreach( var (ni, nj) in this.GetNeighborCoords(map, ci, cj) )
					{
						if( map[ni, nj] != 0 )
						{
							map[ni, nj]++;
							if( map[ni, nj] > 9 )
							{
								map[ni, nj] = 0;
								numFlashes++;
								flashQueue.Enqueue((ni, nj));
							}
						}
					}
				}
			}
			Console.WriteLine(numFlashes);
		}

		public void SolveB()
		{
			var lines = this.ReadInput("p11ex.txt");
			var height = lines.Length;
			var width = lines[0].Length;
			var map = new int[height, width];
			for( int i = 0; i < height; i++ )
			{
				var line = lines[i];
				for( int j = 0; j < width; j++ )
				{
					map[i, j] = int.Parse(line[j].ToString());
				}
			}

			var flashQueue = new Queue<(int i, int j)>();
			var numFlashes = 0;
			for( int n = 0; ; n++ )
			{
				var numFlashesBefore = numFlashes;
				this.PrintArray(map, color: (a) => a == 0 ? ConsoleColor.Green : ConsoleColor.White);

				for( int i = 0; i < height; i++ )
				{
					for( int j = 0; j < width; j++ )
					{
						map[i, j]++;
						if( map[i, j] > 9 )
						{
							map[i, j] = 0;
							numFlashes++;
							flashQueue.Enqueue((i, j));
						}
					}
				}

				while( flashQueue.Count > 0 )
				{
					var (ci, cj) = flashQueue.Dequeue();
					foreach( var (ni, nj) in this.GetNeighborCoords(map, ci, cj) )
					{
						if( map[ni, nj] != 0 )
						{
							map[ni, nj]++;
							if( map[ni, nj] > 9 )
							{
								map[ni, nj] = 0;
								numFlashes++;
								flashQueue.Enqueue((ni, nj));
							}
						}
					}
				}

				if( numFlashes - numFlashesBefore == height * width )
				{
					Console.WriteLine(n + 1);
					break;
				}
			}
		}

		public bool IsInBounds(int[,] map, int i, int j) => 0 <= i && 0 <= j && i < map.GetLength(0) && j < map.GetLength(1);
		public IEnumerable<(int i, int j)> GetNeighborCoords(int[,] map, int i, int j)
		{
			if( this.IsInBounds(map, i - 1, j) ) yield return (i - 1, j);
			if( this.IsInBounds(map, i + 1, j) ) yield return (i + 1, j);
			if( this.IsInBounds(map, i, j - 1) ) yield return (i, j - 1);
			if( this.IsInBounds(map, i, j + 1) ) yield return (i, j + 1);
			if( this.IsInBounds(map, i - 1, j - 1) ) yield return (i - 1, j - 1);
			if( this.IsInBounds(map, i - 1, j + 1) ) yield return (i - 1, j + 1);
			if( this.IsInBounds(map, i + 1, j - 1) ) yield return (i + 1, j - 1);
			if( this.IsInBounds(map, i + 1, j + 1) ) yield return (i + 1, j + 1);
		}
	}
}
