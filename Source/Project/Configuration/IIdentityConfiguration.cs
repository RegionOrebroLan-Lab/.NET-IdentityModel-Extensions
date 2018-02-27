using System;
using System.Collections.Generic;
using RegionOrebroLan.IdentityModel.Metadata;

namespace RegionOrebroLan.IdentityModel.Configuration
{
	public interface IIdentityConfiguration
	{
		#region Properties

		IEnumerable<Uri> AudienceUris { get; }
		IEnumerable<IDisplayClaim> RequiredClaims { get; }

		#endregion
	}
}