
using System;
using System.Collections.Generic;

namespace Ongle
{


	public class Tokens
	{
		private List<Token> _list = new List<Token>();
		private int _pos = 0;
		private int _mark = 0;

		public Tokens ()
		{
		}
		
		public void Add(Token token)
		{
			_list.Add(token);	
		}
		
		public int Count
		{
			get { return _list.Count; }	
		}
				
		public void AddToken(string tokenValue)
		{
			if (tokenValue.Trim() != "")
				this.Add (new Token (tokenValue));				
		}
		
		public void AddTokens(string[] tokensValue)
		{
			foreach ( string token in tokensValue )
				this.AddToken ( token );	
		}
				
		
		public bool AtEnd()
		{
			return _pos >= _list.Count;
		}
		
		public bool NextTokenIs( string tokenValue )
		{
			return this.PeekToken() == tokenValue;	
		}
		
		/// <summary>
		/// Returns the next token
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public string PeekToken ()
		{
			if (this.AtEnd())
				return "";
			
			return _list[_pos].Value;	
		}
		
		/// <summary>
		/// Returns the next token and removes if from the list.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public string PullToken ()
		{
			string result = PeekToken();
			_pos++;
			return result;
		}
		
		public bool PullTokenIfEqual ( string tokenValue )
		{
			if ( PeekToken() == tokenValue )
			{
				_pos++;
				return true;
			}
			else
				return false;
		}

		public void SetMark ()
		{
			_mark = _pos;			
		}

		public void RollbackToMark ()
		{
			_pos = _mark;
		}
		
		/// <summary>
		/// Removes the next token from the list if it is the given value.
		/// </summary>
		/// <param name="tokenValue">
		/// A <see cref="System.String"/>
		/// </param>
		public void RemoveNextToken ( string tokenValue )
		{
			if (_list[_pos].Value == tokenValue) 
				_pos++;
		}
	}
}
