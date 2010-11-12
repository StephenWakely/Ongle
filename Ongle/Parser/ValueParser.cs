using System;
using System.Collections.Generic;
using Ninject;

namespace Ongle
{
	public class ValueParser : IValueParser
	{
		const string BeginBlock = "{";
		const string EndBlock = "}";		
		const string PrintTag = ">";
		const string IfTag = "if";
		const string EndTag = ".";
		const string BeginArray = "(";
		const string EndArray = ")";
		const string StringDelimeter = "'";
		const string ArraySeparator = ",";


		private IBlockParser _blockParser;
		private IExecutorFactory _executorFactory;
		
		[Inject]
		public ValueParser ( IExecutorFactory executorFactory)
		{
			_executorFactory = executorFactory;
		}
		
		/// <summary>
		/// Block parser has to be a property so ninject
		/// can resolve the circular dependency.
		/// </summary>
		public IBlockParser BlockParser
		{
			set
			{
				_blockParser = value;	
			}
		}
		
		/// <summary>
		/// Value is a constant string, number or variable name.
		/// </summary>
		/// <param name="tokens"></param>
		/// <returns></returns>
		public Expression ParseValue ( IScope scope, Tokens tokens )
		{
			Expression value = null;

			if ( tokens.NextTokenIs ( BeginBlock ) )
			{
				value = _blockParser.GetBlock ( scope, tokens );
			}
			else if ( tokens.PeekToken().StartsWith ( "'" ) && tokens.PeekToken().EndsWith ( "'" ) )
			{
				string literal = tokens.PullToken().Trim ( '\'' );
				value = new StringLiteral
				{
					Value = literal
				};
			}
			else if ( tokens.PeekToken().IsNumber () )
			{
				value = new NumberLiteral
				{
					Value = double.Parse ( tokens.PullToken() )
				};
			}
			else if ( tokens.NextTokenIs(BeginArray) )
			{
				return this.ParseArray ( scope, tokens );	
			}
			else
			{
				var variable = new Variable ( _executorFactory.GetVariableExecutor () );
				variable.Scope = scope;
				variable.Ident = tokens.PullToken();

								// Check if there is an indexer into the variable
				if (tokens.PeekToken () == "[")
				{
					tokens.PullToken ();
					variable.Indexer = _blockParser.ParseExpression ( scope, tokens );					
					tokens.RemoveNextToken ("]");
				}
				
				value = variable;
			}

			return value;
		}

		public Expression ParseArray(IScope scope, Tokens tokens)
		{
			var result = new ArrayExpr( _executorFactory.GetArrayExecutor() );

			tokens.RemoveNextToken(BeginArray);

			while (tokens.PeekToken() != EndArray)
			{
				result.Elements.Add(this.ParseValue(scope, tokens));
				tokens.RemoveNextToken(ArraySeparator);
			}
			
			tokens.RemoveNextToken(EndArray);

			return result;
		}
	}
}
