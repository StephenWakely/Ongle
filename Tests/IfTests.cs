using System;
using NUnit.Framework;
using Ongle;
using Ninject;
namespace Tests
{
	[TestFixture()]
	public class IfTests
	{
		static Tokens GetIf ()
		{
			Tokens tokens = new Tokens();
			tokens.AddToken ("if");
			tokens.AddToken ("x");
			tokens.AddToken ("<");
			tokens.AddToken ("10");
			tokens.AddToken ("{");
			tokens.AddToken ("a");
			tokens.AddToken ("}");
			
			return tokens;
		}

		static Block ParseIf ()
		{
			var tokens = GetIf ();
	
			IKernel kernel = TestModule.GetTestKernel ();
			Block block = new Parser ( kernel ).Parse ( tokens );
			return block;
		}

		[Test()]
		public void TestIfParses ()
		{
			Block block = ParseIf();
			
			Assert.AreEqual (1, block.Count);
			Assert.IsInstanceOfType ( typeof(If), block[0], "Should be If type" );			
		}
		
		[Test()]
		public void TestIfTestParses ()
		{
			Block block = ParseIf();

			If expr = block[0] as If;			
			Assert.IsInstanceOfType ( typeof(ArithExpr), expr.Test, "Should be arith type" );			
		}

		[Test()]
		public void TestIfTest ()
		{
			Block block = ParseIf();

			If expr = block[0] as If;			
			ArithExpr test = expr.Test as ArithExpr;
			Assert.IsInstanceOfType (typeof(Variable), test.Left, "Should be Variable"); 
			Assert.AreEqual ( ArithOp.LessThan, test.Op, "Needs to be less than");
			Assert.IsInstanceOfType (typeof(NumberLiteral), test.Right, "Should be a number literal"); 
		}

	}
}

