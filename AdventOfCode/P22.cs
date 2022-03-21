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
			var input = this.ReadInputEx();
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

			var numOn = new BigInteger(0);
			var size = new BigInteger(1);
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

		IEnumerable<Step> Subtract(Step step, Step region)
		{
			var xOk = Between(step.XMin, region.XMin, region.XMax) || Between(step.XMax, region.XMin, region.XMax);
			var yOk = Between(step.YMin, region.YMin, region.YMax) || Between(step.YMax, region.YMin, region.YMax);
			var zOk = Between(step.ZMin, region.ZMin, region.ZMax) || Between(step.ZMax, region.ZMin, region.ZMax);

			if( xOk && yOk && zOk )
			{
				foreach( var xIntervals in this.Intersect(step.XMin, step.XMax, region.XMin, region.XMax) )
					foreach( var yIntervals in this.Intersect(step.YMin, step.YMax, region.YMin, region.YMax) )
						foreach( var zIntervals in this.Intersect(step.ZMin, step.ZMax, region.ZMin, region.ZMax) )
						{
							var newRegion = new Step
							{
								XMin = xIntervals.min,
								XMax = xIntervals.max,
								YMin = yIntervals.min,
								YMax = yIntervals.max,
								ZMin = zIntervals.min,
								ZMax = zIntervals.max,
								TurnOn = region.TurnOn
							};

							if( !this.Contains(step, newRegion) )
								yield return newRegion;
						}
			}
		}

		IEnumerable<(int min, int max)> Intersect(int stepMin, int stepMax, int regionMin, int regionMax)
		{
			// R   |------|
			// S |----|
			if(stepMin <= regionMin && stepMax <= regionMax)
			{
				yield return (regionMin, stepMax);
				yield return (stepMax + 1, regionMax);
			}

			// R |------|
			// S   |--|
			if(regionMin <= stepMin && stepMax <= regionMax)
			{
				yield return (regionMin, stepMin - 1);
				yield return (stepMin, stepMax);
				yield return (stepMax + 1, regionMax);
			}

			// R |------|
			// S     |----|
			if( regionMin <= stepMin && regionMax <= stepMax )
			{
				yield return (regionMin, stepMin - 1);
				yield return (stepMin, regionMax);
			}
		}


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
