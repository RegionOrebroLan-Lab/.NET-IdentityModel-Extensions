using System.IdentityModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionOrebroLan.IdentityModel.Services;

namespace RegionOrebroLan.IdentityModel.UnitTests.Services
{
	[TestClass]
	public class SessionAuthenticationModuleWrapperTest
	{
		#region Methods

		protected internal virtual Mock<SessionAuthenticationModule> CreateSessionAuthenticationModuleMock()
		{
			return new Mock<SessionAuthenticationModule>();
		}

		[TestMethod]
		public void SignOut_ShouldCallSignOutOnTheWrappedInstance()
		{
			var sessionAuthenticationModuleMock = this.CreateSessionAuthenticationModuleMock();
			sessionAuthenticationModuleMock.Setup(sessionAuthenticationModule => sessionAuthenticationModule.SignOut()).Verifiable();

			sessionAuthenticationModuleMock.Verify(sessionAuthenticationModule => sessionAuthenticationModule.SignOut(), Times.Never);

			new SessionAuthenticationModuleWrapper(sessionAuthenticationModuleMock.Object).SignOut();

			sessionAuthenticationModuleMock.Verify(sessionAuthenticationModule => sessionAuthenticationModule.SignOut(), Times.Once);
		}

		#endregion
	}
}