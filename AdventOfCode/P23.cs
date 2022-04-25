using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class P23 : Problem
	{
		public void SolveA()
		{
			//var initial = new State("BCBD ADCA");
			var initial = new State("DBDB CAAC");
			
			//var initial = new State("ABCD ABCD");
			Console.WriteLine(initial);

			int? maxCost = null;
			int idx = 0;
			this.Iterate(initial, ref maxCost, ref idx);
			Console.WriteLine(idx);
			Console.WriteLine(maxCost);

			//var states = new List<State> { initial };
			//var idx = 0;
			//while( true )
			//{
			//	var state = states[0];
			//	if( idx++ % 1000 == 0 )
			//	{
			//		Console.WriteLine(states.Count);
			//		Console.WriteLine(state.Cost);
			//		Console.WriteLine(state);
			//		//Console.ReadLine();
			//	}

			//	if( state.IsDone() )
			//	{
			//		Console.WriteLine(state.Cost);
			//		break;
			//	}
			//	states.RemoveAt(0);
			//	states.AddRange(state.GetStates());
			//	states = states.OrderBy(s => s.Cost).ToList();
			//}
		}

		static List<string> moves = new List<string>
		{
			"R2T 2,3",
			"R2R 1,2",
			"R2T 1,5",
			"T2R 3,1"
		};
		static int midx = 0;
		private void Iterate(State state, ref int? maxCost, ref int idx)
		{
			//if( idx++ % 1000 == 0 )
			//{
			//	Console.WriteLine(idx);
			//	Console.WriteLine(state);
			//	Console.WriteLine(maxCost);
			//}

			if( maxCost != null && state.Cost >= maxCost )
				return;

			if( state.IsDone() )
			{
				maxCost = state.Cost;
				Console.WriteLine(state);
				Console.WriteLine(state.Cost);
				return;
			}

			var nextStates = state.GetStates().ToList();
			foreach( var s in nextStates )
			{
				if( moves.Count > midx && s.NumSteps == midx + 1 && s.LastMove == moves[midx] )
				{
					Console.WriteLine(moves[midx++]);
				}
				this.Iterate(s, ref maxCost, ref idx);
			}
		}

		private class State
		{
			public char[] Top { get; set; } = new char[11];
			public char[,] Rooms { get; set; } = new char[4, 2];

			public int Cost { get; set; } = 0;
			public int NumSteps { get; set; } = 0;
			public string LastMove { get; set; }

			public override string ToString()
			{
				var sb = new StringBuilder();
				sb.Append(this.Cost);
				sb.Append(" ");
				sb.Append(this.NumSteps);
				sb.AppendLine(" ");

				sb.Append("#");
				foreach( var t in this.Top )
				{
					sb.Append(t);
				}
				sb.AppendLine("#");

				sb.Append("###");
				for( int i = 0; i < 4; i++ )
				{
					sb.Append(this.Rooms[i, 0]);
					sb.Append("#");
				}
				sb.AppendLine("##");

				sb.Append("###");
				for( int i = 0; i < 4; i++ )
				{
					sb.Append(this.Rooms[i, 1]);
					sb.Append("#");
				}
				sb.AppendLine("##");
				return sb.ToString();
			}

			public State(string state = null)
			{
				for( int i = 0; i < this.Top.Length; i++ )
				{
					this.Top[i] = '.';
				}

				var stateIdx = 0;
				state = state?.Replace(" ", "");
				for( int j = 0; j < this.Rooms.GetLength(1); j++ )
				{
					for( int i = 0; i < this.Rooms.GetLength(0); i++ )
					{
						this.Rooms[i, j] = state?.ElementAt(stateIdx++) ?? '.';
					}
				}
			}

			public State Copy()
			{
				var other = new State();
				for( int i = 0; i < this.Top.Length; i++ )
				{
					other.Top[i] = this.Top[i];
				}
				for( int i = 0; i < 4; i++ )
				{
					for( int j = 0; j < 2; j++ )
					{
						other.Rooms[i, j] = this.Rooms[i, j];
					}
				}
				other.Cost = this.Cost;
				other.NumSteps = this.NumSteps;
				return other;
			}

			public IEnumerable<State> GetStates()
			{
				bool WantsToMoveFromRoom(int i)
				{
					var owner = 'A' + i;
					var top = this.Rooms[i, 0];
					var bottom = this.Rooms[i, 1];
					// ok: . A
					//     A A
					if( bottom == owner && (top == owner || top == '.') )
						return false;
					return true;
				}

				// moves from top to room
				for( int i = 0; i < this.Top.Length; i++ )
				{
					var current = this.Top[i];
					if( current != '.' )
					{
						var roomIdx = current - 'A';
						var doorIdx = roomIdx * 2 + 2;

						if( !this.CanMoveTop(i, doorIdx) )
							continue;

						var room1 = this.Rooms[roomIdx, 0];
						var room2 = this.Rooms[roomIdx, 1];
						var stepsToDoor = Math.Abs(i - doorIdx);
						var roomPos = -1;
						if( room1 == '.' )
						{
							if( room2 == '.' ) roomPos = 1;
							else if( room2 == current ) roomPos = 0;
						}
						if( roomPos >= 0 )
						{
							var next = this.Copy();
							next.Top[i] = '.';
							next.Rooms[roomIdx, roomPos] = current;
							next.Cost += (stepsToDoor + roomPos + 1) * Weight(current);
							next.NumSteps = this.NumSteps + 1;
							next.LastMove = $"T2R {i},{roomIdx}";
							yield return next;
							yield break;
						}
					}
				}

				// moves from room to top
				for( int i = 0; i < this.Rooms.GetLength(0); i++ )
				{
					//if( this.NumSteps == 0 )
					//{
					//	var a = 0;
					//}
					if( !WantsToMoveFromRoom(i) )
						continue;

					var top = this.Rooms[i, 0];
					var bottom = this.Rooms[i, 1];
					int j = -1;
					if( top != '.' ) j = 0;
					else if( bottom != '.' ) j = 1;

					if( j >= 0 )
					{
						for( int k = 0; k < this.Top.Length; k++ )
						{
							//if(k == 3)
							//{
							//	Console.WriteLine("hello");
							//}
							if( k > 1 && k < 9 && k % 2 == 0 )
								continue;

							var doorIdx = i * 2 + 2;
							if( this.Top[k] == '.' && this.CanMoveTop(doorIdx, k) )
							{
								var next = this.Copy();
								next.Top[k] = this.Rooms[i, j];
								next.Rooms[i, j] = '.';
								var stepsToDoor = Math.Abs(k - doorIdx);
								next.Cost += (stepsToDoor + j + 1) * Weight(this.Rooms[i, j]);
								next.NumSteps = this.NumSteps + 1;
								next.LastMove = $"R2T {i},{k}";
								yield return next;
							}
						}
					}
				}

				// moves from room to room
				for( int i = 0; i < this.Rooms.GetLength(0); i++ )
				{
					if( !WantsToMoveFromRoom(i) )
						continue;

					var top = this.Rooms[i, 0];
					var bottom = this.Rooms[i, 1];
					int j = -1;
					if( top != '.' ) j = 0;
					else if( bottom != '.' ) j = 1;
					var fromDoorIdx = i * 2 + 2;

					if( j >= 0 )
					{
						var current = this.Rooms[i, j];
						var toRoomIdx = current - 'A';
						var toDoorIdx = toRoomIdx * 2 + 2;

						if( !this.CanMoveTop(fromDoorIdx, toDoorIdx) )
							continue;

						var room1 = this.Rooms[toRoomIdx, 0];
						var room2 = this.Rooms[toRoomIdx, 1];
						var stepsToDoor = Math.Abs(fromDoorIdx - toDoorIdx);
						var roomPos = -1;
						if( room1 == '.' )
						{
							if( room2 == '.' ) roomPos = 1;
							else if( room2 == current ) roomPos = 0;
						}
						if( roomPos >= 0 )
						{
							var next = this.Copy();
							next.Rooms[i, j] = '.';
							next.Rooms[toRoomIdx, roomPos] = current;
							next.Cost += (stepsToDoor + (roomPos + 1) + (j + 1)) * Weight(current);
							next.NumSteps = this.NumSteps + 1;
							next.LastMove = $"R2R {i},{toRoomIdx}";
							yield return next;
							yield break;
						}
					}
				}
			}

			public bool IsDone()
			{
				for( int i = 0; i < 4; i++ )
				{
					for( int j = 0; j < 2; j++ )
					{
						if( this.Rooms[i, j] != 'A' + i )
							return false;
					}
				}
				return true;
			}

			private bool CanMoveTop(int from, int to)
			{
				var d = Math.Sign(to - from);
				for( int j = from + d; j != to; j += d )
				{
					if( this.Top[j] != '.' )
						return false;
				}
				return true;
			}

			private static int Weight(char c) => c switch
			{
				'A' => 1,
				'B' => 10,
				'C' => 100,
				'D' => 1000,
				_ => 0
			};
		}
	}
}
