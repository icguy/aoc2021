using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class P16 : Problem
	{
		public void SolveA()
		{
			var input = "D2FE28";
			input = "8A004A801A8002F478";
			input = "620080001611562C8802118E34";
			input = "C0015000016115A2E0802F182340";
			input = "A0016C880162017C3686B18A3D4780";

			input = "C200B40A82";
			input = "04005AC33890";
			input = "880086C3E88112";
			input = "CE00C43D881120";
			input = "D8005AC2A8F0";
			input = "F600BC2D8F";
			input = "9C005AC2F8F0";
			input = "9C0141080250320F1802104A08";

			input = this.ReadInput().First();

			var binBuilder = new StringBuilder();
			foreach( var c in input.ToLower() )
			{
				switch( c )
				{
					case '0': binBuilder.Append("0000"); break;
					case '1': binBuilder.Append("0001"); break;
					case '2': binBuilder.Append("0010"); break;
					case '3': binBuilder.Append("0011"); break;
					case '4': binBuilder.Append("0100"); break;
					case '5': binBuilder.Append("0101"); break;
					case '6': binBuilder.Append("0110"); break;
					case '7': binBuilder.Append("0111"); break;
					case '8': binBuilder.Append("1000"); break;
					case '9': binBuilder.Append("1001"); break;
					case 'a': binBuilder.Append("1010"); break;
					case 'b': binBuilder.Append("1011"); break;
					case 'c': binBuilder.Append("1100"); break;
					case 'd': binBuilder.Append("1101"); break;
					case 'e': binBuilder.Append("1110"); break;
					case 'f': binBuilder.Append("1111"); break;
				}
			}

			var bin = binBuilder.ToString();
			//bin = "00111000000000000110111101000101001010010001001000000000";
			//bin = "11101110000000001101010000001100100000100011000001100000";
			var packets = this.Parse(new BitReader(bin)).ToList();
			//foreach( var p in packets )
			//{
			//	p.Print();
			//}
			Console.WriteLine(this.CalculateVersionSum(packets));
			Console.WriteLine(this.CalculateExpression(packets[0]));
		}

		#region parsing
		class BitReader
		{
			private string _bin;
			private int _next = 0;

			public BitReader(string bin)
			{
				_bin = bin;
			}

			public string Read(int numBits)
			{
				if( _next + numBits > _bin.Length )
					throw new EndOfMessageException();

				var result = _bin.Substring(_next, numBits);
				_next += numBits;
				return result;
			}

			public long ReadLong(int numBits) => P16.ToLong(this.Read(numBits));
			public int ReadInt(int numBits) => (int)P16.ToLong(this.Read(numBits));
		}

		class EndOfMessageException : Exception { }

		IEnumerable<Packet> Parse(BitReader reader, int? maxPackets = null)
		{
			var numParsed = 0;
			while( true )
			{
				if( numParsed >= maxPackets )
					break;

				Packet p = null;
				try
				{
					var version = reader.ReadInt(3);
					var type = reader.ReadInt(3);
					switch( type )
					{
						case LiteralPacket.TypeCode:
							p = this.ParseLiteral(version, type, reader);
							break;
						default:
							p = this.ParseOperator(version, type, reader);
							break;
					}
				}
				catch( EndOfMessageException ex )
				{
				}

				if( p != null )
					yield return p;
				else
					yield break;
				numParsed++;
			}
		}

		private LiteralPacket ParseLiteral(int version, int type, BitReader reader)
		{
			var sb = new StringBuilder();
			while( true )
			{
				var word = reader.Read(5);
				sb.Append(word.Substring(1));
				if( word[0] == '0' )
					break;
			}
			var value = ToLong(sb.ToString());
			return new LiteralPacket
			{
				Version = version,
				PacketType = type,
				Value = value
			};
		}

		private OperatorPacket ParseOperator(int version, int type, BitReader reader)
		{
			return reader.Read(1) == OperatorPacket.BitCountLength.Flag
				? this.ParseOperatorBitCount(version, type, reader)
				: this.ParseOperatorPacketCount(version, type, reader);
		}

		private OperatorPacket ParseOperatorBitCount(int version, int type, BitReader reader)
		{
			var totalLength = reader.ReadInt(15);
			if( totalLength == 0 )
				throw new EndOfMessageException();

			var subPacketsBin = reader.Read(totalLength);
			var subPackets = this.Parse(new BitReader(subPacketsBin)).ToList();
			return new OperatorPacket
			{
				Version = version,
				PacketType = type,
				Length = new OperatorPacket.BitCountLength
				{
					NumBits = totalLength
				},
				SubPackets = subPackets
			};
		}

		private OperatorPacket ParseOperatorPacketCount(int version, int type, BitReader reader)
		{
			var packetCount = reader.ReadInt(11);
			var subPackets = this.Parse(reader, packetCount).ToList();
			return new OperatorPacket
			{
				Version = version,
				PacketType = type,
				Length = new OperatorPacket.PacketCountLength
				{
					SubPacketCount = packetCount
				},
				SubPackets = subPackets
			};
		}

		public static long ToLong(string bin)
		{
			var multiplier = 1;
			var result = 0L;
			for( int i = 0; i < bin.Length; i++ )
			{
				var bit = bin[bin.Length - 1 - i];
				if( bit == '1' )
					result += multiplier;
				multiplier *= 2;
			}
			return result;
		}

		abstract class Packet
		{
			public int Version { get; set; }
			public int PacketType { get; set; }

			public abstract void Print(int indent = 0);
			protected void PrintIndent(int indent)
			{
				for( int i = 0; i < indent; i++ )
				{
					Console.Write("   ");
				}
			}
		}

		class LiteralPacket : Packet
		{
			public const int TypeCode = 4;

			public long Value { get; set; }

			public override void Print(int indent = 0)
			{
				this.PrintIndent(indent);
				Console.WriteLine($"Li v{this.Version} = {this.Value}");
			}
		}

		class OperatorPacket : Packet
		{
			public PacketLength Length { get; set; }
			public List<Packet> SubPackets { get; set; }

			public override void Print(int indent = 0)
			{
				this.PrintIndent(indent);
				Console.WriteLine($"Op v{this.Version}");
				foreach( var subPacket in this.SubPackets )
				{
					subPacket.Print(indent + 1);
				}
			}

			public abstract class PacketLength
			{
			}

			public class BitCountLength : PacketLength
			{
				public const string Flag = "0";
				public int NumBits { get; set; }
			}

			public class PacketCountLength : PacketLength
			{
				public const string Flag = "1";
				public int SubPacketCount { get; set; }
			}
		}
		#endregion

		#region evaluation

		private int CalculateVersionSum(List<Packet> packets)
		{
			var sum = 0;
			foreach( var packet in packets )
			{
				var curr = packet switch
				{
					LiteralPacket lp => lp.Version,
					OperatorPacket op => this.CalculateVersionSum(op.SubPackets) + op.Version,
					_ => 0
				};
				sum += curr;
			}
			return sum;
		}

		private long CalculateExpression(Packet packet)
		{
			if( packet is LiteralPacket lp ) return lp.Value;
			if( packet is OperatorPacket op )
			{
				switch( packet.PacketType )
				{
					case 0: /* sum */ return op.SubPackets.Sum(sp => this.CalculateExpression(sp));
					case 1: /* prod */ return op.SubPackets.Aggregate(1L, (acc, sp) => acc * this.CalculateExpression(sp));
					case 2: /* min */ return op.SubPackets.Min(sp => this.CalculateExpression(sp));
					case 3: /* min */ return op.SubPackets.Max(sp => this.CalculateExpression(sp));
					case 5: /* gt */ return this.CalculateExpression(op.SubPackets[0]) > this.CalculateExpression(op.SubPackets[1]) ? 1 : 0;
					case 6: /* lt */ return this.CalculateExpression(op.SubPackets[0]) < this.CalculateExpression(op.SubPackets[1]) ? 1 : 0;
					case 7: /* eq */ return this.CalculateExpression(op.SubPackets[0]) == this.CalculateExpression(op.SubPackets[1]) ? 1 : 0;
				}
			}
			return 0;
		}

		#endregion
	}
}
