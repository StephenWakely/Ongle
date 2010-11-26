using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ninject;

namespace Ongle
{
	class Program
	{
		static void Main ( string[] args )
		{
			string scriptFile = "";
			bool debug = false;
			
			foreach ( string arg in args )
			{
				if (arg == "-debug" || arg == "-d")
				{
					Console.WriteLine ( "debuggin" );
					debug = true;
				}
				else
					scriptFile = arg;			
			}
			
			if (scriptFile == "")
				Console.WriteLine ( "Specify the script file to run" );
			
			IKernel kernel = RunModule.GetKernel();
			kernel.Get <IDebugInfo> ().PrintToConsole = debug;

			RunOngle ongle = new RunOngle (kernel);
			FileStream stream = new FileStream ( args[0], FileMode.Open);
			ongle.Run ( stream );
		}
	}
}
