using System.Security.Principal;

namespace RegionOrebroLan.IdentityModel.Security.Principal
{
	public interface IWindowsIdentityFactory
	{
		#region Methods

		WindowsIdentity Create(string type, string userPrincipalName);

		#endregion
	}
}