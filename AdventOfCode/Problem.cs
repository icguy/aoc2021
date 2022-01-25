using System.IO;

namespace AdventOfCode
{
	class Problem
	{
		public const string Root = "C:/projektek/sajat/adventofcode/problems/";

		protected string[] ReadInput(string file) => File.ReadAllLines(Root + file);
	}
}
