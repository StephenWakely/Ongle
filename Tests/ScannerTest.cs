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
		public void ScanMultipleLines ()
		{
			string line = @"? ya == 'oof'
xa = 'Groove'
end";
			MemoryStream stream = new MemoryStream ( new System.Text.ASCIIEncoding ().GetBytes ( line ) );
			Scanner scanner = new Scanner ( stream );

			Assert.AreEqual ( 8, scanner.Tokens.Count );
			Assert.AreEqual ( "?", scanner.Tokens[0].Value );
			Assert.AreEqual ( "ya", scanner.Tokens[1].Value );
			Assert.AreEqual ( "==", scanner.Tokens[2].Value );
			Assert.AreEqual ( "'oof'", scanner.Tokens[3].Value );
			Assert.AreEqual ( "xa", scanner.Tokens[4].Value );
			Assert.AreEqual ( "=", scanner.Tokens[5].Value );
			Assert.AreEqual ( "'Groove'", scanner.Tokens[6].Value );
			Assert.AreEqual ( "end", scanner.Tokens[7].Value );

		}

	}
}
