using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	internal class P22 : Problem
	{
		public void SolveA()
		{
			var input = this.ReadInput();
			var steps = input.Select(i =>
			{
				var tokens = i.Split(new string[] { " ", "=", "..", "x", "y", "z", "," }, StringSplitOptions.RemoveEmptyEntries);
				return new Step
				{
					TurnOn = tokens[0] == "on",
					XMin = int.Parse(tokens[1]),
					XMax = int.Parse(tokens[2]),
					YMin = int.Parse(tokens[3]),
					YMax = int.Parse(tokens[4]),
					ZMin = int.Parse(tokens[5]),
					ZMax = int.Parse(tokens[6]),
				};
			})
				.ToList();

			var cubes = new List<Cube>();
			for( int x = -50; x <= 50; x++ )
			{
				for( int y = -50; y <= 50; y++ )
				{
					for( int z = -50; z <= 50; z++ )
					{
						cubes.Add(new Cube { IsOn = false, X = x, Y = y, Z = z });
					}
				}
			}

			foreach( var step in steps )
			{
				foreach( var cube in cubes )
				{
					var xOk = cube.X >= step.XMin && cube.X <= step.XMax;
					var yOk = cube.Y >= step.YMin && cube.Y <= step.YMax;
					var zOk = cube.Z >= step.ZMin && cube.Z <= step.ZMax;
					if( xOk && yOk && zOk )
					{
						cube.IsOn = step.TurnOn;
					}
				}
			}

			Console.WriteLine(cubes.Count(c => c.IsOn));
		}

		public void SolveB()
		{
			var rand = new Random(4);
			var input = this.ReadInput();
			var steps = input.Select(i =>
			{
				var tokens = i.Split(new string[] { " ", "=", "..", "x", "y", "z", "," }, StringSplitOptions.RemoveEmptyEntries);
				var XMin = rand.Next(1, 200);
				var XMax = XMin + rand.Next(1, 200);
				var YMin = rand.Next(1, 200);
				var YMax = YMin + rand.Next(1, 200);
				//var ZMin = rand.Next(1, 200);
				//var ZMax = ZMin + rand.Next(1, 200);
				var ZMin = 0;
				var ZMax = 0;

				return new Step
				{
					TurnOn = tokens[0] == "on",
					XMin = int.Parse(tokens[1]),
					XMax = int.Parse(tokens[2]),
					YMin = int.Parse(tokens[3]),
					YMax = int.Parse(tokens[4]),
					ZMin = int.Parse(tokens[5]),
					ZMax = int.Parse(tokens[6]),
				};

				//return new Step
				//{
				//	TurnOn = rand.Next() % 2 == 0,
				//	XMin = XMin,
				//	XMax = XMax,
				//	YMin = YMin,
				//	YMax = YMax,
				//	ZMin = ZMin,
				//	ZMax = ZMax,
				//};
			})
				//.Take(3)
				.ToList();

			var numOn = new BigInteger(0);
			var size = new BigInteger(1);
			Console.WriteLine("new");
			foreach( var region in this.GenerateRegions(steps) )
			{
				foreach( var step in steps )
				{
					var xOk = region.XMin >= step.XMin && region.XMax <= step.XMax;
					var yOk = region.YMin >= step.YMin && region.YMax <= step.YMax;
					var zOk = region.ZMin >= step.ZMin && region.ZMax <= step.ZMax;
					if( xOk && yOk && zOk )
					{
						region.TurnOn = step.TurnOn;
					}
				}
				//Console.WriteLine(region + " " + region.TurnOn);

				if( region.TurnOn )
				{
					size = 1;
					size *= region.XMax - region.XMin + 1;
					size *= region.YMax - region.YMin + 1;
					size *= region.ZMax - region.ZMin + 1;
					numOn += size;
				}
			}
			Console.WriteLine(numOn);

			var numOn1 = numOn;
			numOn = new BigInteger(0);
			size = new BigInteger(1);
			Console.WriteLine("old");
			foreach( var region in this.GenerateRegionsOld(steps) )
			{
				foreach( var step in steps )
				{
					var xOk = region.XMin >= step.XMin && region.XMax <= step.XMax;
					var yOk = region.YMin >= step.YMin && region.YMax <= step.YMax;
					var zOk = region.ZMin >= step.ZMin && region.ZMax <= step.ZMax;
					if( xOk && yOk && zOk )
					{
						region.TurnOn = step.TurnOn;
					}
				}
				//Console.WriteLine(region + " " + region.TurnOn);

				if( region.TurnOn )
				{
					size = 1;
					size *= region.XMax - region.XMin + 1;
					size *= region.YMax - region.YMin + 1;
					size *= region.ZMax - region.ZMin + 1;
					numOn += size;
				}
			}
			Console.WriteLine(numOn);
			Console.WriteLine(numOn1 == numOn);
		}

		List<Step> GenerateRegions(List<Step> steps)
		{
			var regions = new List<Step>();
			foreach( var step in steps )
			{
				foreach( var region in regions.ToList() )
				{
					if( this.Contains(step, region) )
					{
						regions.Remove(region);
					}
					else
					{
						var newRegions = this.Subtract(step, region).ToList();
						if( newRegions.Count > 0 )
						{
							regions.Remove(region);
							regions.AddRange(newRegions);
						}
					}
				}
				regions.Add(step);
			}
			return regions;
		}

		static bool Between(int val, int min, int max) => val >= min && val <= max;

		// subtract step from region
		IEnumerable<Step> Subtract(Step step, Step region)
		{
			//var xOk = Between(step.XMin, region.XMin, region.XMax) || Between(step.XMax, region.XMin, region.XMax);
			//var yOk = Between(step.YMin, region.YMin, region.YMax) || Between(step.YMax, region.YMin, region.YMax);
			//var zOk = Between(step.ZMin, region.ZMin, region.ZMax) || Between(step.ZMax, region.ZMin, region.ZMax);

			var xOk = !(step.XMin > region.XMax || step.XMax < region.XMin);
			var yOk = !(step.YMin > region.YMax || step.YMax < region.YMin);
			var zOk = !(step.ZMin > region.ZMax || step.ZMax < region.ZMin);

			if( xOk && yOk && zOk )
			{
				var xIntervals = this.Intersect(step.XMin, step.XMax, region.XMin, region.XMax).ToList();
				var yIntervals = this.Intersect(step.YMin, step.YMax, region.YMin, region.YMax).ToList();
				var zIntervals = this.Intersect(step.ZMin, step.ZMax, region.ZMin, region.ZMax).ToList();

				foreach( var xInterval in xIntervals )
					foreach( var yInterval in yIntervals )
						foreach( var zInterval in zIntervals )
						{
							var newRegion = new Step
							{
								XMin = xInterval.min,
								XMax = xInterval.max,
								YMin = yInterval.min,
								YMax = yInterval.max,
								ZMin = zInterval.min,
								ZMax = zInterval.max,
								TurnOn = region.TurnOn
							};

							if( !this.Contains(step, newRegion) && this.Contains(region, newRegion) )
								yield return newRegion;
						}
			}
		}


		IEnumerable<(int min, int max)> Intersect(int stepMin, int stepMax, int regionMin, int regionMax) => this.IntersectInner(stepMin, stepMax, regionMin, regionMax)
			.Where(a => a.min <= a.max)
			.Distinct();

		IEnumerable<(int min, int max)> IntersectInner(int stepMin, int stepMax, int regionMin, int regionMax)
		{
			// R   |------|
			// S |----|
			if( stepMin <= regionMin && stepMax <= regionMax )
			{
				yield return (regionMin, stepMax);
				yield return (stepMax + 1, regionMax);
				yield break;
			}

			// R |------|
			// S   |--|
			if( regionMin <= stepMin && stepMax <= regionMax )
			{
				yield return (regionMin, stepMin - 1);
				yield return (stepMin, stepMax);
				yield return (stepMax + 1, regionMax);
				yield break;
			}

			// R |------|
			// S     |----|
			if( regionMin <= stepMin && regionMax <= stepMax )
			{
				yield return (regionMin, stepMin - 1);
				yield return (stepMin, regionMax);
				yield break;
			}

			// S |------|
			// R   |--|
			if( stepMin <= regionMin && regionMax <= stepMax )
			{
				yield return (stepMin, regionMin - 1);
				yield return (regionMin, regionMax);
				yield return (regionMax + 1, stepMax);
				yield break;
			}
		}

		IEnumerable<Step> GenerateRegionsOld(List<Step> steps)
		{
			var xLimits = new List<int>();
			foreach( var step in steps )
			{
				xLimits.Add(step.XMin);
				xLimits.Add(step.XMax + 1);
			}
			var yLimits = new List<int>();
			foreach( var step in steps )
			{
				yLimits.Add(step.YMin);
				yLimits.Add(step.YMax + 1);
			}
			var zLimits = new List<int>();
			foreach( var step in steps )
			{
				zLimits.Add(step.ZMin);
				zLimits.Add(step.ZMax + 1);
			}
			xLimits.Sort();
			yLimits.Sort();
			zLimits.Sort();

			for( int i = 1; i < xLimits.Count; i++ )
			{
				Console.WriteLine("step: " + i);
				for( int j = 1; j < yLimits.Count; j++ )
				{
					for( int k = 1; k < zLimits.Count; k++ )
					{
						yield return new Step
						{
							TurnOn = false,
							XMin = xLimits[i - 1],
							XMax = xLimits[i] - 1,
							YMin = yLimits[j - 1],
							YMax = yLimits[j] - 1,
							ZMin = zLimits[k - 1],
							ZMax = zLimits[k] - 1,
						};
					}
				}
			}
		}

		// returns true if step contains region
		bool Contains(Step step, Step region)
		{
			var xOk = step.XMin <= region.XMin && region.XMax <= step.XMax;
			var yOk = step.YMin <= region.YMin && region.YMax <= step.YMax;
			var zOk = step.ZMin <= region.ZMin && region.ZMax <= step.ZMax;
			return xOk && yOk && zOk;
		}

		class Step
		{
			public int XMin { get; set; }
			public int XMax { get; set; }
			public int YMin { get; set; }
			public int YMax { get; set; }
			public int ZMin { get; set; }
			public int ZMax { get; set; }
			public bool TurnOn { get; set; }

			public override string ToString() => $"{(TurnOn ? "on " : "off")} x({XMin},{XMax}) y({YMin},{YMax}) z({ZMin},{ZMax})";
		}

		class Cube
		{
			public int X { get; set; }
			public int Y { get; set; }
			public int Z { get; set; }
			public bool IsOn { get; set; }
		}


	}
}
