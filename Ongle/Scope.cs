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
		
		public Scope ( IScope heap )
		{
			_inheritedScope = heap;
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
			
			if (_lookup.ContainsKey ( identifier ) )
				result = _lookup[identifier];
			else if (_inheritedScope != null)
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
			
		public void SetDynamic ( string identifier, Dynamic dynamic )
		{
			if ( !TrySetDynamic ( identifier, dynamic ) ) 
			{
				_lookup[identifier] = dynamic;
			}
		}
	}
}
