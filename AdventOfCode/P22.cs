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
			var regions = new List<Step>();
			var numOn = new BigInteger(0);
			var size = new BigInteger(1);
			for( int i = 1; i < xLimits.Count; i++ )
			{
				Console.WriteLine(i);
				for( int j = 1; j < yLimits.Count; j++ )
				{
					for( int k = 1; k < zLimits.Count; k++ )
					{
						var region = new Step
						{
							TurnOn = false,
							XMin = xLimits[i - 1],
							XMax = xLimits[i] - 1,
							YMin = yLimits[j - 1],
							YMax = yLimits[j] - 1,
							ZMin = zLimits[k - 1],
							ZMax = zLimits[k] - 1,
						};

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

						if(region.TurnOn)
						{
							size = 1;
							size *= region.XMax - region.XMin + 1;
							size *= region.YMax - region.YMin + 1;
							size *= region.ZMax - region.ZMin + 1;
							numOn += size;
						}
					}
				}
			}
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
