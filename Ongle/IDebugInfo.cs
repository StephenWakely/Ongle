using System;
namespace Ongle
{
	public interface IDebugInfo
	{
		bool PrintToConsole {get; set;}
		void PrintDebugInfo ( string message );
	}
}

