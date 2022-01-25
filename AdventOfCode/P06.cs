using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class P06 : Problem
	{
		public void SolveA()
		{
			var lines = this.ReadInput("p06.txt");
			var numDays = 256;

			var pop = new long[9];
			for( int i = 0; i < 9; i++ )
			{
				pop[i] = lines[0].Count(c => c == i.ToString()[0]);
			}

			for( int i = 0; i < numDays; i++ )
			{
				var num0 = pop[0];
				for( int j = 0; j < 8; j++ )
				{
					pop[j] = pop[j + 1];
				}
				pop[6] += num0;
				pop[8] = num0;
			}
			Console.WriteLine(pop.Sum());
		}
	}
}
