using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class P18 : Problem
	{
		public void SolveA()
		{
			var lines = this.ReadInput();
			//			var input = "[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]];[7,[[[3,7],[4,3]],[[6,3],[8,8]]]];[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]];[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]];[7,[5,[[3,8],[1,4]]]];[[2,[2,2]],[8,[8,1]]];[2,9];[1,[[[9,3],9],[[9,0],[0,7]]]];[[[5,[7,4]],7],1];[[[[4,2],2],6],[8,7]]";
			var numbers = lines.Select(l => this.Parse(l)).ToList();
			Number sum = numbers[0];
			foreach( var number in numbers.Skip(1) )
			{
				sum = this.Add(sum, number);
			}
			Console.WriteLine(sum);
			Console.WriteLine(sum.Magnitude);
		}

		public void SolveB()
		{
			var lines = this.ReadInput();
			//lines = "[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]];[7,[[[3,7],[4,3]],[[6,3],[8,8]]]];[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]];[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]];[7,[5,[[3,8],[1,4]]]];[[2,[2,2]],[8,[8,1]]];[2,9];[1,[[[9,3],9],[[9,0],[0,7]]]];[[[5,[7,4]],7],1];[[[[4,2],2],6],[8,7]]".Split(new[] { ':' });
			var numbers = lines.Select(l => this.Parse(l)).ToList();
			var pairs = new List<(Number, Number, long)>();
			foreach( var num in numbers )
			{
				foreach( var num2 in numbers )
				{
					if( num != num2 )
					{
						var c1 = this.Parse(num.ToString());
						var c2 = this.Parse(num2.ToString());
						var mag = this.Add(c1, c2).Magnitude;
						pairs.Add((num, num2, mag));
					}
				}
			}
			var max = pairs.OrderByDescending(p => p.Item3).ToList();
			Console.WriteLine(max[0].Item1);
			Console.WriteLine(max[0].Item2);
			Console.WriteLine(max[0].Item3);
		}

		private Number Add(Number a, Number b)
		{
			var root = new Pair();
			root.Left = a;
			a.Parent = root;
			root.Right = b;
			b.Parent = root;
			this.Reduce(root);
			return root;
		}

		private void Reduce(Number root)
		{
			while( true )
			{
				if( this.Explode(root) )
					continue;
				if( this.Split(root) )
					continue;
				break;
			}
		}

		private bool Split(Number root)
		{
			var isSplit = false;
			this.Iterate(root, i =>
			{
				if( i is Literal lit && lit.Value >= 10 )
				{
					var newLeft = new Literal();
					var newRight = new Literal();
					var newNum = new Pair();
					newLeft.Parent = newNum;
					newLeft.Value = lit.Value / 2;
					newRight.Parent = newNum;
					newRight.Value = lit.Value - lit.Value / 2;
					newNum.Left = newLeft;
					newNum.Right = newRight;
					newNum.Parent = lit.Parent;

					if( lit.Parent.Left == lit )
						lit.Parent.Left = newNum;
					if( lit.Parent.Right == lit )
						lit.Parent.Right = newNum;
					isSplit = true;
					return true;
				}
				return false;
			});
			return isSplit;
		}

		private bool Explode(Number root)
		{
			Pair exploding = null;
			this.Iterate(root, curr =>
			{
				if( curr is Pair p && p.NestingLevel == 4 )
				{
					exploding = p;
					return true;
				}
				return false;
			});


			if( exploding != null )
			{
				if( exploding.Left is not Literal || exploding.Right is not Literal )
					throw new NotImplementedException();

				Literal left = null;
				bool currPassed = false;
				Literal right = null;
				this.Iterate(root, i =>
				{
					Literal lit = i as Literal;
					if( !currPassed && lit != null )
					{
						left = lit;
						return false;
					}

					if( i == exploding )
					{
						currPassed = true;
						return false;
					}

					if( currPassed && lit != null && lit.Parent != exploding )
					{
						right = lit;
						return true;
					}
					return false;
				});

				if( left != null )
					left.Value += (exploding.Left as Literal).Value;
				if( right != null )
					right.Value += (exploding.Right as Literal).Value;

				var newNum = new Literal
				{
					Parent = exploding.Parent,
					Value = 0
				};
				if( exploding.Parent.Left == exploding )
					exploding.Parent.Left = newNum;
				if( exploding.Parent.Right == exploding )
					exploding.Parent.Right = newNum;
				return true;
			}
			return false;
		}

		private bool Iterate(Number n, Func<Number, bool> action)
		{
			if( action(n) )
			{
				return true;
			}
			if( n is Pair p )
			{
				if( this.Iterate(p.Left, action) ) return true;
				if( this.Iterate(p.Right, action) ) return true;
			}
			return false;
		}

		private Number Parse(string input, Pair parent = null)
		{
			if( int.TryParse(input, out var literal) )
			{
				return new Literal
				{
					Value = literal,
					Parent = parent
				};
			}
			else
			{
				var level = 0;
				for( int i = 0; i < input.Length; i++ )
				{
					switch( input[i] )
					{
						case '[': level++; break;
						case ']': level--; break;
						case ',':
							if( level == 1 )
							{
								var newPair = new Pair();
								newPair.Parent = parent;
								newPair.Left = this.Parse(input.Substring(1, i - 1), newPair);
								newPair.Right = this.Parse(input.Substring(i + 1, input.Length - 2 - i), newPair);
								return newPair;
							}
							break;
					}
				}
			}
			throw new NotImplementedException();
		}

		abstract class Number
		{
			public Pair Parent { get; set; }
			public abstract long Magnitude { get; }
		}

		class Pair : Number
		{
			public Number Left { get; set; }
			public Number Right { get; set; }
			public int NestingLevel => (this.Parent?.NestingLevel ?? -1) + 1;
			public override long Magnitude => this.Left.Magnitude * 3 + this.Right.Magnitude * 2;

			public override string ToString()
			{
				return $"[{this.Left},{this.Right}]";
			}
		}

		class Literal : Number
		{
			public int Value { get; set; }
			public override long Magnitude => this.Value;

			public override string ToString()
			{
				return this.Value.ToString();
			}
		}
	}
}
