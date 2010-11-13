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
		
		private List<IStatementParser> _parsers = new List<IStatementParser>();
		
		public List<IStatementParser> Parsers
		{
			get
			{
				return _parsers;
			}
		}
		
		public BlockParser ()
		{
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
			if ( tokens.PeekToken () == BeginBlock )
			{
				enclosed = true;
				tokens.RemoveNextToken ( BeginBlock );
			}

			while ( tokens.Count > 0 )
			{
				if ( enclosed && tokens.PeekToken () == EndBlock )
				{
					tokens.RemoveNextToken ( EndBlock );
					break;
				}
				
				bool parsed = false;
				foreach ( IStatementParser parser in _parsers )
				{
					IStatement nextStatement;
					if ( parser.TryParse( tokens, block.Scope, out nextStatement ) )
					{
						block.Add ( nextStatement );
						parsed = true;
						break;
					}
				}
				
				if (!parsed)
					throw new Exception("Unable to parse token " + tokens.PeekToken() );
			}
		}
	}
}
