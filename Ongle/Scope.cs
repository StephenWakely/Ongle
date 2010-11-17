using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ongle
{
	public class Scope : IScope
	{
		private IScope _inheritedScope = null;
		private Dictionary<string, Dynamic> _lookup = new Dictionary<string,Dynamic>();
			
		public Scope ()
		{
			_setupDefaultVariables();
		}
		
		public Scope ( IScope inheritedScope )
		{
			_inheritedScope = inheritedScope;
		}

		/// <summary>
		/// Sets up some useful default global variables 
		/// </summary>
		private void _setupDefaultVariables()
		{
			_lookup.Add ( "newline", new Dynamic { StringValue = Environment.NewLine } );			
		}

		public Dynamic TryGetDynamic ( string identifier )
		{
			Dynamic result = null;
			
			if ( _lookup.ContainsKey ( identifier ) )
				result = _lookup[identifier];
			else if ( _inheritedScope != null )
				result = _inheritedScope.TryGetDynamic ( identifier ); 		
			    
			return result;
		}
		
		public Dynamic GetDynamic ( string identifier )
		{
			Dynamic result = TryGetDynamic ( identifier ); 
			    
			if ( result != null )
				return result;
			
			Dynamic newDynamic = new Dynamic ()
			{
				Scope = this
			};

			_lookup.Add ( identifier, newDynamic );

			return newDynamic;
		}

		/// <summary>
		/// Try to set the variable at a scope level that has this defined. 
		/// </summary>
		/// <returns>
		/// True if variable has been set.
		/// False if variable has not been set <see cref="System.Boolean"/>
		/// </returns>
		public bool TrySetDynamic ( string identifier, Dynamic dynamic )
		{
			if (_lookup.ContainsKey ( identifier ) )
			{
				_lookup[identifier] = dynamic;
				return true;
			}
			else if ( _inheritedScope != null )
				return _inheritedScope.TrySetDynamic( identifier, dynamic );
			
			return false;
		}

		/// <summary>
		/// Setting a variable with an indexer - the original variable must have already been set
		/// </summary>
		public void SetDynamic ( string identifier, Dynamic indexer, Dynamic dynamic )
		{
			Dynamic variable = TryGetDynamic ( identifier );
			if (variable == null) // TODO Better error 
				throw new Exception ("Indexing into non-existant array");
			
			if (variable.Type != DynamicType.arrayType) // TODO Better error 
				throw new Exception ("Indexing into non-array");
			
			variable.ArrayValue[(Int32)Math.Truncate(indexer.NumberValue)] = dynamic;
		}

		public void SetDynamic ( string identifier, Dynamic dynamic )
		{
			if ( !TrySetDynamic ( identifier, dynamic ) ) 
			{
				AddDynamic(identifier, dynamic);
			}
		}
		
		/// <summary>
		/// Adds the varible at this level of scope 
		/// </summary>
		public void AddDynamic ( string identifier, Dynamic dynamic )
		{
			if (dynamic == null)
				_lookup.Remove ( identifier );
			else
				_lookup[identifier] = dynamic;			
		}
	}
}
