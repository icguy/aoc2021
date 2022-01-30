using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class P15 : Problem
	{
		public void SolveA()
		{
			var lines = this.ReadInputEx();

			var height = lines.Length;
			var width = lines[0].Length;
			var map = new int[height, width];
			var cost = new int[height, width];
			for( int i = 0; i < height; i++ )
			{
				for( int j = 0; j < width; j++ )
				{
					map[i, j] = lines[i][j] - '0';
					cost[i, j] = -1;
				}
			}
			cost[0, 0] = 0;

			var queue = new List<Node>();
			var first = new Node
			{
				I = 0,
				J = 0,
				TotalCost = 0,
				Visited = new List<(int I, int J)>()
			};
			first.H = this.GetH(width, height, first);
			queue.Add(first);

			for( int iter = 0; ; iter++ )
			{
				var curr = queue.First();
				queue.RemoveAt(0);
				//if( iter % 1000 == 0 )
				//{
				//	this.PrintArray(cost, c => (c == -1 ? "" : c.ToString()).PadLeft(3));
				//}
				if( curr.I == height - 1 && curr.J == width - 1 )
				{
					Console.WriteLine(curr.TotalCost);
					break;
				}
				var neighbors = this.GetNeighbors(map, curr.I, curr.J)
					.Where(n => cost[n.I, n.J] == -1)
					.Select(n =>
					{
						var next = new Node
						{
							I = n.I,
							J = n.J,
							TotalCost = curr.TotalCost + map[n.I, n.J],
							Visited = curr.Visited.ToList()
						};
						next.Visited.Add((curr.I, curr.J));
						next.H = this.GetH(width, height, next);
						return next;
					});

				foreach( var n in neighbors )
				{
					queue.Add(n);
					cost[n.I, n.J] = n.TotalCost;
				}
				queue = queue.OrderBy(n => n.H).ToList();
			}
		}

		public void SolveB()
		{
			var lines = this.ReadInputEx();

			var smallHeight = lines.Length;
			var smallWidth = lines[0].Length;

			var height = smallHeight * 5;
			var width = smallWidth * 5;

			var map = new int[height, width];
			var cost = new int[height, width];
			for( int i = 0; i < smallHeight; i++ )
			{
				for( int j = 0; j < smallWidth; j++ )
				{
					for( int ii = 0; ii < 5; ii++ )
					{
						for( int jj = 0; jj < 5; jj++ )
						{
							var ai = ii * smallHeight + i;
							var aj = ii * smallWidth + j;

							var value = lines[i][j] - '0' + ii + jj;
							value = (value - 1) % 9 + 1;
							map[ai, aj] = value;
							cost[ai, aj] = -1;
						}
					}
				}
			}
			cost[0, 0] = 0;

			var queue = new List<Node>();
			var first = new Node
			{
				I = 0,
				J = 0,
				TotalCost = 0,
				Visited = new List<(int I, int J)>()
			};
			first.H = this.GetH(width, height, first);
			queue.Add(first);

			for( int iter = 0; ; iter++ )
			{
				if( iter % 1000 == 0 )
				{
					this.PrintArray(cost, c => (c == -1 ? "" : c.ToString()).PadLeft(3));
				}
				var curr = queue.First();
				queue.RemoveAt(0);
				if( curr.I == height - 1 && curr.J == width - 1 )
				{
					Console.WriteLine(curr.TotalCost);
					break;
				}
				var neighbors = this.GetNeighbors(map, curr.I, curr.J)
					.Where(n => cost[n.I, n.J] == -1)
					.Select(n =>
					{
						var next = new Node
						{
							I = n.I,
							J = n.J,
							TotalCost = curr.TotalCost + map[n.I, n.J],
							Visited = curr.Visited.ToList()
						};
						next.Visited.Add((curr.I, curr.J));
						next.H = this.GetH(width, height, next);
						return next;
					});

				foreach( var n in neighbors )
				{
					queue.Add(n);
					cost[n.I, n.J] = n.TotalCost;
				}
				queue = queue.OrderBy(n => n.H).ToList();
			}
		}

		int GetH(int w, int h, Node n)
		{
			return n.TotalCost + w - n.J + h - n.I;
		}

		class Node
		{
			public int I { get; set; }
			public int J { get; set; }
			public int TotalCost { get; set; }
			public int H { get; set; }
			public List<(int I, int J)> Visited { get; set; }
		}
	}
}
