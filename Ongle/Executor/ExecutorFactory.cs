
using System;
using Ninject;

namespace Ongle
{
	public class ExecutorFactory : IExecutorFactory
	{
		private IStandardOut _standardOut;
		private IDebugInfo _debugInfo;

		[Inject]
		public ExecutorFactory (IStandardOut standardOut, IDebugInfo debugInfo)
		{
			_standardOut = standardOut;
			_debugInfo = debugInfo;
		}
		
		public IPrintExecutor GetPrintExecutor()
		{
			return new PrintExecutor(_standardOut, _debugInfo);	
		}
		
		public IAssignExecutor GetAssignExecutor()
		{
			return new AssignExecutor(_debugInfo);
		}
			
		public IIfExecutor GetIfExecutor()
		{
			return new IfExecutor(_debugInfo);
		}
		
		public IVariableExecutor GetVariableExecutor()
		{
			return new VariableExecutor(_debugInfo);	
		}
		
		public IArrayExecutor GetArrayExecutor()
		{
			return new ArrayExecutor(_debugInfo);	
		}
	}
}
