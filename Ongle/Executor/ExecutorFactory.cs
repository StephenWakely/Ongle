
using System;
using Ninject;

namespace Ongle
{
	public class ExecutorFactory : IExecutorFactory
	{
		private IStandardOut _standardOut;

		[Inject]
		public ExecutorFactory (IStandardOut standardOut)
		{
			_standardOut = standardOut;
		}
		
		public IPrintExecutor GetPrintExecutor()
		{
			return new PrintExecutor(_standardOut);	
		}
		
		public IAssignExecutor GetAssignExecutor()
		{
			return new AssignExecutor();
		}
			
		public IIfExecutor GetIfExecutor()
		{
			return new IfExecutor();	
		}
		
		public IVariableExecutor GetVariableExecutor()
		{
			return new VariableExecutor();	
		}
		
		public IArrayExecutor GetArrayExecutor()
		{
			return new ArrayExecutor();	
		}
	}
}
