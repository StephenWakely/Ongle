using System;
using Ninject;
namespace Ongle
{
	public class ExpressionParser : IExpressionParser
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
		public ExpressionParser ( IBlockParser blockParser, IExecutorFactory executorFactory )
		{
			_blockParser = blockParser;
			_executorFactory = executorFactory;
		}
		
		public Expression ParseExpression ( IScope scope, Tokens tokens )
		{
			Expression leftExpression = null;

			// Get the left value
			leftExpression = ParseValue ( scope, tokens );

			if ( leftExpression == null )
			{
				throw new Exception ( "Expecting a value" );
			}

			// Check if there is an operator which will continue the expression
			if ( !tokens.AtEnd () )
			{
				ArithOp op = ParseOperator ( tokens );
				if ( op != ArithOp.none )
				{
					Expression rightExpression = ParseExpression ( scope, tokens );

					if ( rightExpression != null )
					{
						ArithExpr arithExpression = new ArithExpr
						{
							Scope = scope,
							Left = leftExpression,
							Op = op,
							Right = rightExpression
						};

						return arithExpression;
					}
				}
			}

			return leftExpression;
		}

		private ArithOp ParseOperator ( Tokens tokens )
		{
			if ( tokens.PullTokenIfEqual ( "+" ) )
			{
				return ArithOp.Add;
			}
			else if ( tokens.PullTokenIfEqual ( "-" ) )
			{
				return ArithOp.Sub;
			}
			else if ( tokens.PullTokenIfEqual ( "*" ) )
			{
				return ArithOp.Mul;
			}
			else if ( tokens.PullTokenIfEqual ( "/" ) )
			{
				return ArithOp.Div;
			}
			else if ( tokens.PullTokenIfEqual ( "==" ) )
			{
				return ArithOp.Equality;
			}
			else if ( tokens.PullTokenIfEqual ( "<" ) )
			{
				return ArithOp.LessThan;               
			}
			else if ( tokens.PullTokenIfEqual ( ">" ) )
			{
				return ArithOp.GreaterThan;               
			}

			return ArithOp.none;
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
					variable.Indexer = ParseExpression ( scope, tokens );					
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

