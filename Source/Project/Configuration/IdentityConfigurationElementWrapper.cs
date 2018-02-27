using System;
using System.Collections.Generic;
using System.IdentityModel.Configuration;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Reflection;
using System.Xml;
using RegionOrebroLan.IdentityModel.Metadata;

namespace RegionOrebroLan.IdentityModel.Configuration
{
	public class IdentityConfigurationElementWrapper : IIdentityConfiguration
	{
		#region Constructors

		public IdentityConfigurationElementWrapper(IdentityConfigurationElement identityConfigurationElement)
		{
			this.IdentityConfigurationElement = identityConfigurationElement ?? throw new ArgumentNullException(nameof(identityConfigurationElement));
		}

		#endregion

		#region Properties

		public virtual IEnumerable<Uri> AudienceUris
		{
			get
			{
				try
				{
					return this.IdentityConfigurationElement.AudienceUris.Cast<AudienceUriElement>().Select(item => new Uri(item.Value));
				}
				catch(Exception exception)
				{
					throw new InvalidOperationException("Could not get audience-urls.", exception);
				}
			}
		}

		protected internal virtual IdentityConfigurationElement IdentityConfigurationElement { get; }

		public virtual IEnumerable<IDisplayClaim> RequiredClaims
		{
			get
			{
				try
				{
					var configurationElementInterceptor = (ConfigurationElementInterceptor) this.IdentityConfigurationElement.GetType().GetProperty("ApplicationService", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(this.IdentityConfigurationElement);

					var claimTypeRequired = configurationElementInterceptor?.ChildNodes?.Cast<XmlNode>().FirstOrDefault(item => string.Equals(item.Name, "claimTypeRequired", StringComparison.OrdinalIgnoreCase));

					var requiredClaims = new List<IDisplayClaim>();

					// ReSharper disable All
					if(claimTypeRequired != null)
					{
						foreach(var childNode in claimTypeRequired.ChildNodes.Cast<XmlNode>())
						{
							requiredClaims.Add((DisplayClaimWrapper) new DisplayClaim(childNode.Attributes["type"].Value, childNode.Attributes["name"].Value, string.Empty)
							{
								Optional = bool.Parse(childNode.Attributes["optional"]?.Value ?? bool.FalseString),
								WriteOptionalAttribute = true
							});
						}
					}
					// ReSharper restore All

					return requiredClaims.ToArray();
				}
				catch(Exception exception)
				{
					throw new InvalidOperationException("Could not get required claims.", exception);
				}
			}
		}

		#endregion

		#region Methods

		#region Implicit operator

		public static implicit operator IdentityConfigurationElementWrapper(IdentityConfigurationElement identityConfigurationElement)
		{
			return identityConfigurationElement != null ? new IdentityConfigurationElementWrapper(identityConfigurationElement) : null;
		}

		#endregion

		public static IdentityConfigurationElementWrapper ToIdentityConfigurationElementWrapper(IdentityConfigurationElement identityConfigurationElement)
		{
			return identityConfigurationElement;
		}

		#endregion
	}
}