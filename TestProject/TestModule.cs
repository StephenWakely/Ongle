using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using Ongle;
using Ninject;

namespace TestProject
{
	class TestModule : NinjectModule
	{
		
		public override void Load ( )
		{
			Bind<IPrintExecutor> ().To<PrintExecutor> ().InTransientScope ();
			Bind<IAssignExecutor> ().To<AssignExecutor> ().InTransientScope ();
			Bind<IIfExecutor> ().To<IfExecutor> ().InTransientScope ();
			Bind<IVariableExecutor> ().To<VariableExecutor> ().InTransientScope ();
			Bind<IStandardOut> ().To<StandardOutDummy> ().InSingletonScope ();
		}

		public static IKernel GetTestKernel ()
		{
			return new StandardKernel ( new TestModule () );
		}

	}
}
