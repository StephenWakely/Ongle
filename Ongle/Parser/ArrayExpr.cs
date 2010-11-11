
using System;
using System.Collections.Generic;

namespace Ongle
{
	public class ArrayExpr : Expression
	{
		private IArrayExecutor _executor;
		private List<Expression> _elements = new List<Expression>();
		public ArrayExpr (IArrayExecutor executor)
		{
			_executor = executor;
		}
		
		public List<Expression> Elements
		{
			get
			{
				return _elements;
			}
		}
		
		public override Dynamic Evaluate ()
		{
			return new Dynamic 
			{
				Type = DynamicType.arrayType,
				ArrayValue = this.Elements.ConvertAll<Dynamic> (expression => expression.Evaluate())
			};
		}

	}
}
