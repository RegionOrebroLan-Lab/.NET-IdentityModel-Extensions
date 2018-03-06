using System;

namespace RegionOrebroLan.IdentityModel.Services
{
	public interface IFederationAuthentication : IAuthentication
	{
		#region Methods

		void SignOut(Uri redirectUrl);

		#endregion
	}
}