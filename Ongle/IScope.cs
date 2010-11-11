using System;
namespace Ongle
{
	public interface IScope
	{
		Dynamic TryGetDynamic ( string identifier );
		Dynamic GetDynamic ( string identifier );
		bool TrySetDynamic ( string identifier, Dynamic dynamic );
		void SetDynamic ( string identifier, Dynamic dynamic );
	}
}
