using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ongle
{
	public enum DynamicType { undefinedType, stringType, numberType, boolType, blockType, arrayType }

	public class Dynamic
	{
		private string stringValue;
		private double numberValue;
		private bool boolValue;
		private List<Dynamic> arrayValue;
		private Block blockValue = null;

		public Dynamic ()
		{
			this.Type = DynamicType.undefinedType;
		}

		public IScope Scope
		{
			get;
			set;
		}

		public DynamicType Type
		{
			get;
			set;
		}

		public string StringValue
		{
			get
			{
				if ( this.Type == DynamicType.stringType )
					return stringValue;
				else if ( this.Type == DynamicType.numberType )
					return numberValue.ToString ();
				else if ( this.Type == DynamicType.boolType )
					return boolValue.ToString ();
				else if ( this.Type == DynamicType.blockType )
					return this.blockValue.Evaluate().StringValue;

				return "<undefined>";
			}

			set
			{
				stringValue = value;
				this.Type = DynamicType.stringType;
			}
		}

		public double NumberValue
		{
			get
			{
				if ( this.Type == DynamicType.stringType )
					return double.Parse ( this.stringValue );
				else if ( this.Type == DynamicType.numberType )
					return numberValue;
				else if ( this.Type == DynamicType.boolType )
					return boolValue ? 1 : 0;
				else if ( this.Type == DynamicType.blockType )
					return this.blockValue.Evaluate ().numberValue;

				return 0;
			}

			set
			{
				numberValue = value;
				this.Type = DynamicType.numberType;
			}
		}

		public bool BoolValue
		{
			get
			{
				if ( this.Type == DynamicType.stringType )
					return this.stringValue != "";
				else if ( this.Type == DynamicType.numberType )
					return numberValue != 0;
				else if ( this.Type == DynamicType.boolType )
					return boolValue;
				else if ( this.Type == DynamicType.blockType )
					return this.blockValue.Evaluate ().BoolValue;

				return false;
			}

			set
			{
				boolValue = value;
				this.Type = DynamicType.boolType;
			}
		}

		public List<Dynamic> ArrayValue
		{
			get
			{
				if ( this.Type == DynamicType.stringType ||
				    this.Type == DynamicType.numberType ||
				    this.Type == DynamicType.boolType ||
				    this.Type == DynamicType.blockType )
				{
					var list =new List<Dynamic>();
					list.Add( this );
					return  list;
				}
				else
				{
					return this.arrayValue;	
				}

			}

			set
			{
				this.arrayValue = value;
				this.Type = DynamicType.arrayType;
			}
		}

		public Block BlockValue
		{
			get
			{
				return this.blockValue;
			}
			set
			{
				this.blockValue = value;
			}
		}
	}
}
