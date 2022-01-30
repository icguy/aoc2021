using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class Program
	{
		static void Main(string[] args)
		{
			var classNameRegex = new Regex("P\\d\\d");
			var ass = typeof(Program).Assembly;
			var clazz = ass.GetTypes()
				.Where(c => classNameRegex.IsMatch(c.Name))
				.OrderByDescending(c => c.Name)
				.First();
			var method = clazz.GetMethod("SolveB") ?? clazz.GetMethod("SolveA");
			var instance = Activator.CreateInstance(clazz);
			method.Invoke(instance, new object[0]);
		}
	}
}
