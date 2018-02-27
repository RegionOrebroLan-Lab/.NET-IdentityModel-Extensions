using System;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Metadata;

namespace RegionOrebroLan.IdentityModel.Metadata
{
	public class DisplayClaimWrapper : IDisplayClaim
	{
		#region Constructors

		public DisplayClaimWrapper(DisplayClaim displayClaim)
		{
			this.DisplayClaim = displayClaim ?? throw new ArgumentNullException(nameof(displayClaim));
		}

		#endregion

		#region Properties

		public virtual string ClaimType => this.DisplayClaim.ClaimType;

		public virtual string Description
		{
			get => this.DisplayClaim.Description;
			set => this.DisplayClaim.Description = value;
		}

		protected internal virtual DisplayClaim DisplayClaim { get; }

		public virtual string DisplayTag
		{
			get => this.DisplayClaim.DisplayTag;
			set => this.DisplayClaim.DisplayTag = value;
		}

		public virtual string DisplayValue
		{
			get => this.DisplayClaim.DisplayValue;
			set => this.DisplayClaim.DisplayValue = value;
		}

		[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
		public virtual bool Optional
		{
			get => this.DisplayClaim.Optional;
			set => this.DisplayClaim.Optional = value;
		}

		public virtual bool WriteOptionalAttribute
		{
			get => this.DisplayClaim.WriteOptionalAttribute;
			set => this.DisplayClaim.WriteOptionalAttribute = value;
		}

		#endregion

		#region Methods

		#region Implicit operator

		public static implicit operator DisplayClaimWrapper(DisplayClaim displayClaim)
		{
			return displayClaim != null ? new DisplayClaimWrapper(displayClaim) : null;
		}

		#endregion

		public static DisplayClaimWrapper ToDisplayClaimWrapper(DisplayClaim displayClaim)
		{
			return displayClaim;
		}

		#endregion
	}
}