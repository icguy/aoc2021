using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class P08 : Problem
	{
		public void SolveA()
		{
			var lines = this.ReadInput("p08.txt");
			var lengths = new List<int> { 2, 4, 3, 7 };
			var result = lines
				.Select(a => a.Split(new[] { " | " }, StringSplitOptions.RemoveEmptyEntries)[1])
				.SelectMany(a => a.Split(new[] { ' ' }))
				.Select(a => a.Trim())
				.Count(a => lengths.Contains(a.Length));
			Console.WriteLine(result);
		}

		public void SolveB()
		{
			var lines = this.ReadInput("p08.txt");
			var result = lines
				.Select(line =>
				{
					var halves = line.Split(new[] { " | " }, StringSplitOptions.RemoveEmptyEntries);
					var d = new Display
					{
						Samples = halves[0].Split(new[] { ' ' }).Select(a => a.ToHashSet()).ToArray(),
						Number = halves[1].Split(new[] { ' ' }).Select(a => a.ToHashSet()).ToArray(),
					};
					this.SolveDisplayMap(d);
					var number = 0;
					for( int i = 0; i < d.Number.Length; i++ )
					{
						number *= 10;
						number += d.Mapping.Single(m => m.Key.SetEquals(d.Number[i])).Value;
					}
					return number;
				})
				.Sum();
			Console.WriteLine(result);
		}

		void SolveDisplayMap(Display display)
		{
			var ch1 = display.Samples.Single(s => s.Count == 2);
			var ch4 = display.Samples.Single(s => s.Count == 4);
			var ch7 = display.Samples.Single(s => s.Count == 3);
			var ch8 = display.Samples.Single(s => s.Count == 7);
			
			//var a = ch7.Except(ch1).Single();
			var ch3 = display.Samples.Single(s => s.Count == 5 && s.IsProperSupersetOf(ch7));
			var eb = ch8.Except(ch3);
			var b = ch4.Intersect(eb).Single();
			var e = eb.Single(x => x != b);
			var ch5 = display.Samples.Single(s => s.Count == 5 && s.Contains(b));
			var ch2 = display.Samples.Single(s => s.Count == 5 && s.Contains(e));
			var adg = ch2.Intersect(ch5);
			var c = ch2.Except(adg).Single(x => x != e);
			//var f = ch5.Except(adg).Single(x => x != b);
			var ch0 = display.Samples.Single(s => s.Count == 6 && s.Contains(c) && s.Contains(e));
			//var d = adg.Except(ch0).Single();
			//var g = adg.Single(x => x != a && x != d);

			var ch6 = ch8.Where(x => x != c).ToHashSet();
			var ch9 = ch8.Where(x => x != e).ToHashSet();

			display.Mapping = new Dictionary<HashSet<char>, int>
			{
				{ ch0, 0 },
				{ ch1, 1 },
				{ ch2, 2 },
				{ ch3, 3 },
				{ ch4, 4 },
				{ ch5, 5 },
				{ ch6, 6 },
				{ ch7, 7 },
				{ ch8, 8 },
				{ ch9, 9 },
			};
		}

		class Display
		{
			public HashSet<char>[] Samples { get; set; }
			public HashSet<char>[] Number { get; set; }
			public Dictionary<HashSet<char>, int> Mapping { get; set; }
		}
	}
}
