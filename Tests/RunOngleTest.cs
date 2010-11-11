using Ongle;
using NUnit.Framework;
using Ninject;
using System.IO;

namespace Tests
{


	/// <summary>
	///This is a test class for RunOngleTest and is intended
	///to contain all RunOngleTest Unit Tests
	///</summary>
	[TestFixture()]
	public class RunOngleTest
	{

		/// <summary>
		///A test for Run
		///</summary>
		[Test ()]
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

		[Test ()]
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

		[Test ()]
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
