using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;

namespace Ongle
{
	public class Parser
	{
		IKernel _kernel;

		public Parser ( IKernel kernel )
		{
			MainBlock = new Block ();
			MainBlock.Scope = new Scope ();
			_kernel = kernel;
		}

		public Block MainBlock
		{
			get;
			set;
		}

		/// <summary>
		/// Start parsing the main block. 
		/// </summary>
		/// <param name="tokens">
		/// A <see cref="Tokens"/>
		/// </param>
		/// <returns>
		/// A <see cref="Block"/>
		/// </returns>
		public Block Parse ( Tokens tokens )
		{
			IBlockParser blockParser = _kernel.Get<IBlockParser> ();
			
			blockParser.Parsers.Add ( _kernel.Get<PrintParser> () );
			blockParser.Parsers.Add ( _kernel.Get<IfParser> () );
			blockParser.Parsers.Add ( _kernel.Get<AssignmentParser> () );
			blockParser.Parsers.Add ( _kernel.Get<CallParser> () );
			
			blockParser.ParseBlock ( MainBlock, tokens );
			
			return MainBlock;
		}

	}
}
