using System.IdentityModel.Services;
using RegionOrebroLan.IdentityModel.Abstractions;

namespace RegionOrebroLan.IdentityModel.Services
{
	public class SessionAuthenticationModuleWrapper : Wrapper<SessionAuthenticationModule>, ISessionAuthentication
	{
		#region Constructors

		public SessionAuthenticationModuleWrapper(SessionAuthenticationModule sessionAuthenticationModule) : base(sessionAuthenticationModule, nameof(sessionAuthenticationModule)) { }

		#endregion

		#region Methods

		public virtual void SignOut()
		{
			this.WrappedInstance.SignOut();
		}

		#endregion
	}
}