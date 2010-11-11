
using System;
using System.Collections.Generic;

namespace Ongle
{


	public class Tokens
	{
		private List<Token> _list = new List<Token>();

		public Tokens ()
		{
		}
		
		public void Add(Token token)
		{
			_list.Add(token);	
		}
		
		public Token this[int index]
		{
			get
			{
				return _list[index];	
			}			
		}
		
		public int Count
		{
			get
			{
				return _list.Count;	
			}				
		}
		
		public void Remove(Token token)
		{
			_list.Remove(token);			
		}
		
		public void RemoveAt(int index)
		{
			_list.RemoveAt ( index );	
		}
		
		public bool AtEnd()
		{
			return _list.Count == 0;
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
			
			return _list[0].Value;	
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
			if (!this.AtEnd())
				_list.RemoveAt(0);
			
			return result;
		}
		
		/// <summary>
		/// Removes the next token from the list if it is the given value.
		/// </summary>
		/// <param name="tokenValue">
		/// A <see cref="System.String"/>
		/// </param>
		public void RemoveNextToken ( string tokenValue )
		{
			if (_list[0].Value == tokenValue) 
				_list.RemoveAt(0);	
		}
	}
}
