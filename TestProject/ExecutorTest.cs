using Ongle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Ninject;

namespace TestProject
{
    
    
    /// <summary>
    ///This is a test class for ExecutorTest and is intended
    ///to contain all ExecutorTest Unit Tests
    ///</summary>
	[TestClass ()]
	public class ExecutorTest
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
		///A test for Executor Constructor
		///</summary>
		[TestMethod ()]
		public void SimplePrintTest ()
		{
			IKernel kernel = TestModule.GetTestKernel ();
			Print print = kernel.Get<Print> ();
			print.Expr = new StringLiteral {
				Value = "Yo planet!"
			};

			Block statements = new Block ();
			statements.Add ( print );

			Executor target = new Executor ( statements );

			StandardOutDummy output = kernel.Get<IStandardOut> () as StandardOutDummy;
			Assert.AreEqual( "Yo planet!", output.Text );
		}

		[TestMethod ()]
		public void PrintExpressionTest ()
		{
			IKernel kernel = TestModule.GetTestKernel ();
			Print print = kernel.Get<Print> ();
			print.Expr = new ArithExpr
			{
				Left = new StringLiteral
				{
					Value = "Yo "
				},
				Right = new StringLiteral
				{
					Value = "planet!"
				},
				Op = ArithOp.Add
			};

			Block statements = new Block ();
			statements.Add ( print );

			Executor target = new Executor ( statements );

			StandardOutDummy output = kernel.Get<IStandardOut> () as StandardOutDummy;
			Assert.AreEqual ( "Yo planet!", output.Text );
		}

		[TestMethod ()]
		public void StringVariableTest ()
		{
			IKernel kernel = TestModule.GetTestKernel ();
			Block statements = new Block ();

			Assign assign = kernel.Get<Assign> ();
			assign.Ident = "x";
			assign.Expr = new StringLiteral
			{
				Value = "Yo planet!"
			};
			statements.Add ( assign );

			Print print = kernel.Get<Print> ();
			print.Expr = kernel.Get<Variable> ();
			( print.Expr as Variable ).Ident = "x";
			statements.Add ( print );

			Executor target = new Executor ( statements );

			StandardOutDummy output = kernel.Get<IStandardOut> () as StandardOutDummy;
			Assert.AreEqual ( "Yo planet!", output.Text );
		}

		[TestMethod ()]
		public void SimpleIfTest ()
		{
			IKernel kernel = TestModule.GetTestKernel ();
			Block statements = new Block ();

			Assign assign = kernel.Get<Assign> ();
			assign.Ident = "x";
			assign.Expr = new NumberLiteral
			{
				Value = 5
			};
			statements.Add ( assign );

			If iif = kernel.Get<If> ();
			Variable variable = kernel.Get<Variable> ();
			( variable as Variable ).Ident = "x";

			iif.Test = new ArithExpr
			{
				Left = variable,
				Op = ArithOp.Equality,
				Right = new NumberLiteral
				{
					Value = 5
				}
			};

			iif.Body = new Block ();

			Print print = kernel.Get<Print> ();
			Variable call = kernel.Get<Variable> ();
			( call as Variable ).Ident = "x";
			print.Expr = call;
			
			iif.Body.Add( print );
			statements.Add ( iif );

			Executor target = new Executor ( statements );

			StandardOutDummy output = kernel.Get<IStandardOut> () as StandardOutDummy;
			Assert.AreEqual ( "5", output.Text );
		}

		[TestMethod ()]
		public void ExecuteBlockTest ()
		{
			IKernel kernel = TestModule.GetTestKernel ();
			Assign assign = kernel.Get<Assign> ();
			assign.Ident = "x";

			Block block = new Block();
			Print print = kernel.Get<Print> ();
			print.Expr = new StringLiteral { Value = "Yo planet!" };
			block.Add(print);

			assign.Expr = block;

			Block statements = new Block ();
			statements.Add ( assign );
			Variable variable = kernel.Get<Variable> ();
			variable.Ident = "x";
			statements.Add ( variable );

			Executor target = new Executor ( statements );

			StandardOutDummy output = kernel.Get<IStandardOut> () as StandardOutDummy;
			Assert.AreEqual ( "Yo planet!", output.Text );
		}

	}
}
