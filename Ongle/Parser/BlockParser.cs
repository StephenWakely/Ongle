using System;
using System.Collections.Generic;
using Ninject;

namespace Ongle
{

	public class BlockParser : IBlockParser
	{
		const string BeginBlock = "{";
		const string EndBlock = "}";		
		const string PrintTag = "print";
		const string IfTag = "if";
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
					print.Expr = ParseExpression ( block.Scope, tokens );

					block.Add ( print );
				}
				else if ( currentToken.Value == IfTag )
				{
					// If statement
					tokens.Remove ( currentToken );
					If iff = new If ( _executorFactory.GetIfExecutor ());
					iff.Test = ParseExpression ( block.Scope, tokens );
					iff.Body = GetBlock ( block.Scope, tokens );

					block.Add ( iff );
				}
				else
				{
					string firstToken = tokens.PullToken ();

					// Look ahead to see if this is an assignment
					if ( tokens.Count > 0 && tokens.PeekToken () == "=" )
					{
						tokens.PullToken ();

						// it is an assignment.
						Assign assign = new Assign ( _executorFactory.GetAssignExecutor() );
						assign.Ident = firstToken;

						block.Add ( assign );

						Expression expression = ParseExpression ( block.Scope, tokens );
						assign.Expr = expression;
					}
					else
					{
						// This is a block call
						Variable variable = new Variable ( _executorFactory.GetVariableExecutor() );
						variable.Scope = block.Scope;
						variable.Ident = firstToken;

						// Check if there is an indexer into the variable
						if (tokens.PeekToken () == "[")
						{
							tokens.PullToken ();
							variable.Indexer = ParseExpression ( block.Scope, tokens );
							
							tokens.RemoveNextToken ("]");
						}
						
						block.Add ( variable );
					}
				}
			}
		}

		public Expression ParseExpression ( IScope scope, Tokens tokens )
		{
			Expression leftExpression = null;

			// Get the left value
			leftExpression = _valueParser.ParseValue ( scope, tokens );

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
			else if ( nextToken.Value == "<" )
			{
				tokens.Remove ( nextToken );
				return ArithOp.LessThan;               
			}
			else if ( nextToken.Value == ">" )
			{
				tokens.Remove ( nextToken );
				return ArithOp.GreaterThan;               
			}

			return ArithOp.none;
		}

	}
}
