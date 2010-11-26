
using System;
using System.Collections.Generic;

namespace Ongle
{


	public class Block : Expression
	{		
		private List<IStatement> _statements = new List<IStatement> ();

		public override IScope Scope 
		{
			get 
			{
				return base.Scope;
			}
			set
			{
				// Nest the heap so we create a new scope.
				base.Scope = new Scope(value);
			}
		}

		public override void Execute ()
		{
			ExecuteBlock ( false );
		}
		
		private bool IsTailCall (IStatement statement)
		{
			return statement == _statements[_statements.Count - 1] && statement is ITailCallExecution;
		}

		public ITailCallExecution ExecuteBlock ( bool returnTailCall )
		{
			int currentStatement = 0;
			IStatement statement = _statements[currentStatement];
			
			while ( statement != null )
			{
				// Check if this is the tail call
				if (returnTailCall && IsTailCall(statement))
				{
					return statement as ITailCallExecution;
				}
				
				IStatement nextStatement = null;
				
				if ( statement is ITailCallExecution )
				{
					// Call this statement and handle any tail call it has returned.
					ITailCallExecution tailCall = (statement as ITailCallExecution).ExecuteWithTailCall ();
					if ( tailCall != null )
					{
						// We should call this tail call next
						nextStatement = tailCall; 
					}
					else
					{
						// We should call our next statement 
						currentStatement++;	
					}
				}
				else
				{
					// Execute the next statement as normal
					statement.Execute ();
					currentStatement++;	
				}
				
				if ( nextStatement == null && currentStatement < _statements.Count )
				{
					nextStatement = _statements[currentStatement];					
				}
				
				statement = nextStatement;
			}
			
			return null;
		}

		public ITailCallExecution ExecuteBlockWithTailCallElimination ()
		{
			return ExecuteBlock ( true );
		}

		/// <summary>
		/// Evaluate a block sticks it in a dynamic and returns it 
		/// for later execution.
		/// </summary>
		/// <param name="heap"></param>
		/// <returns></returns>
		public override Dynamic Evaluate ()
		{
			var result = new Dynamic
			{
				Scope = Scope,
				BlockValue = this
			};

			return result;
		}

		public void Add ( IStatement statement )
		{	
			statement.Scope = Scope;			
			_statements.Add ( statement );
		}

		public int Count
		{
			get
			{
				return _statements.Count;
			}
		}

		public IStatement this[int index]
		{
			get
			{
				return _statements[index];
			}
		}
	}
}
