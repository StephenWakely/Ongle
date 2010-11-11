using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using Ongle;
using Ninject;

namespace Tests
{
	class TestModule : NinjectModule
	{
		
		public override void Load ( )
		{
			Bind<IExecutorFactory> ().To<ExecutorFactory> ();
			Bind<IBlockParser> ().To<BlockParser> ().InSingletonScope ();
			Bind<IValueParser> ().To<ValueParser> ().InSingletonScope ();
			Bind<IStandardOut> ().To<StandardOutDummy> ().InSingletonScope ();
		}

		public static IKernel GetTestKernel ()
		{
			return new StandardKernel ( new TestModule () );
		}

	}
}
