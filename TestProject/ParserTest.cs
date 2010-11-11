using Ongle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Ninject;

namespace TestProject
{
    
    
    /// <summary>
    ///This is a test class for ParserTest and is intended
    ///to contain all ParserTest Unit Tests
    ///</summary>
	[TestClass ()]
	public class ParserTest
	{


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for Parser Constructor
		///</summary>
		[TestMethod ()]
		public void ParseSimplePrintTest ()
		{
			List<Token> tokens = new List<Token>();
			tokens.Add(new Token(">"));
			tokens.Add(new Token("'ongle'"));

			Parser parser = new Parser ( TestModule.GetTestKernel() );
			parser.Parse ( tokens );
			
			Assert.AreEqual(1, parser.Statements.Count);
			Assert.IsInstanceOfType ( parser.Statements[0], typeof ( Print ) );
		}

		[TestMethod ()]
		public void StringLiteralPrintTest ()
		{
			List<Token> tokens = new List<Token> ();
			tokens.Add ( new Token ( ">" ) );
			tokens.Add ( new Token ( "'ongle'" ) );

			Parser parser = new Parser ( TestModule.GetTestKernel () );
			parser.Parse ( tokens );

			Assert.IsInstanceOfType ( parser.Statements[0], typeof ( Print ) );
			Print print = (Print)parser.Statements[0];
			Assert.IsInstanceOfType ( print.Expr, typeof ( StringLiteral ) );
			Assert.AreEqual ( ((StringLiteral)print.Expr).Value, "ongle" );
		}

		[TestMethod ()]
		public void ParsePrintAddingStringsTest ()
		{
			List<Token> tokens = new List<Token> ();
			tokens.Add ( new Token ( ">" ) );
			tokens.Add ( new Token ( "'ongle'" ) );
			tokens.Add ( new Token ( "+" ) );
			tokens.Add ( new Token ( "'ooog'" ) );

			Parser parser = new Parser ( TestModule.GetTestKernel () );
			parser.Parse ( tokens );

			Assert.IsInstanceOfType ( ( (Print)parser.Statements[0] ).Expr, typeof ( ArithExpr ) );
		}

		[TestMethod ()]
		public void VariableAssignmentTest ()
		{
			List<Token> tokens = new List<Token> ();
			tokens.Add ( new Token ( "ong" ) );
			tokens.Add ( new Token ( "=" ) );
			tokens.Add ( new Token ( "10" ) );

			Parser parser = new Parser ( TestModule.GetTestKernel () );
			parser.Parse ( tokens );

			Assert.AreEqual ( 1, parser.Statements.Count );
			Assert.IsInstanceOfType ( parser.Statements[0], typeof ( Assign ) );
			Assert.AreEqual ( ((Assign)parser.Statements[0]).Ident, "ong" );
		}

		[TestMethod ()]
		public void ExpressionAssignmentTest ()
		{
			List<Token> tokens = new List<Token> ();
			tokens.Add ( new Token ( "ong" ) );
			tokens.Add ( new Token ( "=" ) );
			tokens.Add ( new Token ( "bong" ) );
			tokens.Add ( new Token ( "+" ) );
			tokens.Add ( new Token ( "10" ) );

			Parser parser = new Parser ( TestModule.GetTestKernel () );
			parser.Parse ( tokens );

			ArithExpr expression = (ArithExpr)( (Assign)parser.Statements[0] ).Expr;
			Assert.IsInstanceOfType ( expression.Left, typeof(Variable ));
			Assert.AreEqual ( ArithOp.Add, expression.Op );
			Assert.AreEqual ( 10, ((NumberLiteral) expression.Right).Value );
		}

		[TestMethod ()]
		public void ParseBlockAssignment ()
		{
			List<Token> tokens = new List<Token> ();
			tokens.Add ( new Token ( "x" ) );
			tokens.Add ( new Token ( "=" ) );
			tokens.Add ( new Token ( "{" ) );
			tokens.Add ( new Token ( ">" ) );
			tokens.Add ( new Token ( "'bong'" ) );
			tokens.Add ( new Token ( "}" ) );

			Parser parser = new Parser ( TestModule.GetTestKernel () );
			parser.Parse ( tokens );

			Assert.IsInstanceOfType ( parser.Statements[0], typeof ( Assign ) );
			Assign ass = parser.Statements[0] as Assign;
			Assert.IsInstanceOfType ( ass.Expr, typeof ( Block ) );
		}

		[TestMethod ()]
		public void ParseBlockCall ()
		{
			List<Token> tokens = new List<Token> ();
			tokens.Add ( new Token ( "x" ) );
			tokens.Add ( new Token ( "=" ) );
			tokens.Add ( new Token ( "{" ) );
			tokens.Add ( new Token ( ">" ) );
			tokens.Add ( new Token ( "'bong'" ) );
			tokens.Add ( new Token ( "}" ) );
			tokens.Add ( new Token ( "x" ) );

			Parser parser = new Parser ( TestModule.GetTestKernel () );
			parser.Parse ( tokens );

			Assert.IsInstanceOfType ( parser.Statements[1], typeof ( Variable ) );
			Assert.AreEqual ( "x", (parser.Statements[1] as Variable).Ident );
		}


		[TestMethod ()]
		public void ParseSimpleIfCreatesOneStatementTest ()
		{
			Parser parser = ParseSimpleIf ();

			Assert.AreEqual ( 1, parser.Statements.Count );
			Assert.IsInstanceOfType ( parser.Statements[0], typeof ( If ) );
		}

		//[TestMethod ()]
		//public void ParseSimpleIfHasCorrectExpressionTest ()
		//{
		//    Parser parser = ParseSimpleIf ();

		//    Assert.AreEqual ( 1, parser.Statements.Count );
		//    Assert.IsInstanceOfType ( (parser.Statements[0] as If).Test, typeof ( Comparison ) );
		//}


		private static Parser ParseSimpleIf ()
		{
			List<Token> tokens = new List<Token> ();
			tokens.Add ( new Token ( "?" ) );
			tokens.Add ( new Token ( "x" ) );
			tokens.Add ( new Token ( "==" ) );
			tokens.Add ( new Token ( "5" ) );
			tokens.Add ( new Token ( ">" ) );
			tokens.Add ( new Token ( "'yay'" ) );
			tokens.Add ( new Token ( "." ) );

			Parser parser = new Parser ( TestModule.GetTestKernel () );
			parser.Parse ( tokens );
			return parser;
		}

	}
}
