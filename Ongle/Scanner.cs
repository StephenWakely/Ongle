using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ongle
{
	public class Scanner
	{
		public Tokens Tokens
		{
			get;
			set;
		}

		public Scanner ( Stream input )
		{
			ScanState state = new None ();

			this.Tokens = new Tokens();

			while ( input.Position != input.Length )
			{
				char ch = (char)input.ReadByte ();
				state = state.Action ( ch, this.Tokens );
			}

			state.Eof ( this.Tokens );
		}

	}

	abstract class ScanState
	{
		protected StringBuilder currentToken = new StringBuilder ();

		public ScanState ()
		{
		}

		public ScanState ( char ch )
		{
			currentToken.Append ( ch );
		}

		public void Eof ( Tokens tokens )
		{
			if ( currentToken.Length > 0 )
				tokens.Add ( new Token ( currentToken.ToString () ) );
		}

		public abstract ScanState Action ( char ch, Tokens tokens );

		protected static bool IsSymbol ( char ch )
		{
			return new List<char> {'*','+','=','-','/','?','<','>','{','}','[',']', '(', ')', ','}.Contains(ch);
		}

		protected static bool IsCharacter ( char ch )
		{
			return char.IsLetter ( ch ) || char.IsNumber ( ch ) || ch == '_';
		}

		protected static bool IsTextDelimiter ( char ch )
		{
			return ch == '\'' || ch == '"';
		}

	}

	class None : ScanState
	{
		public override ScanState Action ( char ch, Tokens tokens )
		{
			if ( !char.IsWhiteSpace ( ch ) )
			{
				if ( IsCharacter ( ch ) )
					return new Word ( ch );
				else if ( IsSymbol ( ch ) )
					return new Symbol ( ch );
				else if ( IsTextDelimiter ( ch ) )
					return new Text ( ch );
			}

			return this;
		}

	}

	class Word : ScanState
	{
		public Word ( char ch )
			: base ( ch )
		{
		}

		public override ScanState Action ( char ch, Tokens tokens )
		{
			if ( char.IsWhiteSpace ( ch ) || IsSymbol ( ch ) )
			{
				tokens.Add ( new Token ( currentToken.ToString () ) );

				if ( IsSymbol ( ch ) )
				{
					return new Symbol ( ch );
				}
				else
				{
					return new None ();
				}
			}

			currentToken.Append ( ch );
			return this;
		}
	}

	class Symbol : ScanState
	{
		public Symbol ( char ch )
			: base ( ch )
		{
		}

		public override ScanState Action ( char ch, Tokens tokens )
		{
			if ( char.IsWhiteSpace ( ch ) || IsCharacter ( ch ) )
			{
				tokens.Add ( new Token ( currentToken.ToString () ) );

				if ( IsCharacter ( ch ) )
				{
					return new Word ( ch );
				}
				else
				{
					return new None ();
				}
			}

			currentToken.Append ( ch );
			return this;
		}
	}

	class Text : ScanState
	{
		public Text ( char ch )
			: base ( ch )
		{
		}

		public override ScanState Action ( char ch, Tokens tokens )
		{
			currentToken.Append ( ch );

			if ( IsTextDelimiter ( ch ) )
			{
				Token token = new Token ( currentToken.ToString () );
				tokens.Add ( token );
				return new None ();
			}

			return this;
		}
	}
}
