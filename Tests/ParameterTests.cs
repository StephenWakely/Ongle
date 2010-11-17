using System;
using NUnit.Framework;
using Ongle;
using Ninject;
using System.IO;
namespace Tests
{
	[TestFixture()]
	public class ParameterTests
	{
		
		private Tokens GetBlockCallTokens ()
		{
			Tokens tokens = new Tokens();
			tokens.AddTokens ( new string[] { "x", "=", "{", "print", "$", "[", "0", "]", "}", "x", "(", "'yay'", ")" } );
	
			return tokens;
		}

		[Test()]
		public void TestScanParameterArray ()
		{
			string line = "$[3]";
			MemoryStream stream = new MemoryStream ( new System.Text.ASCIIEncoding().GetBytes(line) );
			Scanner scanner = new Scanner (stream);
			
			Assert.AreEqual ("$", scanner.Tokens.PullToken());
			Assert.AreEqual ("[", scanner.Tokens.PullToken());
			Assert.AreEqual ("3", scanner.Tokens.PullToken());
			Assert.AreEqual ("]", scanner.Tokens.PullToken());

		}

		[Test()]
		public void TestCallHasParameter ()
		{
			
			Parser parser = new Parser ( TestModule.GetTestKernel() );
			parser.Parse ( GetBlockCallTokens () );
			
			Variable variable = parser.MainBlock[1] as Variable;
			Assert.IsInstanceOfType( typeof(ArrayExpr), variable.Parameters );			
		}
		
		[Test()]
		public void TestCallEvaluatesParameters ()
		{
			
			Parser parser = new Parser ( TestModule.GetTestKernel() );
			parser.Parse ( GetBlockCallTokens () );
			
			Variable variable = parser.MainBlock[1] as Variable;
			Dynamic parameters = variable.Parameters.Evaluate();

			Assert.AreEqual ( DynamicType.arrayType, parameters.Type );			
		}
		
		[Test()]
		public void TestCallPassesParameters ()
		{
			IKernel kernel = TestModule.GetTestKernel();
			Parser parser = new Parser ( kernel );
			parser.Parse ( GetBlockCallTokens () );

			new Executor ( parser.MainBlock );

			// It should give give us the correct value in standard out
			StandardOutDummy output = kernel.Get<IStandardOut> () as StandardOutDummy;
			Assert.AreEqual ( "yay", output.Text );
		}
		
		[Test()]
		public void TestBlockPassesAsParameter ()
		{
			Tokens tokens = new Tokens();
			tokens.AddTokens(new string[] {"x", "(", "{", "print", "'yay'","}",")"});
			
			IKernel kernel = TestModule.GetTestKernel();
			Parser parser = new Parser ( kernel );
			parser.Parse ( tokens );
			
			Variable variable = parser.MainBlock[0] as Variable;
			ArrayExpr array = variable.Parameters;
			
			Assert.AreEqual ( 1, array.Elements.Count, "Should only be one parameter" );
			Assert.IsInstanceOfType ( typeof(Block), array.Elements[0] );			
		}
	}
}

