
using System;
using NUnit.Framework;
using Ongle;
using Ninject;
using System.IO;

namespace Tests
{


	[TestFixture()]
	public class VariableTests
	{

		[Test()]
		public void TestTokenizeArray ()
		{
			string line = "x = y[5]";
			MemoryStream stream = new MemoryStream ( new System.Text.ASCIIEncoding().GetBytes(line) );
			Scanner scanner = new Scanner (stream);

			Assert.AreEqual ("x", scanner.Tokens.PullToken());
			Assert.AreEqual ("=", scanner.Tokens.PullToken());
			Assert.AreEqual ("y", scanner.Tokens.PullToken());
			Assert.AreEqual ("[", scanner.Tokens.PullToken());
			Assert.AreEqual ("5", scanner.Tokens.PullToken());
			Assert.AreEqual ("]", scanner.Tokens.PullToken());
		}
			
		[Test()]
		public void TestParseVariableWithArrayIndex ()
		{
			Tokens tokens = new Tokens();
			tokens.AddToken("x");
			tokens.AddToken("[");
			tokens.AddToken("0");
			tokens.AddToken("]");
			
			Parser parser = new Parser ( TestModule.GetTestKernel() );
			parser.Parse ( tokens );
			
			Assert.AreEqual(1, parser.MainBlock.Count);
			Assert.IsInstanceOfType ( typeof ( Variable ), parser.MainBlock[0] );
		}

		[Test()]
		public void TestParseArrayIndex ()
		{
			Tokens tokens = new Tokens();
			tokens.AddToken("x");
			tokens.AddToken("[");
			tokens.AddToken("0");
			tokens.AddToken("]");
			
			Parser parser = new Parser ( TestModule.GetTestKernel() );
			parser.Parse ( tokens );
			
			Variable variable = parser.MainBlock[0] as Variable;
			Dynamic indexer = variable.Indexer.Evaluate ();
				
			Assert.AreEqual(0, indexer.NumberValue, "Indexer should evaluate to 0"); 			
		}

		[Test()]
		public void TestParseArrayIndexAsExpression ()
		{
			Tokens tokens = new Tokens();
			tokens.AddToken("print");
			tokens.AddToken("x");
			tokens.AddToken("[");
			tokens.AddToken("0");
			tokens.AddToken("]");
			
			Parser parser = new Parser ( TestModule.GetTestKernel() );
			parser.Parse ( tokens );
			
			Print print = parser.MainBlock[0] as Print;
			
			Variable variable = print.Expr as Variable;
			Dynamic indexer = variable.Indexer.Evaluate ();
				
			Assert.AreEqual(0, indexer.NumberValue, "Indexer should evaluate to 0"); 			
		}
		
		[Test()]
		public void TestParseVariableWithArrayIndexExpression ()
		{
			Tokens tokens = new Tokens();
			tokens.AddToken("x");
			tokens.AddToken("[");
			tokens.AddToken("0");
			tokens.AddToken("+");
			tokens.AddToken("4");
			tokens.AddToken("]");
			
			Parser parser = new Parser ( TestModule.GetTestKernel() );
			parser.Parse ( tokens );
			
			Assert.AreEqual(1, parser.MainBlock.Count);
			Assert.IsInstanceOfType ( typeof ( Variable ), parser.MainBlock[0] );
		}

		[Test()]
		public void TestAccessArray ()
		{
			IKernel kernel = TestModule.GetTestKernel ();
			var factory = kernel.Get<IExecutorFactory>();
			
			Block statements = new Block ();
			statements.Scope = new Scope ();
			
			ArrayExpr array = new ArrayExpr(factory.GetArrayExecutor());
			array.Scope = statements.Scope;
			
			array.Elements.Add(new StringLiteral {
				Value = "yay"
			});

			array.Elements.Add(new StringLiteral {
				Value = "boo"
			});
			
			Assign assign = new Assign(factory.GetAssignExecutor())
			{
				Ident = new Variable ( factory.GetVariableExecutor ())
				{
					Scope = statements.Scope,
					Ident = "x"	
				},
				Expr = array,
				Scope = statements.Scope
			};

			Print print = new Print(factory.GetPrintExecutor());			
			print.Expr = new Variable( factory.GetVariableExecutor()) 
			{
				Scope = statements.Scope,
				Ident = "x",
				Indexer = new NumberLiteral()
				{
					Value = 1
				}	
			};
				
			statements.Add ( assign );
			statements.Add ( print );

			statements.Execute();
			
			StandardOutDummy output = kernel.Get<IStandardOut> () as StandardOutDummy;
			Assert.AreEqual ( "boo", output.Text );
		}
	}
}
