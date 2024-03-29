﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//-----------------------------------------------------------------------------

namespace Abc.IdentityModel.Protocols {
    using System;

	///<summary>
	/// Provides access to resource key names for the SR resource file.
	///</summary>
	[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
	[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	internal class SR {
		private static global::System.Resources.ResourceManager resourceManager;
		private static global::System.Globalization.CultureInfo resourceCulture;

		[global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal SR() {
		}

		/// <summary>
		/// Returns the cached ResourceManager instance used by this class.
		/// </summary>
		[global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		internal static global::System.Resources.ResourceManager ResourceManager {
			get {
				if (object.ReferenceEquals(resourceManager, null)) {
					global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Abc.IdentityModel.Protocols.SR", typeof(SR).Assembly);
					resourceManager = temp;
				}
				return resourceManager;
			}
		}
        
		/// <summary>
		/// Overrides the current thread's CurrentUICulture property for all
		/// resource lookups using this strongly typed resource class.
		/// </summary>
		[global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		internal static global::System.Globalization.CultureInfo Culture {
			get {
				return resourceCulture;
			}
			set {
				resourceCulture = value;
			}
		}

		/// <summary>
		/// Looks up a localized string similar to 'Submit'.
		/// </summary>
		internal static string HtmlPostNoScriptButtonText {
			get {
				return ResourceManager.GetString("HtmlPostNoScriptButtonText", resourceCulture);
			}
		}

		/// <summary>
		/// Looks up a localized string similar to 'Script is disabled. Click Submit to continue.'.
		/// </summary>
		internal static string HtmlPostNoScriptMessage {
			get {
				return ResourceManager.GetString("HtmlPostNoScriptMessage", resourceCulture);
			}
		}

		/// <summary>
		/// Looks up a localized string similar to 'position: absolute;background: white url(&apos;data:image/gif;base64,R0lGODlhGAAYAIAAAFLOQv///yH/C05FVFNDQVBFMi4wAwEAAAAh+QQFCgABACwOAAAABAAEAAACBQxgp5dRACH5BAUKAAEALBQABgAEAAQAAAIFDGCnl1EAIfkEBQoAAQAsFAAOAAQABAAAAgUMYKeXUQAh+QQFCgABACwOABQABAAEAAACBQxgp5dRACH5BAUKAAEALAYAFAAEAAQAAAIFDGCnl1EAIfkEBQoAAQAsAAAOAAQABAAAAgUMYKeXUQAh+QQFCgABACwAAAYABAAEAAACBQxgp5dRACH5BAkKAAEALAAAAAAYABgAAAIdjB8AyN2qnGRwzZss3rz7D4biSJbmiabqyrbuOxYAIfkECQoAAQAsAAAAABgAGAAAAjaMHwDI3aqcZHDNmyzevO/oGVEVVqRHgqUWtu4LU2w3Qqs5Y2n+8fH/yKkuJ5FNdzQOJ8uMowAAIfkECQoAAQAsAAAAABgAGAAAAjKMHwDI3aqcZHDNmyzevO/oUVCIVGSpnerKth4YRuZbjbScdrDLP3nGmRmEEyKx+NsdCgAh+QQJCgABACwAAAAAEgAYAAACKowfAMit2mJ6MsKKsz53v+VR2zSW5olKnfaNrQum8rkatS3i96vHyL4oAAAh+QQJCgABACwAAAAACgAYAAACIIwfAHiqnByMs84H3WL6bguGIoJxklmS38iqXmp02VIAACH5BAkKAAEALAAAAAAKABIAAAIajB8AeKqcHIyzzgfdYvpuC4YignGSWZIfUgAAIfkECQoAAQAsAAAAAAoACgAAAhGMHwB4qpwcjLPOB91i+m5WAAAh+QQJCgABACwGAAAABAAEAAACBQxgp5dRADs=&apos;) no-repeat center center;top: 0;left: 0;width: 100%;height: 100%;'.
		/// </summary>
		internal static string HtmlPostStyle {
			get {
				return ResourceManager.GetString("HtmlPostStyle", resourceCulture);
			}
		}

		/// <summary>
		/// Looks up a localized string similar to 'Working...'.
		/// </summary>
		internal static string HtmlPostTitle {
			get {
				return ResourceManager.GetString("HtmlPostTitle", resourceCulture);
			}
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID0001: The value argument is not a valid type. ClaimsProviderBase type expected.'.
		/// </summary>
		internal static string ID0001 {
			get {
				return ResourceManager.GetString("ID0001", resourceCulture);
			}
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID0002: SecurityTokenServiceConfiguration.ClaimsProviderBase is either null or not of type SecurityTokenService. Specify a valid SecurityTokenService type.'.
		/// </summary>
		internal static string ID0002 {
			get {
				return ResourceManager.GetString("ID0002", resourceCulture);
			}
		}

		/// <summary>
        /// Formats a localized string similar to 'ID0003: SecurityTokenServiceConfiguration.ClaimsProviderBase is of type &apos;{0}&apos; but is expected to be of type &apos;{1}&apos;.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID0003Format(object arg0, object arg1) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID0003", resourceCulture), arg0, arg1);
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID0004: SecurityTokenServiceConfiguration is null.'.
		/// </summary>
		internal static string ID0004 {
			get {
				return ResourceManager.GetString("ID0004", resourceCulture);
			}
		}

		/// <summary>
        /// Formats a localized string similar to 'ID0005: EncodedContext cannot be created for message types other than requests. Message type: {0}'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID0005Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID0005", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID0010: The input &apos;{0}&apos; is null.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID0010Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID0010", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID0011: The input &apos;{0}&apos; collection does not contain a property named &apos;{1}&apos;.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID0011Format(object arg0, object arg1) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID0011", resourceCulture), arg0, arg1);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID0101: The key needed to decrypt the encrypted security token could not be resolved from the following security key identifier &apos;{0}&apos;. Ensure that the SecurityTokenResolver is populated with the required key.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID0101Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID0101", resourceCulture), arg0);
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID0102: The encrypted security token was directly encrypted using an asymmetric key, which is not supported. An xenc:EncryptedKey must be used to encrypt a symmetric key using the asymmetric key.'.
		/// </summary>
		internal static string ID0102 {
			get {
				return ResourceManager.GetString("ID0102", resourceCulture);
			}
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID0103: The keyInfoSerializer must return an EncryptedKeyIdentifierClause when reading an xenc:EncryptedKey element.'.
		/// </summary>
		internal static string ID0103 {
			get {
				return ResourceManager.GetString("ID0103", resourceCulture);
			}
		}

		/// <summary>
        /// Formats a localized string similar to 'ID1006: More than one configuration element found under &apos;{0}&apos;.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID1006Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID1006", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID1007: Configuration errors. Custom configuration has unrecognized element &apos;{0}&apos; as a child of &apos;{1}&apos;.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID1007Format(object arg0, object arg1) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID1007", resourceCulture), arg0, arg1);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID1008: Configuration errors. The element &apos;{0}&apos; must contain the following attributes &apos;{1}&apos;.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID1008Format(object arg0, object arg1) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID1008", resourceCulture), arg0, arg1);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID1009: The type &apos;{0}&apos; cannot be resolved. Verify the spelling is correct or that the full type name is provided.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID1009Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID1009", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID1012: The configuration property value is not valid.\nProperty name: &apos;{0}&apos;\nError: &apos;{1}&apos;'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID1012Format(object arg0, object arg1) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID1012", resourceCulture), arg0, arg1);
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID1013: FederatedPassiveSignIn.UseFederationPropertiesFromConfiguration is true. However, a valid WSFederationAuthenticationElement could not be retrieved from configuration.'.
		/// </summary>
		internal static string ID1013 {
			get {
				return ResourceManager.GetString("ID1013", resourceCulture);
			}
		}

		/// <summary>
        /// Formats a localized string similar to 'ID1014: Cannot retrieve value for parameter: &apos;{0}&apos;.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID1014Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID1014", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID1015: The RequireHttps attribute is set to true but &apos;{0}&apos; URI scheme is not https. Change the Issuer URI scheme to https or set RequireHttps to false.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID1015Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID1015", resourceCulture), arg0);
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID1016: The EncryptedSecurityTokenHandler&apos;s ContainingCollection is set to null. EncryptedSecurityTokens cannot be written as the ContainingCollection has the SecurityTokenHandlers required to write the inner token. Set this property to a valid SecurityTokenHandlerCollection.'.
		/// </summary>
		internal static string ID1016 {
			get {
				return ResourceManager.GetString("ID1016", resourceCulture);
			}
		}

		/// <summary>
        /// Formats a localized string similar to 'ID1017: A SecurityTokenHandler is not registered to read security token (&apos;{0}&apos;, &apos;{1}&apos;).'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID1017Format(object arg0, object arg1) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID1017", resourceCulture), arg0, arg1);
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID1018: The EncryptedSecurityTokenHandler&apos;s ContainingCollection is set to null. EncryptedSecurityTokens cannot be written as the ContainingCollection has the SecurityTokenHandlers required to write the inner token. Set this property to a valid SecurityTokenHandlerCollection.'.
		/// </summary>
		internal static string ID1018 {
			get {
				return ResourceManager.GetString("ID1018", resourceCulture);
			}
		}

		/// <summary>
        /// Formats a localized string similar to 'ID1019: A SecurityTokenHandler that is registered to write a token of type &apos;{0}&apos; was not found.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID1019Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID1019", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID1020: The token cannot be validated or writed because it is not a {0}.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID1020Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID1020", resourceCulture), arg0);
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID1021: The Configuration property of this SecurityTokenHandler is set to null. Tokens cannot be read or validated in this state. Set this property or add this SecurityTokenHandler to a SecurityTokenHandlerCollection with a valid Configuration property.'.
		/// </summary>
		internal static string ID1021 {
			get {
				return ResourceManager.GetString("ID1021", resourceCulture);
			}
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID1022: Cannot detect token replays: Configuration.TokenReplayCache property is null.'.
		/// </summary>
		internal static string ID1022 {
			get {
				return ResourceManager.GetString("ID1022", resourceCulture);
			}
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID1023: The Configuration.ServiceTokenResolver property of this SecurityTokenHandler is set to null. Tokens cannot be read in this state.Set the value of Configuration.ServiceTokenResolver.'.
		/// </summary>
		internal static string ID1023 {
			get {
				return ResourceManager.GetString("ID1023", resourceCulture);
			}
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID2001: The SAML protocol message cannot be read because it contains data that is not valid.'.
		/// </summary>
		internal static string ID2001 {
			get {
				return ResourceManager.GetString("ID2001", resourceCulture);
			}
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2002: Unrecognized SAML message type: &apos;{0}&apos;.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2002Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2002", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2003: An element of an unexpected type was encountered. To support extended types in SAML protocol messages, extend the SamlProtocolSerializer. Expected type name={0} namespace={1}; Encountered type name={2} namespace={3}'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2003Format(object arg0, object arg1, object arg2, object arg3) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2003", resourceCulture), arg0, arg1, arg2, arg3);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2004: &apos;{0}&apos; is not a valid xsd:ID value.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2004Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2004", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2005: The element cannot be empty(&apos;{0}&apos;, &apos;{1}&apos;)'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2005Format(object arg0, object arg1) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2005", resourceCulture), arg0, arg1);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2006: A required attribute &apos;{0}&apos; is not present.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2006Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2006", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2007: SAML message with MajorVersion &apos;{0}&apos; and MinorVersion &apos;{1}&apos; is not supported. The supported version is MajorVersion &apos;{2}&apos; and MinorVersion &apos;{3}&apos;.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2007Format(object arg0, object arg1, object arg2, object arg3) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2007", resourceCulture), arg0, arg1, arg2, arg3);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2008: SAML message is not valid. XmlReader was expected to reference (&apos;{0}&apos;, &apos;{1}&apos;) but was referencing (&apos;{2}&apos;, &apos;{3}&apos;).'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2008Format(object arg0, object arg1, object arg2, object arg3) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2008", resourceCulture), arg0, arg1, arg2, arg3);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2009: SAML message has more than one &apos;{0}&apos; element specified.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2009Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2009", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2010: SAML message contains an unrecognized element (&apos;{0}&apos;, &apos;{1}&apos;).'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2010Format(object arg0, object arg1) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2010", resourceCulture), arg0, arg1);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2011: SAML message must contain at least one &apos;{0}&apos; specified.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2011Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2011", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2012: The given SAML query is not recognized. &apos;{0}&apos;'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2012Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2012", resourceCulture), arg0);
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID2013: SAML Request does not have any SAML Query, AssertionIDreference or AssertionArtifact  . SAML assertion must have at least one SAML Query, AssertionIDReference or AssertionArtifact.'.
		/// </summary>
		internal static string ID2013 {
			get {
				return ResourceManager.GetString("ID2013", resourceCulture);
			}
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID2014: SAML Request  have more SAML Query, AssertionIDreference or AssertionArtifact  . SAML assertion must have one SAML Query, AssertionIDReference or AssertionArtifact.'.
		/// </summary>
		internal static string ID2014 {
			get {
				return ResourceManager.GetString("ID2014", resourceCulture);
			}
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID2015: An exception occurred while parsing a SAML binding message.'.
		/// </summary>
		internal static string ID2015 {
			get {
				return ResourceManager.GetString("ID2015", resourceCulture);
			}
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID2016: Cannot create a SAML binding message from the given HttpRequest. Check if the request contains a valid Uri or Form Post that contains protocol parameters for SAML HTTP bindings.'.
		/// </summary>
		internal static string ID2016 {
			get {
				return ResourceManager.GetString("ID2016", resourceCulture);
			}
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2017: Cannot create SAML message from the given URI &apos;{0}&apos;.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2017Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2017", resourceCulture), arg0);
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID2018: The IdentityProvider collection is empty. An empty collection cannot be serialized. Add at least one IdentityProviderEntry to the collection before serialization.'.
		/// </summary>
		internal static string ID2018 {
			get {
				return ResourceManager.GetString("ID2018", resourceCulture);
			}
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2019: The RequestedAuthenticationContext cannot be serialized because the ReferenceType property is set to a value that is not valid: {0}'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2019Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2019", resourceCulture), arg0);
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID2020: The RequestedAuthenticationContext cannot be serialized because the References collection is empty. Add at least one reference to the collection before serialization.'.
		/// </summary>
		internal static string ID2020 {
			get {
				return ResourceManager.GetString("ID2020", resourceCulture);
			}
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2021: A required element is not present: {0}.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2021Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2021", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2022: The string does not meet the restriction for samlp:AuthnContextComparisonType: {0}.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2022Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2022", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2023: An unsupported SAML version was encountered: {0}.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2023Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2023", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2024: Wrong SAML message type &apos;{0}&apos;, expected &apos;{1}&apos;.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2024Format(object arg0, object arg1) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2024", resourceCulture), arg0, arg1);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2025: Expected HTTP redirect or HTTP POST SAML binding. Received: &apos;{0}&apos;'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2025Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2025", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2026: An abstract element was encountered which does not specify its concrete type.\nElement name: &apos;{0}&apos;\nElement namespace: &apos;{1}&apos;'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2026Format(object arg0, object arg1) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2026", resourceCulture), arg0, arg1);
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID2901: The protocol message cannot be read because it contains data that is not valid.'.
		/// </summary>
		internal static string ID2901 {
			get {
				return ResourceManager.GetString("ID2901", resourceCulture);
			}
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2902: A required element is not present: {0}.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2902Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2902", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2903: &apos;{0}&apos; is not a valid xsd:Any value.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2903Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2903", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2904: &apos;{0}&apos; is not a valid xsd:ID value.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2904Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2904", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2905: The element cannot be empty(&apos;{0}&apos;, &apos;{1}&apos;)'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2905Format(object arg0, object arg1) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2905", resourceCulture), arg0, arg1);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2906: A required attribute &apos;{0}&apos; is not present.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2906Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2906", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2907: An unexpected attribute &apos;{0}&apos; value was encountered. Expected value=&apos;{1}&apos;; Encountered value=&apos;{2}&apos;'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2907Format(object arg0, object arg1, object arg2) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2907", resourceCulture), arg0, arg1, arg2);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2908: Unexpected element &apos;{0}&apos; in namespace &apos;{1}&apos; found. Expected element &apos;{2}&apos; under namespace &apos;{3}&apos;.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2908Format(object arg0, object arg1, object arg2, object arg3) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2908", resourceCulture), arg0, arg1, arg2, arg3);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID2909: An error occurred while writing the Required parameter &apos;{0}&apos; is not set.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID2909Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID2909", resourceCulture), arg0);
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID3001: The AttributeStorage query parameter is null.'.
		/// </summary>
		internal static string ID3001 {
			get {
				return ResourceManager.GetString("ID3001", resourceCulture);
			}
		}

		/// <summary>
        /// Formats a localized string similar to 'ID3002: The query:&apos;{0}&apos; has at least one parameter placeholder that is not valid: {1}. Make sure there are no single quotes around the placeholder.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID3002Format(object arg0, object arg1) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID3002", resourceCulture), arg0, arg1);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID3003: The AttributeStorage query parameter &apos;{0}&apos; cannot contain &apos;;&apos;.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID3003Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID3003", resourceCulture), arg0);
		}

		/// <summary>
        /// Formats a localized string similar to 'ID3004: The AttributeStorage query:&apos;{0}&apos; has incorrect number of parameter values specified:&apos;{1}&apos;.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID3004Format(object arg0, object arg1) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID3004", resourceCulture), arg0, arg1);
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID3005: AttributeStorage query attribute is null.'.
		/// </summary>
		internal static string ID3005 {
			get {
				return ResourceManager.GetString("ID3005", resourceCulture);
			}
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID3006: At least one AttributeStorage query parameter must be specified.'.
		/// </summary>
		internal static string ID3006 {
			get {
				return ResourceManager.GetString("ID3006", resourceCulture);
			}
		}

		/// <summary>
        /// Formats a localized string similar to 'ID4001: The WS-Federation Passive session state cannot be decoded from the input value &apos;{0}&apos;.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID4001Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID4001", resourceCulture), arg0);
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID4002: An error occurred while processing the request. Contact your administrator for details.'.
		/// </summary>
		internal static string ID4002 {
			get {
				return ResourceManager.GetString("ID4002", resourceCulture);
			}
		}

		/// <summary>
        /// Formats a localized string similar to 'ID4003: The SAML authentication request element &apos;{0}&apos; is not supported.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID4003Format(object arg0) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID4003", resourceCulture), arg0);
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID4004: SAML authentication request for the WebSSO profile must specify an Issuer.'.
		/// </summary>
		internal static string ID4004 {
			get {
				return ResourceManager.GetString("ID4004", resourceCulture);
			}
		}

		/// <summary>
        /// Formats a localized string similar to 'ID4005: SAML authentication request for the WebSSO profile must specify an Issuer with Format &apos;{0}&apos;. Received Issuer Format: &apos;{1}&apos;.'.
		/// </summary>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
		internal static string ID4005Format(object arg0, object arg1) {
            return string.Format(resourceCulture, ResourceManager.GetString("ID4005", resourceCulture), arg0, arg1);
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID4006: SAML authentication request for the WebSSO profile must specify an Issuer with no NameQualifier, SPNameQualifier or SPProvidedId properties.'.
		/// </summary>
		internal static string ID4006 {
			get {
				return ResourceManager.GetString("ID4006", resourceCulture);
			}
		}

		/// <summary>
		/// Looks up a localized string similar to 'ID4007: SAML authentication request for the WebSSO profile must not specify any SubjectConfirmations.'.
		/// </summary>
		internal static string ID4007 {
			get {
				return ResourceManager.GetString("ID4007", resourceCulture);
			}
		}

	}
} 

