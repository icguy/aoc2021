using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class P21 : Problem
	{
		public void SolveA()
		{
			var player1 = new Player(4);
			var player2 = new Player(7);
			var currIs1 = true;
			var die = new Die();

			while( player1.Score < 1000 && player2.Score < 1000 )
			{
				var curr = currIs1 ? player1 : player2;
				var steps = die.Roll() + die.Roll() + die.Roll();
				curr.Position += steps;
				curr.Position = (curr.Position + 9) % 10 + 1;
				curr.Score += curr.Position;
				currIs1 = !currIs1;
			}

			var losingScore = player1.Score < 1000 ? player1.Score : player2.Score;
			Console.WriteLine(losingScore * die.NumRolls);
		}

		public void SolveB()
		{
			var player1 = new Player(4);
			var player2 = new Player(7);
			var w1 = new BigInteger(0);
			var w2 = new BigInteger(0);
			var state = new GameState
			{
				IsPlayer1Current = true,
				P1 = player1,
				P2 = player2,
			};
			this.Iterate(state, ref w1, ref w2);
		}

		private void Iterate(GameState state, ref BigInteger w1, ref BigInteger w2)
		{
			var p = state.IsPlayer1Current ? state.P1 : state.P2;
			if( p.Score >= 1000 )
			{
				if( state.IsPlayer1Current )
					w1++;
				else
					w2++;
				return;
			}


			for( int roll1 = 1; roll1 <= 3; roll1++ )
				for( int roll2 = 1; roll2 <= 3; roll2++ )
					for( int roll3 = 1; roll3 <= 3; roll3++ )
					{
						p.Position += roll1 + roll2 + roll3;
						p.Position = (p.Position + 9) % 10 + 1;
						p.Score += p.Position;
						p.NumTurns++;
						if( state.IsPlayer1Current )
							state.P1 = p;
						else
							state.P2 = p;

						state.IsPlayer1Current = !state.IsPlayer1Current;
						this.Iterate(state, ref w1, ref w2);
					}
		}

		struct GameState
		{
			public Player P1 { get; set; }
			public Player P2 { get; set; }
			public bool IsPlayer1Current { get; set; }

			public override string ToString() => $"{(IsPlayer1Current ? "!" : "")}p1=({P1}) {(IsPlayer1Current ? "" : "!")}p2=({P2})";
		}

		struct Player
		{
			public Player(int position)
			{
				this.Position = position;
				this.Score = 0;
				this.NumTurns = 0;
			}

			public int Score { get; set; }
			public int Position { get; set; }
			public int NumTurns { get; set; }

			public override string ToString() => $"p{Position} s{Score}";
		}

		class Die
		{
			private int _nextRoll = 0;
			public int NumRolls { get; set; } = 0;

			public int Roll()
			{
				this.NumRolls++;
				var next = _nextRoll++;
				_nextRoll %= 100;
				return next + 1;
			}
		}
	}
}
