using System;
namespace Ongle
{
	public interface ITailCallExecution : IStatement
	{
		ITailCallExecution ExecuteWithTailCall ();
	}
}

