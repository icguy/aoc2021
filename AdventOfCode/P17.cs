using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class P17 : Problem
	{
		// live
		private const int _txMin = 241;
		private const int _txMax = 273;
		private const int _tyMin = -97;
		private const int _tyMax = -63;

		// example
		//private const int _txMin = 20;
		//private const int _txMax = 30;
		//private const int _tyMin = -10;
		//private const int _tyMax = -5;

		public void SolveA()
		{
			var probe = new State
			{
				Vx = 0,
				Vy = 96
			};

			{
				var x = 0;
				var vx = 0;
				while( x < _txMax )
				{
					x += vx;
					if( _txMin <= x && x <= _txMax )
						probe.Vx = vx;
					vx++;
				}
			}

			while( true )
			{
				Console.WriteLine(probe);
				this.Step(probe);
				Console.ReadLine();
			}
		}

		public void SolveB()
		{
			var vxs = new List<int>();
			for( int vx = 0; vx <= _txMax; vx++ )
			{
				var s = new State
				{
					Vx = vx
				};
				while( true )
				{
					if( s.Vx == 0 )
						break;
					if( s.Px >= _txMin && s.Px <= _txMax )
					{
						vxs.Add(vx);
						break;
					}
					this.Step(s);
				}
			}

			var vys = new List<int>();
			for( int vy = _tyMin - 1; vy < -_tyMin + 1; vy++ )
			{
				var s = new State
				{
					Vy = vy
				};
				while( true )
				{
					if( s.Py < _tyMin )
						break;
					if( s.Py >= _tyMin && s.Py <= _tyMax )
					{
						vys.Add(vy);
						break;
					}
					this.Step(s);
				}
			}

			var corrects = new List<State>();
			foreach( var vx in vxs )
			{
				foreach( var vy in vys )
				{
					var s = new State
					{
						Vx = vx,
						Vy = vy
					};
					if( this.Simulate(s) )
						corrects.Add(s);
				}
			}
			Console.WriteLine(corrects.Count);
		}

		private bool Simulate(State state)
		{
			var s = state.Clone();
			while( true )
			{
				if( IsInTargetArea(s) )
					return true;
				if( IsFailed(s) )
					return false;
				this.Step(s);
			}
		}

		private void Step(State s)
		{
			s.Px += s.Vx;
			s.Py += s.Vy;
			s.Vx -= Math.Sign(s.Vx);
			s.Vy -= 1;
		}

		private static bool IsInTargetArea(State s) => s.Px >= _txMin && s.Px <= _txMax && s.Py >= _tyMin && s.Py <= _tyMax;
		private static bool IsFailed(State s)
		{
			var stoppedBeforeX = s.Px < _txMin && s.Vx == 0;
			var isAfterX = s.Px > _txMax;
			var isUnderY = s.Py < _tyMin;
			return stoppedBeforeX || isAfterX || isUnderY;
		}

		private class State
		{
			public int Px { get; set; } = 0;
			public int Py { get; set; } = 0;
			public int Vx { get; set; }
			public int Vy { get; set; }

			public State Clone() => new State
			{
				Px = this.Px,
				Py = this.Py,
				Vx = this.Vx,
				Vy = this.Vy,
			};

			public override string ToString()
			{
				return $"P=({this.Px}, {this.Py}) V({this.Vx}, {this.Vy}) T={(IsInTargetArea(this) ? 1 : 0)} F={(IsFailed(this) ? 1 : 0)}";
			}
		}
	}
}
