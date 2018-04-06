# .NET-IdentityModel-Extensions

Extensions/additions for System.IdentityModel.

## Web.config example

    <?xml version="1.0" encoding="utf-8"?>
    <configuration>
        <configSections>
            <section name="system.identityModel" type="System.IdentityModel.Configuration.SystemIdentityModelSection, System.IdentityModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
            <section name="system.identityModel.services" type="System.IdentityModel.Services.Configuration.SystemIdentityModelServicesSection, System.IdentityModel.Services, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
        </configSections>
        <location path="FederationMetadata/2007-06">
            <system.web>
                <authorization>
                    <allow users="*" />
                </authorization>
            </system.web>
            <system.webServer>
                <handlers>
                    <add
                        name="FederationMetadataHandler"
                        path="FederationMetadata.xml"
                        type="RegionOrebroLan.IdentityModel.Web.FederationMetadataHandler, RegionOrebroLan.IdentityModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=520b099ae7bbdead"
                        verb="GET"
                    />
                </handlers>
            </system.webServer>
        </location>
        <system.identityModel>
            <identityConfiguration>
                <applicationService>
                    <claimTypeRequired>
                        <claimType name="UserPrincipalName" optional="false" type="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn" />
                    </claimTypeRequired>
                </applicationService>
                <audienceUris>
                    <add value="https://web.local.net/" />
                </audienceUris>
                <certificateValidation certificateValidationMode="PeerOrChainTrust" />
                <issuerNameRegistry type="System.IdentityModel.Tokens.ConfigurationBasedIssuerNameRegistry, System.IdentityModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
                    <trustedIssuers>
                        <add name="https://adfs.local.net/adfs/services/trust/" thumbprint="abcdefghijklmnopqrstuvxyzabcde0123456789" />
                    </trustedIssuers>
                </issuerNameRegistry>
                <securityTokenHandlers>
                    <remove type="System.IdentityModel.Tokens.SamlSecurityTokenHandler, System.IdentityModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
                    <add type="RegionOrebroLan.IdentityModel.Tokens.SamlImpersonatableSecurityTokenHandler, RegionOrebroLan.IdentityModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=520b099ae7bbdead">
                        <samlSecurityTokenRequirement issuerCertificateRevocationMode="Online" issuerCertificateTrustedStoreLocation="LocalMachine" issuerCertificateValidationMode="PeerOrChainTrust" mapToWindows="true">
                            <nameClaimType value="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name" />
                            <roleClaimType value="schemas.microsoft.com/ws/2006/04/identity/claims/role" />
                        </samlSecurityTokenRequirement>
                    </add>
                </securityTokenHandlers>
            </identityConfiguration>
        </system.identityModel>
        <system.identityModel.services>
            <federationConfiguration>
                <cookieHandler requireSsl="true" />
                <wsFederation issuer="https://adfs.local.net/adfs/ls/" passiveRedirectEnabled="true" realm="https://web.local.net/" requireHttps="true" />
            </federationConfiguration>
        </system.identityModel.services>
        <system.web>
            <authentication mode="Windows" />
            <authorization>
                <deny users="?" />
            </authorization>
            <compilation targetFramework="4.5" />
            <httpRuntime targetFramework="4.5" />
            <identity impersonate="true" />
        </system.web>
        <system.webServer>
            <modules>
                <remove name="WindowsAuthentication" />
                <add name="SessionAuthenticationModule" preCondition="managedHandler" type="System.IdentityModel.Services.SessionAuthenticationModule, System.IdentityModel.Services, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
                <add name="WSFederationAuthenticationModule" preCondition="managedHandler" type="System.IdentityModel.Services.WSFederationAuthenticationModule, System.IdentityModel.Services, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
            </modules>
            <validation validateIntegratedModeConfiguration="false" />
        </system.webServer>
    </configuration>