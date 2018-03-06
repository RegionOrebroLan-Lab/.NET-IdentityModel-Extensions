using System.DirectoryServices.AccountManagement;
using System.Reflection;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.IdentityModel.Tokens;

namespace RegionOrebroLan.IdentityModel.IntegrationTests.Token
{
	[TestClass]
	public class SamlImpersonatableSecurityTokenHandlerTest
	{
		#region Methods

		protected internal virtual WindowsIdentity CreateWindowsIdentity(string userPrincipalName)
		{
			var createWindowsIdentityMethod = typeof(SamlImpersonatableSecurityTokenHandler).GetMethod("CreateWindowsIdentity", BindingFlags.Instance | BindingFlags.NonPublic);

			// ReSharper disable PossibleNullReferenceException
			return (WindowsIdentity) createWindowsIdentityMethod.Invoke(new SamlImpersonatableSecurityTokenHandler(), new object[] {userPrincipalName});
			// ReSharper restore PossibleNullReferenceException
		}

		[TestMethod]
		public void CreateWindowsIdentity_ShouldReturnAnImpersonatableWindowsIdentity()
		{
			using(var userPrincipal = UserPrincipal.Current)
			{
				using(var windowsIdentity = this.CreateWindowsIdentity(userPrincipal.UserPrincipalName))
				{
					Assert.AreEqual(TokenImpersonationLevel.Impersonation, windowsIdentity.ImpersonationLevel);
				}
			}
		}

		#endregion
	}
}