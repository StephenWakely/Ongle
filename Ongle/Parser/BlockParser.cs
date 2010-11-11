using System;
using System.Collections.Generic;
using Ninject;

namespace Ongle
{

	public class BlockParser : IBlockParser
	{
		const string BeginBlock = "{";
		const string EndBlock = "}";		
		const string PrintTag = ">";
		const string IfTag = "?";
		const string EndTag = ".";


		private IExecutorFactory _executorFactory;
		private IValueParser _valueParser;
		
		[Inject]
		public BlockParser (IExecutorFactory executorFactory, IValueParser valueParser )
		{
			this._executorFactory = executorFactory;
			this._valueParser = valueParser;
			this._valueParser.BlockParser = this;
		}

		public Block GetBlock ( IScope scope, Tokens tokens )
		{
			Block newBlock = new Block ();
			newBlock.Scope = scope;
			ParseBlock ( newBlock, tokens );

			return newBlock;
		}

		public void ParseBlock ( Block block, Tokens tokens )
		{
			bool enclosed = false;
			Token currentToken = tokens[0];
			if ( currentToken.Value == BeginBlock )
			{
				enclosed = true;
				tokens.Remove ( currentToken );
			}

			while ( tokens.Count > 0 )
			{
				currentToken = tokens[0];

				if ( ( enclosed && currentToken.Value == EndBlock ) || currentToken.Value == EndTag )
				{
					tokens.Remove ( currentToken );
					break;
				}

				if ( currentToken.Value == PrintTag )
				{
					// Print statement
					tokens.Remove ( currentToken );
					Print print = new Print( _executorFactory.GetPrintExecutor ());
					print.Expr = ParseExpression ( block, tokens );

					block.Add ( print );
				}
				else if ( currentToken.Value == IfTag )
				{
					// If statement
					tokens.Remove ( currentToken );
					If iff = new If ( _executorFactory.GetIfExecutor ());
					iff.Test = ParseExpression ( block, tokens );
					iff.Body = GetBlock ( block.Scope, tokens );

					block.Add ( iff );
				}
				else
				{
					Token firstToken = currentToken;
					tokens.Remove ( currentToken );

					// Look ahead to see if this is an assignment
					if ( tokens.Count > 0 && tokens[0].Value == "=" )
					{
						Token nextToken = tokens[0];
						tokens.Remove ( nextToken );

						// it is an assignment.
						Assign assign = new Assign ( _executorFactory.GetAssignExecutor() );
						assign.Ident = firstToken.Value;

						block.Add ( assign );

						Expression expression = ParseExpression ( block, tokens );
						assign.Expr = expression;
					}
					else
					{
						// This is a block call
						Variable variable = new Variable ( _executorFactory.GetVariableExecutor() );
						variable.Ident = firstToken.Value;
						block.Add ( variable );
					}
				}
			}
		}

		private Expression ParseExpression ( Block block, Tokens tokens )
		{
			Expression leftExpression = null;

			// Get the left value
			leftExpression = _valueParser.ParseValue ( block.Scope, tokens );

			if ( leftExpression == null )
			{
				throw new Exception ( "Expecting a value" );
			}

			// Check if there is an operator which will continue the expression
			if ( tokens.Count > 0 )
			{
				ArithOp op = ParseOperator ( tokens );
				if ( op != ArithOp.none )
				{
					Expression rightExpression = ParseExpression ( block, tokens );

					if ( rightExpression != null )
					{
						ArithExpr arithExpression = new ArithExpr
						{
							Scope = block.Scope,
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
			Token nextToken = tokens[0];
			if ( nextToken.Value == "+" )
			{
				tokens.Remove ( nextToken );
				return ArithOp.Add;
			}
			else if ( nextToken.Value == "-" )
			{
				tokens.Remove ( nextToken );
				return ArithOp.Sub;
			}
			else if ( nextToken.Value == "*" )
			{
				tokens.Remove ( nextToken );
				return ArithOp.Mul;
			}
			else if ( nextToken.Value == "/" )
			{
				tokens.Remove ( nextToken );
				return ArithOp.Div;
			}
			else if ( nextToken.Value == "==" )
			{
				tokens.Remove ( nextToken );
				return ArithOp.Equality;
			}

			return ArithOp.none;
		}

	}
}
