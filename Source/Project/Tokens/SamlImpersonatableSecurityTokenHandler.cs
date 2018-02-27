using System;
using System.IdentityModel.Tokens;
using System.Security.Principal;
using RegionOrebroLan.IdentityModel.Security.Principal;

namespace RegionOrebroLan.IdentityModel.Tokens
{
	public class SamlImpersonatableSecurityTokenHandler : SamlSecurityTokenHandler
	{
		#region Fields

		private static readonly IWindowsIdentityFactory _windowsIdentityFactory = new ImpersonatableWindowsIdentityFactory();

		#endregion

		#region Constructors

		public SamlImpersonatableSecurityTokenHandler() : this(_windowsIdentityFactory) { }
		public SamlImpersonatableSecurityTokenHandler(SamlSecurityTokenRequirement samlSecurityTokenRequirement) : this(samlSecurityTokenRequirement, _windowsIdentityFactory) { }

		protected internal SamlImpersonatableSecurityTokenHandler(IWindowsIdentityFactory windowsIdentityFactory)
		{
			this.WindowsIdentityFactory = windowsIdentityFactory ?? throw new ArgumentNullException(nameof(windowsIdentityFactory));
		}

		protected internal SamlImpersonatableSecurityTokenHandler(SamlSecurityTokenRequirement samlSecurityTokenRequirement, IWindowsIdentityFactory windowsIdentityFactory) : base(samlSecurityTokenRequirement)
		{
			this.WindowsIdentityFactory = windowsIdentityFactory ?? throw new ArgumentNullException(nameof(windowsIdentityFactory));
		}

		#endregion

		#region Properties

		protected internal virtual IWindowsIdentityFactory WindowsIdentityFactory { get; }

		#endregion

		#region Methods

		protected override WindowsIdentity CreateWindowsIdentity(string upn)
		{
			return this.WindowsIdentityFactory.Create("Federation", upn);
		}

		#endregion
	}
}