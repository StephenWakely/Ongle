using Ongle;
using NUnit.Framework;
using System.Collections.Generic;
using Ninject;

namespace Tests
{
    
    
    /// <summary>
    ///This is a test class for ExecutorTest and is intended
    ///to contain all ExecutorTest Unit Tests
    ///</summary>
	[TestFixture()]
	public class ExecutorTest
	{

		/// <summary>
		///A test for Executor Constructor
		///</summary>
		[Test ()]
		public void SimplePrintTest ()
		{
			IKernel kernel = TestModule.GetTestKernel ();
			IExecutorFactory factory = kernel.Get<IExecutorFactory> (); 
			Print print = new Print ( factory.GetPrintExecutor() );
			print.Expr = new StringLiteral {
				Value = "Yo planet!"
			};

			Block statements = new Block ();
			statements.Add ( print );

			new Executor ( statements );

			StandardOutDummy output = kernel.Get<IStandardOut> () as StandardOutDummy;
			Assert.AreEqual( "Yo planet!", output.Text );
		}

		[Test ()]
		public void PrintExpressionTest ()
		{
			IKernel kernel = TestModule.GetTestKernel ();
			IExecutorFactory factory = kernel.Get<IExecutorFactory> (); 
			Print print = new Print ( factory.GetPrintExecutor() );
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

			new Executor ( statements );

			StandardOutDummy output = kernel.Get<IStandardOut> () as StandardOutDummy;
			Assert.AreEqual ( "Yo planet!", output.Text );
		}

		[Test ()]
		public void StringVariableTest ()
		{
			IKernel kernel = TestModule.GetTestKernel ();
			IExecutorFactory factory = kernel.Get<IExecutorFactory> (); 
			Block statements = new Block ();
			statements.Scope = new Scope ();

			Assign assign = new Assign ( factory.GetAssignExecutor() );
			assign.Scope = statements.Scope;
			assign.Ident = "x";
			assign.Expr = new StringLiteral
			{
				Value = "Yo planet!"
			};
			statements.Add ( assign );

			Print print = new Print ( factory.GetPrintExecutor() );
			print.Scope = statements.Scope;
			print.Expr = new Variable ( factory.GetVariableExecutor () );
			( print.Expr as Variable ).Ident = "x";
			( print.Expr as Variable ).Scope = statements.Scope;
			statements.Add ( print );

			new Executor ( statements );

			StandardOutDummy output = kernel.Get<IStandardOut> () as StandardOutDummy;
			Assert.AreEqual ( "Yo planet!", output.Text );
		}

		[Test ()]
		public void SimpleIfTest ()
		{
			IKernel kernel = TestModule.GetTestKernel ();
			IExecutorFactory factory = kernel.Get<IExecutorFactory> (); 
			Block statements = new Block ();
			statements.Scope = new Scope ();

			Assign assign = new Assign ( factory.GetAssignExecutor () );
			assign.Scope = statements.Scope;
			assign.Ident = "x";
			assign.Expr = new NumberLiteral
			{
				Value = 5
			};
			statements.Add ( assign );

			If iif = new If ( factory.GetIfExecutor() );
			Variable variable = new Variable ( factory.GetVariableExecutor () );
			( variable as Variable ).Ident = "x";

			iif.Scope = statements.Scope;
			iif.Test = new ArithExpr
			{
				Scope = iif.Scope,
				Left = variable,
				Op = ArithOp.Equality,
				Right = new NumberLiteral
				{
					Value = 5
				}
			};

			iif.Body = new Block ();
			iif.Body.Scope = statements.Scope;

			Print print = new Print ( factory.GetPrintExecutor () );
			Variable call = new Variable ( factory.GetVariableExecutor () );
			( call as Variable ).Ident = "x";
			print.Expr = call;
			
			print.Scope = iif.Body.Scope;
			call.Scope = iif.Body.Scope;
			iif.Body.Add( print );
			statements.Add ( iif );

			new Executor ( statements );

			StandardOutDummy output = kernel.Get<IStandardOut> () as StandardOutDummy;
			Assert.AreEqual ( "5", output.Text );
		}

		[Test ()]
		public void ExecuteBlockTest ()
		{
			IKernel kernel = TestModule.GetTestKernel ();
			IExecutorFactory factory = kernel.Get<IExecutorFactory> (); 
			Block statements = new Block ();
			statements.Scope = new Scope();

			Assign assign = new Assign ( factory.GetAssignExecutor () );
			assign.Ident = "x";
			assign.Scope = statements.Scope;
				
			Block block = new Block();
			block.Scope = statements.Scope;
			
			Print print = new Print ( factory.GetPrintExecutor () );
			print.Scope = block.Scope;
			print.Expr = new StringLiteral { Value = "Yo planet!" };
			block.Add(print);

			assign.Expr = block;

			Assert.IsTrue ( assign.Scope != null );
			statements.Add ( assign );
			Variable variable = new Variable ( factory.GetVariableExecutor () );
			variable.Ident = "x";
			variable.Scope = statements.Scope;
			statements.Add ( variable );

			new Executor ( statements );

			StandardOutDummy output = kernel.Get<IStandardOut> () as StandardOutDummy;
			Assert.AreEqual ( "Yo planet!", output.Text );
		}

	}
}
