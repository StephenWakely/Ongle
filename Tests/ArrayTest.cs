
using System;
using NUnit.Framework;
using Ongle;
using System.Collections.Generic;
using Ninject;
using System.IO;

namespace Tests
{

	[TestFixture()]
	public class ArrayTest
	{
		static Tokens CreateSimpleArray ()
		{
			Tokens tokens = new Tokens ();
			tokens.Add ( new Token ( "(" ) );
			tokens.Add ( new Token ( "'a'" ) );
			tokens.Add ( new Token ( "," ) );
			tokens.Add ( new Token ( "'b'" ) );
			tokens.Add ( new Token ( "," ) );
			tokens.Add ( new Token ( "3" ) );
			tokens.Add ( new Token ( ")" ) );

			return tokens;
		}

		[Test()]
		public void TestScanSimpleArray ()
		{
			string line = "x=(1,'3',4+3)";
			MemoryStream stream = new MemoryStream ( new System.Text.ASCIIEncoding().GetBytes(line) );
			Scanner scanner = new Scanner (stream);

			Assert.AreEqual ("x", scanner.Tokens.PullToken());
			Assert.AreEqual ("=", scanner.Tokens.PullToken());
			Assert.AreEqual ("(", scanner.Tokens.PullToken());
			Assert.AreEqual ("1", scanner.Tokens.PullToken());
			Assert.AreEqual (",", scanner.Tokens.PullToken());
			Assert.AreEqual ("'3'", scanner.Tokens.PullToken());
			Assert.AreEqual (",", scanner.Tokens.PullToken());
			Assert.AreEqual ("4", scanner.Tokens.PullToken());
			Assert.AreEqual ("+", scanner.Tokens.PullToken());
			Assert.AreEqual ("3", scanner.Tokens.PullToken());
			Assert.AreEqual (")", scanner.Tokens.PullToken());			
		}
		
		[Test()]
		public void TestParseArrayCreatesArray ()
		{
			var tokens = CreateSimpleArray ();
			IKernel kernel = TestModule.GetTestKernel ();
			IExpressionParser parser = kernel.Get<IExpressionParser> (); 
			Expression array = parser.ParseArray(new Scope(), tokens);			
			Assert.IsInstanceOfType ( typeof(ArrayExpr), array, "Should be array type" );			
		}
		
		[Test()]
		public void TestParseArrayContainsElements ()
		{
			var tokens = CreateSimpleArray();
			
			IKernel kernel = TestModule.GetTestKernel ();
			IExpressionParser parser = kernel.Get<IExpressionParser> (); 
			ArrayExpr array = parser.ParseArray(new Scope(), tokens) as ArrayExpr;			

			Assert.AreEqual ( 3, array.Elements.Count, "Array should have 3 elements");
		}
		
		[Test()]
		public void TestEvaluateArrayWithStringLiterals ()
		{
			IKernel kernel = TestModule.GetTestKernel ();
			var factory = kernel.Get<IExecutorFactory>();
			ArrayExpr array = new ArrayExpr(factory.GetArrayExecutor());
			
			array.Elements.Add(new StringLiteral {
				Value = "yay"
			});

			array.Elements.Add(new StringLiteral {
				Value = "boo"
			});
			
			Dynamic dynamic = array.Evaluate();
			Assert.AreEqual(DynamicType.arrayType, dynamic.Type, "Type should be array");
			Assert.AreEqual(dynamic.ArrayValue[0].StringValue, "yay", "First element should be 'yay'");
			Assert.AreEqual(dynamic.ArrayValue[1].StringValue, "boo", "Second element should be 'boo'");			
		}
		
		
		[Test()]
		public void TestEvaluateArrayWithArithLiterals ()
		{
			IKernel kernel = TestModule.GetTestKernel ();
			var factory = kernel.Get<IExecutorFactory>();
			ArrayExpr array = new ArrayExpr(factory.GetArrayExecutor());

			array.Elements.Add(new ArithExpr {
				Left = new NumberLiteral {
					Value = 3 },
				Right = new NumberLiteral {
					Value = 4 },
				Op = ArithOp.Add
			});
			
			Dynamic dynamic = array.Evaluate();
			Assert.AreEqual(dynamic.ArrayValue[0].NumberValue, 7, "First element should be 3+4");
		}
	}
}
