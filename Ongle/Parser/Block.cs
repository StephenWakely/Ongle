
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
			foreach ( IStatement statement in _statements )
			{
				statement.Execute ();
			}
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
