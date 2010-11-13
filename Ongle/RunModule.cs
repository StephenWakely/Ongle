using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using Ninject;

namespace Ongle
{
	class RunModule : NinjectModule
	{
		public override void Load ()
		{
			Bind<IExecutorFactory> ().To<ExecutorFactory> ().InSingletonScope ();
			Bind<IBlockParser> ().To<BlockParser> ().InSingletonScope ();
			Bind<IExpressionParser> ().To<ExpressionParser> ().InSingletonScope ();
			Bind<IStandardOut> ().To<StandardOut> ().InSingletonScope ();			
		}

		public static IKernel GetKernel ()
		{
			return new StandardKernel ( new RunModule () );
		}


	}
}
