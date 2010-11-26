using System;
namespace Ongle
{
	public class DebugInfo : IDebugInfo
	{
		public bool PrintToConsole {get; set;}

		public DebugInfo ()
		{
		}
		
		public void PrintDebugInfo ( string message )
		{
			if ( PrintToConsole )
				Console.WriteLine ( message );	
		}		
	}
}

