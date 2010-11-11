using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;

namespace Ongle
{

	public class DeclareVarExecutor : IDeclareVarExecutor
	{
		[Inject]
		public DeclareVarExecutor ( )
		{
		}

		public IScope Scope
		{
			get;
			set;
		}

		public void Execute ( DeclareVar info )
		{
			Scope.SetDynamic ( info.Ident, info.Expr.Evaluate () );
		}
	}

	public class PrintExecutor : IPrintExecutor
	{
		IStandardOut _output;

		[Inject]
		public PrintExecutor ( IStandardOut output )
		{
			_output = output;
		}

		public IScope Scope
		{
			get;
			set;
		}

		public void Execute ( Print info )
		{
			_output.Output ( info.Expr.Evaluate ().StringValue );
		}
	}

	public class AssignExecutor : IAssignExecutor
	{
		[Inject]
		public AssignExecutor ( )
		{
		}

		public IScope Scope
		{
			get;
			set;
		}

		public void Execute ( Assign info )
		{
			if (Scope == null)
				Console.WriteLine ( "Scope is null" );

			Scope.SetDynamic ( info.Ident, info.Expr.Evaluate () );
		}
	}

	public class IfExecutor : IIfExecutor
	{
		[Inject]
		public IfExecutor ( )
		{
		}

		public IScope Scope
		{
			get;
			set;
		}

		public void Execute ( If info )
		{
			Dynamic test = info.Test.Evaluate ();

			if ( test.BoolValue )
				info.Body.Execute ();
		}
	}

	public class VariableExecutor : IVariableExecutor
	{
		[Inject]
		public VariableExecutor ( )
		{
		}

		public IScope Scope
		{
			get;
			set;
		}

		public void Execute ( Variable variable )
		{
			Dynamic dynamic = Scope.GetDynamic ( variable.Ident );
			if ( dynamic.BlockValue != null )
			{
				dynamic.BlockValue.Execute ();
			}
		}
	}
}