using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class P12 : Problem
	{
		public void SolveA()
		{
			var lines = this.ReadInput("p12.txt");
			var edges = lines.Select(a => a.Split(new[] { '-' })).ToList();
			var nodes = edges
				.SelectMany(a => a)
				.Distinct()
				.Select(a => new Node
				{
					Name = a,
					IsLarge = a.ToUpper() == a,
					Neighbors = new List<Node>()
				})
				.ToList();

			foreach( var edge in edges )
			{
				var p1 = nodes.First(a => a.Name == edge[0]);
				var p2 = nodes.First(a => a.Name == edge[1]);
				p1.Neighbors.Add(p2);
				p2.Neighbors.Add(p1);
			}

			var start = nodes.First(a => a.Name == "start");
			var end = nodes.First(a => a.Name == "end");

			var paths = new Queue<List<Node>>();
			paths.Enqueue(new List<Node> { start });
			var numPaths = 0;
			while( paths.Count > 0 )
			{
				var curr = paths.Dequeue();
				var nexts = curr.Last().Neighbors.Where(a => !curr.Contains(a) || a.IsLarge).ToList();
				if( nexts.Count > 0 )
				{
					foreach( var next in nexts )
					{
						if( next == end )
						{
							numPaths++;
						}
						else
						{
							var nextPath = curr.ToList();
							nextPath.Add(next);
							paths.Enqueue(nextPath);
						}
					}
				}
			}
			Console.WriteLine(numPaths);
		}

		public void SolveB()
		{
			var lines = this.ReadInput("p12.txt");
			var edges = lines.Select(a => a.Split(new[] { '-' })).ToList();
			var nodes = edges
				.SelectMany(a => a)
				.Distinct()
				.Select(a => new Node
				{
					Name = a,
					IsLarge = a.ToUpper() == a,
					Neighbors = new List<Node>()
				})
				.ToList();

			foreach( var edge in edges )
			{
				var p1 = nodes.First(a => a.Name == edge[0]);
				var p2 = nodes.First(a => a.Name == edge[1]);
				p1.Neighbors.Add(p2);
				p2.Neighbors.Add(p1);
			}

			var start = nodes.First(a => a.Name == "start");
			var end = nodes.First(a => a.Name == "end");

			var paths = new Queue<Path>();
			paths.Enqueue(new Path { Nodes = new List<Node> { start } });
			var numPaths = 0;
			while( paths.Count > 0 )
			{
				var curr = paths.Dequeue();
				var nexts = curr.Nodes.Last().Neighbors.Where(a => (!curr.Nodes.Contains(a) || a.IsLarge || !curr.SmallTwice) && a != start).ToList();
				if( nexts.Count > 0 )
				{
					foreach( var next in nexts )
					{
						if( next == end )
						{
							numPaths++;
						}
						else
						{
							var nextPath = new Path
							{
								Nodes = curr.Nodes.ToList(),
								SmallTwice = curr.SmallTwice
							};
							nextPath.Nodes.Add(next);
							if( !next.IsLarge && curr.Nodes.Contains(next) )
								nextPath.SmallTwice = true;
							paths.Enqueue(nextPath);
						}
					}
				}
			}
			Console.WriteLine(numPaths);
		}

		class Path
		{
			public List<Node> Nodes { get; set; }
			public bool SmallTwice { get; set; }

			public override string ToString() => (this.SmallTwice ? "2! " : "") + string.Join(",", this.Nodes.Select(n => n.ToString()));
		}

		class Node
		{
			public string Name { get; set; }
			public bool IsLarge { get; set; }
			public List<Node> Neighbors { get; set; }

			public override string ToString() => this.Name;

		}
	}
}
