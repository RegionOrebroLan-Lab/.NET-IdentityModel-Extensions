using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.IO;
using System.Text;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionOrebroLan.IdentityModel.Configuration;
using RegionOrebroLan.IdentityModel.Metadata;
using RegionOrebroLan.IdentityModel.Web;

namespace RegionOrebroLan.IdentityModel.UnitTests.Web
{
	[TestClass]
	public class FederationMetadataHandlerTest
	{
		#region Methods

		protected internal virtual IIdentityConfiguration CreateDefaultIdentityConfiguration()
		{
			return this.CreateIdentityConfiguration(new[] {new Uri("http://localhost/"),}, null);
		}

		protected internal virtual IIdentityConfiguration CreateEmptyIdentityConfiguration()
		{
			return this.CreateIdentityConfiguration(null, null);
		}

		protected internal virtual HttpContextBase CreateHttpContext()
		{
			return this.CreateHttpContextMock().Object;
		}

		protected internal virtual Mock<HttpContextBase> CreateHttpContextMock()
		{
			return this.CreateHttpContextMock(Mock.Of<Stream>());
		}

		protected internal virtual Mock<HttpContextBase> CreateHttpContextMock(Stream stream)
		{
			return this.CreateHttpContextMock(this.CreateHttpResponseMock(stream).Object);
		}

		protected internal virtual Mock<HttpContextBase> CreateHttpContextMock(HttpResponseBase httpResponse)
		{
			var httpContextMock = new Mock<HttpContextBase>();
			httpContextMock.Setup(httpContext => httpContext.Response).Returns(httpResponse);

			return httpContextMock;
		}

		protected internal virtual Mock<HttpContextBase> CreateHttpContextMock(Mock<HttpResponseBase> httpResponseMock)
		{
			return this.CreateHttpContextMock(httpResponseMock.Object);
		}

		protected internal virtual Mock<HttpResponseBase> CreateHttpResponseMock()
		{
			return this.CreateHttpResponseMock(Mock.Of<Stream>());
		}

		protected internal virtual Mock<HttpResponseBase> CreateHttpResponseMock(Stream stream)
		{
			var httpResponseMock = new Mock<HttpResponseBase>().SetupAllProperties();

			httpResponseMock.Setup(httpResponse => httpResponse.OutputStream).Returns(stream);

			return httpResponseMock;
		}

		protected internal virtual IIdentityConfiguration CreateIdentityConfiguration(Uri audienceUri)
		{
			return this.CreateIdentityConfiguration(new[] {audienceUri}, null);
		}

		protected internal virtual IIdentityConfiguration CreateIdentityConfiguration(IEnumerable<Uri> audienceUris, IEnumerable<IDisplayClaim> requiredClaims)
		{
			return this.CreateIdentityConfigurationMock(audienceUris, requiredClaims).Object;
		}

		protected internal virtual Mock<IIdentityConfiguration> CreateIdentityConfigurationMock(IEnumerable<Uri> audienceUris, IEnumerable<IDisplayClaim> requiredClaims)
		{
			var identityConfigurationMock = new Mock<IIdentityConfiguration>().SetupAllProperties();

			if(audienceUris != null)
				identityConfigurationMock.Setup(identityConfiguration => identityConfiguration.AudienceUris).Returns(audienceUris);

			if(requiredClaims != null)
				identityConfigurationMock.Setup(identityConfiguration => identityConfiguration.RequiredClaims).Returns(requiredClaims);

			return identityConfigurationMock;
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ProcessRequest_IfTheContextParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			try
			{
				// ReSharper disable AssignNullToNotNullAttribute
				new FederationMetadataHandler(Mock.Of<IIdentityConfiguration>()).ProcessRequest(null);
				// ReSharper restore AssignNullToNotNullAttribute
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(string.Equals(argumentNullException.ParamName, "context", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ProcessRequestInternal_IfTheHttpContextParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			try
			{
				// ReSharper disable AssignNullToNotNullAttribute
				new FederationMetadataHandler(Mock.Of<IIdentityConfiguration>()).ProcessRequestInternal(null);
				// ReSharper restore AssignNullToNotNullAttribute
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(string.Equals(argumentNullException.ParamName, "httpContext", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(MetadataSerializationException))]
		public void ProcessRequestInternal_IfThereAreNoAudienceUris_ShouldThrowAMetadataSerializationException()
		{
			new FederationMetadataHandler(this.CreateEmptyIdentityConfiguration()).ProcessRequestInternal(this.CreateHttpContext());
		}

		[TestMethod]
		public void ProcessRequestInternal_ShouldClearTheResponse()
		{
			var httpResponseMock = this.CreateHttpResponseMock();

			httpResponseMock.Verify(httpResponse => httpResponse.Clear(), Times.Never);

			new FederationMetadataHandler(this.CreateDefaultIdentityConfiguration()).ProcessRequestInternal(this.CreateHttpContextMock(httpResponseMock).Object);

			httpResponseMock.Verify(httpResponse => httpResponse.Clear(), Times.Once);
		}

		[TestMethod]
		public void ProcessRequestInternal_ShouldClearTheResponseHeaders()
		{
			var httpResponseMock = this.CreateHttpResponseMock();

			httpResponseMock.Verify(httpResponse => httpResponse.ClearHeaders(), Times.Never);

			new FederationMetadataHandler(this.CreateDefaultIdentityConfiguration()).ProcessRequestInternal(this.CreateHttpContextMock(httpResponseMock).Object);

			httpResponseMock.Verify(httpResponse => httpResponse.ClearHeaders(), Times.Once);
		}

		[TestMethod]
		public void ProcessRequestInternal_ShouldSetTheResponseContentTypeToTextXml()
		{
			var httpResponseMock = this.CreateHttpResponseMock();

			Assert.IsNull(httpResponseMock.Object.ContentType);

			new FederationMetadataHandler(this.CreateDefaultIdentityConfiguration()).ProcessRequestInternal(this.CreateHttpContextMock(httpResponseMock).Object);

			Assert.AreEqual("text/xml", httpResponseMock.Object.ContentType);
		}

		[TestMethod]
		public void ProcessRequestInternal_ShouldWriteToTheResponseOutputStream()
		{
			using(var stream = new MemoryStream())
			{
				var httpResponseMock = this.CreateHttpResponseMock(stream);

				new FederationMetadataHandler(this.CreateDefaultIdentityConfiguration()).ProcessRequestInternal(this.CreateHttpContextMock(httpResponseMock).Object);

				var content = Encoding.UTF8.GetString(stream.ToArray());

				Assert.IsTrue(content.StartsWith("<EntityDescriptor ID=\"", StringComparison.Ordinal));
				Assert.IsTrue(content.EndsWith("\" entityID=\"http://localhost/\" xmlns=\"urn:oasis:names:tc:SAML:2.0:metadata\"><RoleDescriptor xsi:type=\"fed:ApplicationServiceType\" protocolSupportEnumeration=\"http://docs.oasis-open.org/wsfed/federation/200706\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:fed=\"http://docs.oasis-open.org/wsfed/federation/200706\"><fed:TargetScopes><wsa:EndpointReference xmlns:wsa=\"http://www.w3.org/2005/08/addressing\"><wsa:Address>http://localhost/</wsa:Address></wsa:EndpointReference></fed:TargetScopes><fed:PassiveRequestorEndpoint><wsa:EndpointReference xmlns:wsa=\"http://www.w3.org/2005/08/addressing\"><wsa:Address>http://localhost/</wsa:Address></wsa:EndpointReference></fed:PassiveRequestorEndpoint></RoleDescriptor></EntityDescriptor>", StringComparison.Ordinal));
			}
		}

		#endregion
	}
}