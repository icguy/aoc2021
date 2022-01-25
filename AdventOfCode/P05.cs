using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class P05 : Problem
	{
		public void SolveA()
		{
			var lines = this.ReadInput("p05.txt");
			var vents = lines
				.Select(l =>
				{
					var coords = l
						.Split(new[] { ",", " -> " }, StringSplitOptions.RemoveEmptyEntries)
						.Select(a => int.Parse(a))
						.ToList();
					return new Vent
					{
						X1 = coords[0],
						Y1 = coords[1],
						X2 = coords[2],
						Y2 = coords[3],
						Source = l
					};
				})
				.ToList();
			var map = new int[1000, 1000];
			foreach( var vent in vents )
			{
				if( vent.X1 == vent.X2 || vent.Y1 == vent.Y2 )
				{
					int dx = Math.Sign(vent.X2 - vent.X1);
					int dy = Math.Sign(vent.Y2 - vent.Y1);

					var x = vent.X1;
					var y = vent.Y1;

					map[x, y] += 1;
					do
					{
						x += dx;
						y += dy;
						map[x, y] += 1;
					} while( x != vent.X2 || y != vent.Y2 );
				}
				//Console.WriteLine(vent.Source);
				//Print(map);
			}
			//Print(map);

			var count = 0;
			for( int x = 0; x < map.GetLength(0); x++ )
			{
				for( int y = 0; y < map.GetLength(1); y++ )
				{
					if( map[x, y] > 1 )
						count++;
				}
			}
			Console.WriteLine(count);
		}

		public void SolveB()
		{
			var lines = this.ReadInput("p05.txt");
			var vents = lines
				.Select(l =>
				{
					var coords = l
						.Split(new[] { ",", " -> " }, StringSplitOptions.RemoveEmptyEntries)
						.Select(a => int.Parse(a))
						.ToList();
					return new Vent
					{
						X1 = coords[0],
						Y1 = coords[1],
						X2 = coords[2],
						Y2 = coords[3],
						Source = l
					};
				})
				.ToList();
			var map = new int[1000, 1000];
			foreach( var vent in vents )
			{
				int dx = Math.Sign(vent.X2 - vent.X1);
				int dy = Math.Sign(vent.Y2 - vent.Y1);

				var x = vent.X1;
				var y = vent.Y1;

				map[x, y] += 1;
				do
				{
					x += dx;
					y += dy;
					map[x, y] += 1;
				} while( x != vent.X2 || y != vent.Y2 );
				//Console.WriteLine(vent.Source);
				//Print(map);
			}
			//Print(map);

			var count = 0;
			for( int x = 0; x < map.GetLength(0); x++ )
			{
				for( int y = 0; y < map.GetLength(1); y++ )
				{
					if( map[x, y] > 1 )
						count++;
				}
			}
			Console.WriteLine(count);
		}

		void Print(int[,] map)
		{
			for( int x = 0; x < map.GetLength(0); x++ )
			{
				for( int y = 0; y < map.GetLength(1); y++ )
				{
					Console.Write(map[x, y] == 0 ? "." : map[x, y].ToString());
				}
				Console.WriteLine();
			}
			Console.WriteLine();
		}

		class Vent
		{
			public int X1 { get; set; }
			public int Y1 { get; set; }
			public int X2 { get; set; }
			public int Y2 { get; set; }
			public string Source { get; set; }
		}
	}
}
