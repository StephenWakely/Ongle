using Ongle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System.IO;

namespace TestProject
{


	/// <summary>
	///This is a test class for RunOngleTest and is intended
	///to contain all RunOngleTest Unit Tests
	///</summary>
	[TestClass ()]
	public class RunOngleTest
	{


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
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for Run
		///</summary>
		[TestMethod ()]
		public void SimplePrintTest ()
		{
			IKernel kernel = TestModule.GetTestKernel ();
			RunOngle target = new RunOngle ( kernel );

			string line = 
@"x = 5
> 5";
			MemoryStream stream = new MemoryStream ( new System.Text.ASCIIEncoding ().GetBytes ( line ) );

			target.Run ( stream );

			StandardOutDummy output = kernel.Get<IStandardOut> () as StandardOutDummy;
			Assert.AreEqual ( "5", output.Text );
		}

		[TestMethod ()]
		public void SimpleIfTest ()
		{
			IKernel kernel = TestModule.GetTestKernel ();
			RunOngle target = new RunOngle ( kernel );

			string line =
@"x = 5
? x==5
> 'yay'";
			MemoryStream stream = new MemoryStream ( new System.Text.ASCIIEncoding ().GetBytes ( line ) );

			target.Run ( stream );

			StandardOutDummy output = kernel.Get<IStandardOut> () as StandardOutDummy;
			Assert.AreEqual ( "yay", output.Text );
		}

		[TestMethod ()]
		public void FailingIfTest ()
		{
			IKernel kernel = TestModule.GetTestKernel ();
			RunOngle target = new RunOngle ( kernel );

			string line =
@"x=5
?x==4
> 'yay'";
			MemoryStream stream = new MemoryStream ( new System.Text.ASCIIEncoding ().GetBytes ( line ) );

			target.Run ( stream );

			StandardOutDummy output = kernel.Get<IStandardOut> () as StandardOutDummy;
			Assert.AreEqual ( null, output.Text );
		}
	}
}
