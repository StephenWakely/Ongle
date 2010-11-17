using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.IO;
using Ongle;

namespace Tests
{
	/// <summary>
	/// Summary description for ScannerTest
	/// </summary>
	[TestFixture()]
	public class ScannerTest
	{
		[Test ()]
		public void ScanSimpleSingleLine ()
		{
			string line = "x = 5";
			MemoryStream stream = new MemoryStream ( new System.Text.ASCIIEncoding().GetBytes(line) );
			Scanner scanner = new Scanner (stream);

			Assert.AreEqual ( 3, scanner.Tokens.Count );
		}

		[Test ()]
		public void ScanQuotedSingleLine ()
		{
			string line = "x = 'Groove'";
			MemoryStream stream = new MemoryStream ( new System.Text.ASCIIEncoding ().GetBytes ( line ) );
			Scanner scanner = new Scanner ( stream );

			Assert.AreEqual ( 3, scanner.Tokens.Count );
		}

		[Test ()]
		public void ScanBlock ()
		{
			string line = @"x= {
	> 'grooveit' + y
}";
			MemoryStream stream = new MemoryStream ( new System.Text.ASCIIEncoding ().GetBytes ( line ) );
			Scanner scanner = new Scanner ( stream );
			Assert.AreEqual ( 8, scanner.Tokens.Count );
		}

		[Test ()]
		public void ScanExpression ()
		{
			string line = "x = a*b";
			MemoryStream stream = new MemoryStream ( new System.Text.ASCIIEncoding ().GetBytes ( line ) );
			Scanner scanner = new Scanner ( stream );

			Assert.AreEqual ( 5, scanner.Tokens.Count );
		}

		[Test ()]
		public void ScanLessThanTest ()
		{
			string line = "if x < 50";
			MemoryStream stream = new MemoryStream ( new System.Text.ASCIIEncoding ().GetBytes ( line ) );
			Scanner scanner = new Scanner ( stream );

			Assert.AreEqual ( 4, scanner.Tokens.Count );
		}

		[Test ()]
		public void ScanMultipleLines ()
		{
			string line = @"if ya == 'oof'
xa = 'Groove'
end";
			MemoryStream stream = new MemoryStream ( new System.Text.ASCIIEncoding ().GetBytes ( line ) );
			Scanner scanner = new Scanner ( stream );

			Assert.AreEqual ( 8, scanner.Tokens.Count );
			Assert.AreEqual ( "if", scanner.Tokens.PullToken () );
			Assert.AreEqual ( "ya", scanner.Tokens.PullToken () );
			Assert.AreEqual ( "==", scanner.Tokens.PullToken () );
			Assert.AreEqual ( "'oof'", scanner.Tokens.PullToken () );
			Assert.AreEqual ( "xa", scanner.Tokens.PullToken () );
			Assert.AreEqual ( "=", scanner.Tokens.PullToken () );
			Assert.AreEqual ( "'Groove'", scanner.Tokens.PullToken () );
			Assert.AreEqual ( "end", scanner.Tokens.PullToken () );

		}

	}
}
