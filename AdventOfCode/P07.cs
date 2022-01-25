using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class P07 : Problem
	{
		public void SolveA()
		{
			var lines = this.ReadInput("p07.txt");
			var positions = lines[0]
				.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(a => long.Parse(a))
				.GroupBy(a => a)
				.ToDictionary(a => a.Key, a => a.Count());

			var toLeft = 0;
			var onPosition = positions.TryGetValue(0, out int val) ? val : 0;
			var toRight = positions.Sum(a => a.Value) - onPosition;

			var sum = positions.Sum(a => a.Key * a.Value);
			var x = 0;
			var best = sum;
			var bestX = x;
			while( toRight > 0 )
			{
				x++;
				sum += toLeft + onPosition - toRight;
				if( best > sum )
				{
					best = sum;
					bestX = x;
				}

				toLeft += onPosition;
				onPosition = positions.TryGetValue(x, out val) ? val : 0;
				toRight -= onPosition;
			}
			Console.WriteLine(best);
			Console.WriteLine(bestX);
		}

		public void SolveAv2()
		{
			var lines = this.ReadInput("p07.txt");
			var positions = lines[0]
				.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(a => long.Parse(a))
				.ToList();

			var best = long.MaxValue;
			var bestX = -1;
			for( int x = 0; x < positions.Max(); x++ )
			{
				long sum = 0;
				for( int i = 0; i < positions.Count; i++ )
				{
					var n = Math.Abs(x - positions[i]);
					sum += n * (n + 1) / 2;
				}
				if(sum < best)
				{
					best = sum;
					bestX = x;
				}
			}
			Console.WriteLine(best);
			Console.WriteLine(bestX);
		}
	}
}
