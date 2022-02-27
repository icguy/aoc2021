using System;
using System.Collections.Generic;
using System.Linq;
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
			var nums1 = this.GetNums(player1).ToList();
			var nums2 = this.GetNums(player2).ToList();
		}

		private IEnumerable<int> GetNums(Player p)
		{
			if( p.Score >= 21 )
			{
				yield return p.NumTurns;
				yield break;
			}

			for( int i = 1; i <= 3; i++ )
			{
				var curr = p;
				curr.Position += i;
				curr.Position = (curr.Position + 9) % 10 + 1;
				curr.Score += curr.Position;
				curr.NumTurns++;
				foreach( var num in this.GetNums(curr) )
				{
					yield return num;
				}
			}
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
