using System;
using System.IdentityModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionOrebroLan.IdentityModel.Services;

namespace RegionOrebroLan.IdentityModel.UnitTests.Services
{
	[TestClass]
	public class FederationAuthenticationModuleWrapperTest
	{
		#region Methods

		protected internal virtual Mock<WSFederationAuthenticationModule> CreateFederationAuthenticationModuleMock()
		{
			return new Mock<WSFederationAuthenticationModule>();
		}

		[TestMethod]
		public void SignOut_ShouldCallSignOutWithIsIpRequestParameterSetToFalseOnTheWrappedInstance()
		{
			var federationAuthenticationModuleMock = this.CreateFederationAuthenticationModuleMock();
			federationAuthenticationModuleMock.Setup(federationAuthenticationModule => federationAuthenticationModule.SignOut(It.IsAny<bool>())).Verifiable();

			federationAuthenticationModuleMock.Verify(federationAuthenticationModule => federationAuthenticationModule.SignOut(It.IsAny<bool>()), Times.Never);

			new FederationAuthenticationModuleWrapper(federationAuthenticationModuleMock.Object).SignOut();

			federationAuthenticationModuleMock.Verify(federationAuthenticationModule => federationAuthenticationModule.SignOut(false), Times.Once);
		}

		[TestMethod]
		public void SignOut_WithUriParameter_IfTheUriParameterIsAnAbsoluteUri_ShouldCallSignOutWithRedirectUrlParameterSetToUriToStringOnTheWrappedInstance()
		{
			const string redirectUrlValue = "http://company.com/";
			var redirectUrl = new Uri(redirectUrlValue);

			var federationAuthenticationModuleMock = this.CreateFederationAuthenticationModuleMock();
			federationAuthenticationModuleMock.Setup(federationAuthenticationModule => federationAuthenticationModule.SignOut(It.IsAny<string>())).Verifiable();

			federationAuthenticationModuleMock.Verify(federationAuthenticationModule => federationAuthenticationModule.SignOut(It.IsAny<string>()), Times.Never);

			new FederationAuthenticationModuleWrapper(federationAuthenticationModuleMock.Object).SignOut(redirectUrl);

			federationAuthenticationModuleMock.Verify(federationAuthenticationModule => federationAuthenticationModule.SignOut(redirectUrlValue), Times.Once);
		}

		[TestMethod]
		public void SignOut_WithUriParameter_IfTheUriParameterIsARelativeUri_ShouldCallSignOutWithRedirectUrlParameterSetToUriOriginalStringOnTheWrappedInstance()
		{
			const string redirectUrlValue = "/first-part/second-part/";
			var redirectUrl = new Uri(redirectUrlValue, UriKind.RelativeOrAbsolute);

			var federationAuthenticationModuleMock = this.CreateFederationAuthenticationModuleMock();
			federationAuthenticationModuleMock.Setup(federationAuthenticationModule => federationAuthenticationModule.SignOut(It.IsAny<string>())).Verifiable();

			federationAuthenticationModuleMock.Verify(federationAuthenticationModule => federationAuthenticationModule.SignOut(It.IsAny<string>()), Times.Never);

			new FederationAuthenticationModuleWrapper(federationAuthenticationModuleMock.Object).SignOut(redirectUrl);

			federationAuthenticationModuleMock.Verify(federationAuthenticationModule => federationAuthenticationModule.SignOut(redirectUrlValue), Times.Once);
		}

		[TestMethod]
		public void SignOut_WithUriParameter_IfTheUriParameterIsNull_ShouldCallSignOutWithIsIpRequestParameterSetToFalseOnTheWrappedInstance()
		{
			var federationAuthenticationModuleMock = this.CreateFederationAuthenticationModuleMock();
			federationAuthenticationModuleMock.Setup(federationAuthenticationModule => federationAuthenticationModule.SignOut(It.IsAny<bool>())).Verifiable();

			federationAuthenticationModuleMock.Verify(federationAuthenticationModule => federationAuthenticationModule.SignOut(It.IsAny<bool>()), Times.Never);

			new FederationAuthenticationModuleWrapper(federationAuthenticationModuleMock.Object).SignOut(null);

			federationAuthenticationModuleMock.Verify(federationAuthenticationModule => federationAuthenticationModule.SignOut(false), Times.Once);
		}

		#endregion
	}
}