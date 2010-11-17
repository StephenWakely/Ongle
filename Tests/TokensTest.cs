using System;
using NUnit.Framework;
using Ongle;
namespace Tests
{
	[TestFixture()]
	public class TokensTest
	{
		[Test()]
		public void TestSetRollbackPoint ()
		{
			Tokens tokens = new Tokens();
			tokens.AddTokens ( new string[] { "1", "2", "3", "4", "5" } );
			
			Assert.AreEqual ( "1", tokens.PeekToken () );
			
			tokens.SetMark ();
			Assert.AreEqual ( "1", tokens.PullToken () );
			Assert.AreEqual ( "2", tokens.PullToken () );
			Assert.AreEqual ( "3", tokens.PullToken () );

			tokens.RollbackToMark ();
			Assert.AreEqual ( "1", tokens.PeekToken () );			
		}
	}
}

