using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class P09 : Problem
	{
		public void SolveA()
		{
			var lines = this.ReadInput("p09.txt");
			var map = lines.Select(l => l.Select(a => int.Parse(a.ToString())).ToArray()).ToArray();
			var sum = 0;
			for( int i = 0; i < map.Length; i++ )
			{
				for( int j = 0; j < map[i].Length; j++ )
				{
					if( this.GetNeighbors(map, i, j).All(n => n > map[i][j]) )
						sum += 1 + map[i][j];
				}
			}
			Console.WriteLine(sum);
		}

		public void SolveB()
		{
			var lines = this.ReadInput("p09.txt");
			var map = lines.Select(l => l.Select(a => int.Parse(a.ToString())).ToArray()).ToArray();
			var sizes = new List<int>();
			var width = map[0].Length;
			var height = map.Length;

			var check = new bool[height, width];
			var queue = new Queue<(int i, int j)>();

			for( int i = 0; i < map.Length; i++ )
			{
				for( int j = 0; j < map[i].Length; j++ )
				{
					if( map[i][j] == 9 || check[i, j] ) continue;

					// do flood fill
					queue.Enqueue((i, j));
					check[i, j] = true;
					var size = 0;
					while( queue.Count > 0 )
					{
						var (ci, cj) = queue.Dequeue();
						size++;
						foreach( var (ni, nj) in this.GetNeighborCoords(map, ci, cj) )
						{
							if( !check[ni, nj] && map[ni][nj] != 9 )
							{
								check[ni, nj] = true;
								queue.Enqueue((ni, nj));
							}
						}
					}
					sizes.Add(size);
				}
			}

			var result = sizes.OrderByDescending(a => a).Take(3).Aggregate(1, (acc, curr) => acc * curr);
			Console.WriteLine(result);
		}

		public bool IsInBounds(int[][] map, int r, int c) => r < map.Length && 0 <= r && 0 <= c && c < map[r].Length;
		public IEnumerable<int> GetNeighbors(int[][] map, int r, int c) => this.GetNeighborCoords(map, r, c).Select(p => map[p.i][p.j]);
		public IEnumerable<(int i, int j)> GetNeighborCoords(int[][] map, int r, int c)
		{
			if( this.IsInBounds(map, r - 1, c) ) yield return (r - 1, c);
			if( this.IsInBounds(map, r + 1, c) ) yield return (r + 1, c);
			if( this.IsInBounds(map, r, c - 1) ) yield return (r, c - 1);
			if( this.IsInBounds(map, r, c + 1) ) yield return (r, c + 1);
		}
	}
}
