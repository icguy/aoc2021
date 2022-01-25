using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class P04 : Problem
	{
		public void SolveA()
		{
			var lines = this.ReadInput("p04.txt");
			var drawn = lines[0]
				.Split(new[] { ',' })
				.Where(a => !string.IsNullOrWhiteSpace(a))
				.Select(a => int.Parse(a))
				.ToList();
			var boards = new List<Board>();

			for( int i = 2; i + 4 < lines.Length; i += 6 )
			{
				var newBoard = new int[5, 5];
				for( int r = 0; r < 5; r++ )
				{
					var row = lines[i + r];
					var numbers = row
						.Split(new[] { ' ' })
						.Where(a => !string.IsNullOrWhiteSpace(a))
						.Select(a => int.Parse(a.Trim()))
						.ToList();
					for( int c = 0; c < 5; c++ )
					{
						newBoard[r, c] = numbers[c];
					}
				}
				boards.Add(new Board { Numbers = newBoard });
			}

			foreach( var num in drawn )
			{
				foreach( var board in boards )
				{
					board.CrossNumber(num);
					if( this.CheckBoard(board) )
					{
						var sum = 0;
						for( int r = 0; r < 5; r++ )
						{
							for( int c = 0; c < 5; c++ )
							{
								if( !board.Crossed[r, c] )
									sum += board.Numbers[r, c];
							}
						}
						var result = sum * num;
						Console.WriteLine(result);
						return;
					}
				}
			}
		}

		public void SolveB()
		{
			var lines = this.ReadInput("p04.txt");
			var drawn = lines[0]
				.Split(new[] { ',' })
				.Where(a => !string.IsNullOrWhiteSpace(a))
				.Select(a => int.Parse(a))
				.ToList();
			var boards = new List<Board>();

			for( int i = 2; i + 4 < lines.Length; i += 6 )
			{
				var newBoard = new int[5, 5];
				for( int r = 0; r < 5; r++ )
				{
					var row = lines[i + r];
					var numbers = row
						.Split(new[] { ' ' })
						.Where(a => !string.IsNullOrWhiteSpace(a))
						.Select(a => int.Parse(a.Trim()))
						.ToList();
					for( int c = 0; c < 5; c++ )
					{
						newBoard[r, c] = numbers[c];
					}
				}
				boards.Add(new Board { Numbers = newBoard });
			}

			foreach( var num in drawn )
			{
				foreach( var board in boards.ToList() )
				{
					board.CrossNumber(num);
					if( this.CheckBoard(board) )
					{
						if( boards.Count > 1 )
						{
							boards.Remove(board);
						}
						else
						{
							var sum = 0;
							for( int r = 0; r < 5; r++ )
							{
								for( int c = 0; c < 5; c++ )
								{
									if( !board.Crossed[r, c] )
										sum += board.Numbers[r, c];
								}
							}
							var result = sum * num;
							Console.WriteLine(result);
							return;
						}
					}
				}
			}
		}

		private bool CheckBoard(Board board)
		{
			for( int r = 0; r < 5; r++ )
			{
				var rowComplete = true;
				for( int c = 0; c < 5; c++ )
				{
					if( !board.Crossed[r, c] )
					{
						rowComplete = false;
						break;
					}
				}
				if( rowComplete )
					return true;
			}

			for( int c = 0; c < 5; c++ )
			{
				var colComplete = true;
				for( int r = 0; r < 5; r++ )
				{
					if( !board.Crossed[r, c] )
					{
						colComplete = false;
						break;
					}
				}
				if( colComplete )
					return true;
			}
			return false;
		}

		class Board
		{
			public int[,] Numbers { get; set; }
			public bool[,] Crossed { get; set; } = new bool[5, 5];

			public void CrossNumber(int number)
			{
				for( int r = 0; r < 5; r++ )
				{
					for( int c = 0; c < 5; c++ )
					{
						if( this.Numbers[r, c] == number )
							this.Crossed[r, c] = true;
					}
				}
			}
		}
	}
}
