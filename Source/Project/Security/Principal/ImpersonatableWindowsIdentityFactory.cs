using System.Security.Principal;
using Microsoft.IdentityModel.WindowsTokenService;

namespace RegionOrebroLan.IdentityModel.Security.Principal
{
	public class ImpersonatableWindowsIdentityFactory : IWindowsIdentityFactory
	{
		#region Methods

		public virtual WindowsIdentity Create(string type, string userPrincipalName)
		{
			var windowsIdentity = S4UClient.UpnLogon(userPrincipalName);

			return new WindowsIdentity(windowsIdentity.Token, type, WindowsAccountType.Normal, true);
		}

		#endregion
	}
}