
using System;

namespace Ongle
{
	public interface IExecutorFactory
	{
		IPrintExecutor GetPrintExecutor();		
		IAssignExecutor GetAssignExecutor();
		IArrayExecutor GetArrayExecutor();
		IIfExecutor GetIfExecutor();
		IVariableExecutor GetVariableExecutor();
	}
}
