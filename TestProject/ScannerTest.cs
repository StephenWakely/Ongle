using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Ongle;

namespace TestProject
{
	/// <summary>
	/// Summary description for ScannerTest
	/// </summary>
	[TestClass]
	public class ScannerTest
	{
		public ScannerTest ()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void ScanSimpleSingleLine ()
		{
			string line = "x = 5";
			MemoryStream stream = new MemoryStream ( new System.Text.ASCIIEncoding().GetBytes(line) );
			Scanner scanner = new Scanner (stream);

			Assert.AreEqual ( 3, scanner.Tokens.Count );
		}

		[TestMethod]
		public void ScanQuotedSingleLine ()
		{
			string line = "x = 'Groove'";
			MemoryStream stream = new MemoryStream ( new System.Text.ASCIIEncoding ().GetBytes ( line ) );
			Scanner scanner = new Scanner ( stream );

			Assert.AreEqual ( 3, scanner.Tokens.Count );
		}

		[TestMethod]
		public void ScanBlock ()
		{
			string line = @"x= {
	> 'grooveit' + y
}";
			MemoryStream stream = new MemoryStream ( new System.Text.ASCIIEncoding ().GetBytes ( line ) );
			Scanner scanner = new Scanner ( stream );
			Assert.AreEqual ( 8, scanner.Tokens.Count );
		}

		[TestMethod]
		public void ScanExpression ()
		{
			string line = "x = a*b";
			MemoryStream stream = new MemoryStream ( new System.Text.ASCIIEncoding ().GetBytes ( line ) );
			Scanner scanner = new Scanner ( stream );

			Assert.AreEqual ( 5, scanner.Tokens.Count );
		}

		[TestMethod]
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
