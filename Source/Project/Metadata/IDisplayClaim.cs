using System.Diagnostics.CodeAnalysis;

namespace RegionOrebroLan.IdentityModel.Metadata
{
	public interface IDisplayClaim
	{
		#region Properties

		string ClaimType { get; }
		string Description { get; set; }
		string DisplayTag { get; set; }
		string DisplayValue { get; set; }

		[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
		bool Optional { get; set; }

		bool WriteOptionalAttribute { get; set; }

		#endregion
	}
}