using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ongle
{
	class Program
	{
		static void Main ( string[] args )
		{
			if ( args.Length != 1 )
			{
				Console.WriteLine ( "Specify the script file to run" );
				return;
			}

			RunOngle ongle = new RunOngle (RunModule.GetKernel());
			FileStream stream = new FileStream ( args[0], FileMode.Open);
			ongle.Run ( stream );
		}
	}
}
