using System;
using System.IdentityModel.Configuration;
using System.IdentityModel.Metadata;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Web;
using RegionOrebroLan.IdentityModel.Configuration;
using RegionOrebroLan.IdentityModel.Metadata;

namespace RegionOrebroLan.IdentityModel.Web
{
	/// <information>
	/// Found help here: Microsoft.IdentityModel.Tools.FedUtil.FederationUtilityOperations.CreateApplicationMetadata
	/// Assembly: Microsoft.IdentityModel.Tools.FedUtil, Version=3.5.0.0 
	/// C:\Windows\assembly\GAC_MSIL\Microsoft.IdentityModel.Tools.FedUtil\3.5.0.0__31bf3856ad364e35\Microsoft.IdentityModel.Tools.FedUtil.dll
	/// If you dont have the dll on your system:
	/// 1. Enable the Windows Identity Foundation 3.5 feature
	/// 2. Install Windows Identity Foundation SDK, https://www.microsoft.com/en-us/download/details.aspx?id=4451, WindowsIdentityFoundation-SDK-4.0.msi.
	/// </information>
	/// <inheritdoc />
	public class FederationMetadataHandler : IHttpHandler
	{
		#region Fields

		private EntityDescriptor _entityDescriptor;
		private const string _protocolSupported = "http://docs.oasis-open.org/wsfed/federation/200706";

		#endregion

		#region Constructors

		public FederationMetadataHandler() : this((IdentityConfigurationElementWrapper) SystemIdentityModelSection.DefaultIdentityConfigurationElement) { }

		protected internal FederationMetadataHandler(IIdentityConfiguration identityConfiguration)
		{
			this.IdentityConfiguration = identityConfiguration ?? throw new ArgumentNullException(nameof(identityConfiguration));
		}

		#endregion

		#region Properties

		protected internal virtual EntityDescriptor EntityDescriptor
		{
			get
			{
				// ReSharper disable InvertIf
				if(this._entityDescriptor == null)
				{
					var audienceUris = this.IdentityConfiguration.AudienceUris.ToArray();
					var firstAudienceUri = audienceUris.FirstOrDefault();

					var entityDescriptor = new EntityDescriptor
					{
						EntityId = new EntityId(firstAudienceUri?.AbsoluteUri)
					};

					var applicationServiceDescriptor = new ApplicationServiceDescriptor();

					foreach(var claim in this.IdentityConfiguration.RequiredClaims)
					{
						applicationServiceDescriptor.ClaimTypesRequested.Add(this.ToDisplayClaim(claim));
					}

					if(firstAudienceUri != null)
						applicationServiceDescriptor.PassiveRequestorEndpoints.Add(new EndpointReference(firstAudienceUri.AbsoluteUri));

					applicationServiceDescriptor.ProtocolsSupported.Add(new Uri(this.ProtocolSupported));

					foreach(var uri in audienceUris)
					{
						applicationServiceDescriptor.TargetScopes.Add(new EndpointReference(uri.AbsoluteUri));
					}

					entityDescriptor.RoleDescriptors.Add(applicationServiceDescriptor);

					this._entityDescriptor = entityDescriptor;
				}
				// ReSharper restore InvertIf

				return this._entityDescriptor;
			}
		}

		protected internal virtual IIdentityConfiguration IdentityConfiguration { get; }
		public virtual bool IsReusable => false;
		protected internal virtual string ProtocolSupported => _protocolSupported;

		#endregion

		#region Methods

		public virtual void ProcessRequest(HttpContext context)
		{
			if(context == null)
				throw new ArgumentNullException(nameof(context));

			this.ProcessRequestInternal(new HttpContextWrapper(context));
		}

		protected internal virtual void ProcessRequestInternal(HttpContextBase httpContext)
		{
			if(httpContext == null)
				throw new ArgumentNullException(nameof(httpContext));

			var httpResponse = httpContext.Response;

			httpResponse.ClearHeaders();

			httpResponse.Clear();
			httpResponse.ContentType = "text/xml";

			new MetadataSerializer().WriteMetadata(httpResponse.OutputStream, this.EntityDescriptor);
		}

		protected internal virtual DisplayClaim ToDisplayClaim(IDisplayClaim claim)
		{
			if(claim == null)
				return null;

			return claim is DisplayClaimWrapper displayClaimWrapper
				? displayClaimWrapper.DisplayClaim
				: new DisplayClaim(claim.ClaimType)
				{
					Description = claim.Description,
					DisplayTag = claim.DisplayTag,
					DisplayValue = claim.DisplayValue,
					Optional = claim.Optional,
					WriteOptionalAttribute = claim.WriteOptionalAttribute
				};
		}

		#endregion
	}
}