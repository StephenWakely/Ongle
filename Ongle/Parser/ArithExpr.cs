
using System;

namespace Ongle
{
	// <arith_expr> := <expr> <arith_op> <expr>
	public class ArithExpr : Expression
	{
		private Expression _left;
		private Expression _right;

		public Expression Left
		{
			get
			{
				return _left;	
			}
			set	
			{
				_left = value;
				_left.Scope = this.Scope;
			}
		}
		
		public Expression Right
		{
			get
			{
				return _right;
			}
			set	
			{
				_right = value;
				_right.Scope = this.Scope;
			}
		}

		public ArithOp Op;

		public override Dynamic Evaluate ()
		{
			Dynamic left = Left.Evaluate ();
			Dynamic right = Right.Evaluate ();

			if ( Op == ArithOp.Equality )
			{
				return new Dynamic ()
				{
					BoolValue = left.StringValue == right.StringValue,
					Scope = this.Scope
				};
			}
			else if ( Op == ArithOp.LessThan )
			{
				return new Dynamic ()
				{
					BoolValue = left.NumberValue < right.NumberValue,
					Scope = this.Scope
				};
			}
			else if ( Op == ArithOp.GreaterThan )
			{
				return new Dynamic ()
				{
					BoolValue = left.NumberValue > right.NumberValue,
					Scope = this.Scope
				};
			}

			if ( left.Type == DynamicType.numberType && right.Type == DynamicType.numberType )
			{
				if ( Op == ArithOp.Add )
					return EvaluateAdd ( left, right );
				if ( Op == ArithOp.Div )
					return EvaluateDiv ( left, right );
				if ( Op == ArithOp.Mul )
					return EvaluateMul ( left, right );
				if ( Op == ArithOp.Sub )
					return EvaluateSub ( left, right );
			}

			return new Dynamic ( )
			{
				StringValue = left.StringValue + right.StringValue,
				Scope = this.Scope
			};
		}

		private Dynamic EvaluateAdd ( Dynamic left, Dynamic right )
		{
			return new Dynamic ( )
			{
				NumberValue = left.NumberValue + right.NumberValue,
				Scope = this.Scope
			};
		}

		private Dynamic EvaluateSub ( Dynamic left, Dynamic right )
		{
			return new Dynamic ( )
			{
				NumberValue = left.NumberValue - right.NumberValue,
				Scope = this.Scope 
			};
		}

		private Dynamic EvaluateMul ( Dynamic left, Dynamic right )
		{
			return new Dynamic ( )
			{
				NumberValue = left.NumberValue * right.NumberValue,
				Scope = this.Scope
			};
		}

		private Dynamic EvaluateDiv ( Dynamic left, Dynamic right )
		{
			return new Dynamic ( )
			{
				NumberValue = left.NumberValue / right.NumberValue,
				Scope = this.Scope
			};
		}

	}

	// <arith_op> := + | - | * | /
	public enum ArithOp
	{
		none,
		Add,
		Sub,
		Mul,
		Div,
		Equality,
		LessThan,
		GreaterThan
	}
}
