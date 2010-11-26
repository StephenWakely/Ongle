using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;

namespace Ongle
{

	public class PrintExecutor : IPrintExecutor
	{
		IStandardOut _output;
		IDebugInfo _debug;

		[Inject]
		public PrintExecutor ( IStandardOut output, IDebugInfo debug )
		{
			_output = output;
			_debug = debug;
		}

		public IScope Scope
		{
			get;
			set;
		}

		public void Execute ( Print info )
		{
			_debug.PrintDebugInfo ( "Printing" );
			
			_output.Output ( info.Expr.Evaluate ().StringValue );
		}
	}

	public class AssignExecutor : IAssignExecutor
	{
		IDebugInfo _debug;

		[Inject]
		public AssignExecutor ( IDebugInfo debug )
		{
			_debug = debug;
		}

		public IScope Scope
		{
			get;
			set;
		}

		public void Execute ( Assign info )
		{
			_debug.PrintDebugInfo ( "Assigning : " + info.Ident.Ident );
			
			if (Scope == null)
				Console.WriteLine ( "Scope is null" );

			if (info.Ident.Indexer != null )
				Scope.SetDynamic ( info.Ident.Ident, info.Ident.Indexer.Evaluate (), info.Expr.Evaluate () );
			else
				Scope.SetDynamic ( info.Ident.Ident, info.Expr.Evaluate () );
		}
	}

	public class IfExecutor : IIfExecutor
	{
		IDebugInfo _debug;

		[Inject]
		public IfExecutor ( IDebugInfo debug )
		{
			_debug = debug;
		}

		public IScope Scope
		{
			get;
			set;
		}

		/// <summary>
		/// Returns a variable to execute if it was the tail call of the if block. 
		/// </summary>
		public ITailCallExecution Execute ( If info )
		{
			_debug.PrintDebugInfo ( "Checking If" );
			
			Dynamic test = info.Test.Evaluate ();

			if ( test.BoolValue )
			{
				_debug.PrintDebugInfo ( "If passed" );
				
				return info.Body.ExecuteBlockWithTailCallElimination ();
			}
			
			return null;
		}
	}

	public class VariableExecutor : IVariableExecutor
	{
		IDebugInfo _debug;

		[Inject]
		public VariableExecutor ( IDebugInfo debug )
		{
			_debug = debug;
		}

		public IScope Scope
		{
			get;
			set;
		}

		/// <summary>
		/// Calls a block that this variable is assigned to.
		/// Returns a Variable which holds a block if that was the 
		/// tail call of this block.
		/// </summary>
		public ITailCallExecution Execute ( Variable variable, Dynamic parameters )
		{
			Dynamic dynamic = Scope.GetDynamic ( variable.Ident );
			if (dynamic.Type == DynamicType.arrayType)
			{
				Dynamic index = variable.Indexer.Evaluate();
				dynamic = dynamic.ArrayValue[(Int32)Math.Truncate (index.NumberValue)];
			}

			if ( dynamic.BlockValue != null )
			{
				_debug.PrintDebugInfo ( "Executing block : " + variable.Ident );
				
				dynamic.BlockValue.Scope.AddDynamic ( "$", parameters );
				ITailCallExecution tailCall = dynamic.BlockValue.ExecuteBlockWithTailCallElimination ();
				
				if (tailCall != null)
					_debug.PrintDebugInfo ( "Tail call returned : " + tailCall.ToString()  );
				
				return tailCall;
			}
			
			return null;
		}
	}
}
