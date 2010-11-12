using Ongle;
using NUnit.Framework;
using System.Collections.Generic;
using Ninject;

namespace Tests
{
    
    
    /// <summary>
    ///This is a test class for ParserTest and is intended
    ///to contain all ParserTest Unit Tests
    ///</summary>
	[TestFixture()]
	public class ParserTest
	{



		/// <summary>
		///A test for Parser Constructor
		///</summary>
		[Test ()]
		public void ParseSimplePrintTest ()
		{
			Tokens tokens = new Tokens();
			tokens.Add(new Token("print"));
			tokens.Add(new Token("'ongle'"));

			Parser parser = new Parser ( TestModule.GetTestKernel() );
			parser.Parse ( tokens );
			
			Assert.AreEqual(1, parser.MainBlock.Count);
			Assert.IsInstanceOfType ( typeof ( Print ), parser.MainBlock[0] );
		}

		[Test ()]
		public void StringLiteralPrintTest ()
		{
			Tokens tokens = new Tokens ();
			tokens.Add ( new Token ( "print" ) );
			tokens.Add ( new Token ( "'ongle'" ) );

			Parser parser = new Parser ( TestModule.GetTestKernel () );
			parser.Parse ( tokens );

			Assert.IsInstanceOfType ( typeof ( Print ), parser.MainBlock[0] );
			Print print = (Print)parser.MainBlock[0];
			Assert.IsInstanceOfType ( typeof ( StringLiteral ), print.Expr );
			Assert.AreEqual ( ((StringLiteral)print.Expr).Value, "ongle" );
		}

		[Test ()]
		public void ParsePrintAddingStringsTest ()
		{
			Tokens tokens = new Tokens ();
			tokens.Add ( new Token ( "print" ) );
			tokens.Add ( new Token ( "'ongle'" ) );
			tokens.Add ( new Token ( "+" ) );
			tokens.Add ( new Token ( "'ooog'" ) );

			Parser parser = new Parser ( TestModule.GetTestKernel () );
			parser.Parse ( tokens );

			Assert.IsInstanceOfType ( typeof ( ArithExpr ), ( (Print)parser.MainBlock[0] ).Expr );
		}

		[Test ()]
		public void VariableAssignmentTest ()
		{
			Tokens tokens = new Tokens ();
			tokens.Add ( new Token ( "ong" ) );
			tokens.Add ( new Token ( "=" ) );
			tokens.Add ( new Token ( "10" ) );

			Parser parser = new Parser ( TestModule.GetTestKernel () );
			parser.Parse ( tokens );

			Assert.AreEqual ( 1, parser.MainBlock.Count );
			Assert.IsInstanceOfType ( typeof ( Assign ), parser.MainBlock[0] );
			Assert.AreEqual ( ((Assign)parser.MainBlock[0]).Ident, "ong" );
		}

		[Test ()]
		public void ExpressionAssignmentTest ()
		{
			Tokens tokens = new Tokens ();
			tokens.Add ( new Token ( "ong" ) );
			tokens.Add ( new Token ( "=" ) );
			tokens.Add ( new Token ( "bong" ) );
			tokens.Add ( new Token ( "+" ) );
			tokens.Add ( new Token ( "10" ) );

			Parser parser = new Parser ( TestModule.GetTestKernel () );
			parser.Parse ( tokens );

			ArithExpr expression = (ArithExpr)( (Assign)parser.MainBlock[0] ).Expr;
			Assert.IsInstanceOfType ( typeof ( Variable ), expression.Left);
			Assert.AreEqual ( ArithOp.Add, expression.Op );
			Assert.AreEqual ( 10, ((NumberLiteral) expression.Right).Value );
		}

		[Test ()]
		public void ParseBlockAssignment ()
		{
			Tokens tokens = new Tokens ();
			tokens.Add ( new Token ( "x" ) );
			tokens.Add ( new Token ( "=" ) );
			tokens.Add ( new Token ( "{" ) );
			tokens.Add ( new Token ( "print" ) );
			tokens.Add ( new Token ( "'bong'" ) );
			tokens.Add ( new Token ( "}" ) );

			Parser parser = new Parser ( TestModule.GetTestKernel () );
			parser.Parse ( tokens );

			Assert.IsInstanceOfType ( typeof ( Assign ), parser.MainBlock[0] );
			Assign ass = parser.MainBlock[0] as Assign;
			Assert.IsInstanceOfType ( typeof ( Block ), ass.Expr );
		}

		[Test ()]
		public void ParseBlockCall ()
		{
			Tokens tokens = new Tokens ();
			tokens.Add ( new Token ( "x" ) );
			tokens.Add ( new Token ( "=" ) );
			tokens.Add ( new Token ( "{" ) );
			tokens.Add ( new Token ( "print" ) );
			tokens.Add ( new Token ( "'bong'" ) );
			tokens.Add ( new Token ( "}" ) );
			tokens.Add ( new Token ( "x" ) );

			Parser parser = new Parser ( TestModule.GetTestKernel () );
			parser.Parse ( tokens );

			Assert.IsInstanceOfType ( typeof ( Assign ), parser.MainBlock[0] );
			Assert.IsInstanceOfType ( typeof ( Variable ), parser.MainBlock[1] );
			Assert.AreEqual ( "x", (parser.MainBlock[1] as Variable).Ident );
		}


		[Test ()]
		public void ParseSimpleIfCreatesOneStatementTest ()
		{
			Parser parser = ParseSimpleIf ();

			Assert.AreEqual ( 1, parser.MainBlock.Count );
			Assert.IsInstanceOfType ( typeof ( If ), parser.MainBlock[0] );
		}

		[Test ()]
		public void ParseNestedBlockTest ()
		{
			Tokens tokens = new Tokens ();
			tokens.AddTokens ( new string[] { "x", "=", "{", "if", "x", "==", "5", "{", "print", "x", "}", "}", "print", "x" } );
			
			Parser parser = new Parser ( TestModule.GetTestKernel () );
			Block block = parser.Parse ( tokens );
			
			Assert.AreEqual ( 2, block.Count, "Should be two statements, one assign, one print");
		}
			
		
		private static Parser ParseSimpleIf ()
		{
			Tokens tokens = new Tokens ();
			tokens.Add ( new Token ( "if" ) );
			tokens.Add ( new Token ( "x" ) );
			tokens.Add ( new Token ( "==" ) );
			tokens.Add ( new Token ( "5" ) );
			tokens.Add ( new Token ( "{" ) );
			tokens.Add ( new Token ( "print" ) );
			tokens.Add ( new Token ( "'yay'" ) );
			tokens.Add ( new Token ( "}" ) );

			Parser parser = new Parser ( TestModule.GetTestKernel () );
			parser.Parse ( tokens );
			return parser;
		}

	}
}
