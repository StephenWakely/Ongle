using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ninject;

namespace Ongle
{
	public class RunOngle
	{
		IKernel _kernel;

		public RunOngle ( IKernel kernel )
		{
			_kernel = kernel;
		}

		public void Run ( Stream program )
		{
			Scanner scanner = new Scanner ( program );
			Parser parser = new Parser ( _kernel );
			Block block = parser.Parse( scanner.Tokens );
			block.Execute ();
		}
	}
}
