using System.IdentityModel.Tokens;
using System.Security.Principal;
using Microsoft.IdentityModel.WindowsTokenService;

namespace RegionOrebroLan.IdentityModel.Tokens
{
	public class SamlImpersonatableSecurityTokenHandler : SamlSecurityTokenHandler
	{
		#region Constructors

		public SamlImpersonatableSecurityTokenHandler() { }
		public SamlImpersonatableSecurityTokenHandler(SamlSecurityTokenRequirement samlSecurityTokenRequirement) : base(samlSecurityTokenRequirement) { }

		#endregion

		#region Methods

		protected override WindowsIdentity CreateWindowsIdentity(string upn)
		{
			var windowsIdentity = S4UClient.UpnLogon(upn);

			return new WindowsIdentity(windowsIdentity.Token, "Federation", WindowsAccountType.Normal, true);
		}

		#endregion
	}
}