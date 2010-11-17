using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Ongle
{
	public class Scanner
	{
		public Tokens Tokens
		{
			get;
			set;
		}

		public Scanner ( Stream input )
		{
			this.Tokens = new Tokens();
				
			StreamReader reader = new StreamReader(input);

			Regex tokenizer = new Regex(@"'.*'|\*|\+|==|=|\$|-|/|\?|<|>|{|}|\[|\]|\(|\)|,|\b\w*\b");
			var matches = tokenizer.Matches ( reader.ReadToEnd() ) ;
			foreach ( Match match in matches )
				Tokens.AddToken ( match.Value );			
		}
	}
}
