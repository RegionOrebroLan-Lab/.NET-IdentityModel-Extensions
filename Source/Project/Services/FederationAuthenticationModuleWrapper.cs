using System;
using System.IdentityModel.Services;
using RegionOrebroLan.IdentityModel.Abstractions;

namespace RegionOrebroLan.IdentityModel.Services
{
	public class FederationAuthenticationModuleWrapper : Wrapper<WSFederationAuthenticationModule>, IFederationAuthentication
	{
		#region Constructors

		public FederationAuthenticationModuleWrapper(WSFederationAuthenticationModule federationAuthenticationModule) : base(federationAuthenticationModule, nameof(federationAuthenticationModule)) { }

		#endregion

		#region Methods

		public virtual void SignOut()
		{
			this.WrappedInstance.SignOut(false);
		}

		public virtual void SignOut(Uri redirectUrl)
		{
			if(redirectUrl == null)
				this.WrappedInstance.SignOut(false);
			else
				this.WrappedInstance.SignOut(redirectUrl.IsAbsoluteUri ? redirectUrl.ToString() : redirectUrl.OriginalString);
		}

		#endregion
	}
}