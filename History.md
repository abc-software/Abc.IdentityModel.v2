1. STS izsaukums

var f = new ChannelFactory<IWSTrust13Contract>(b, e);
f.Credentials.UserName.UserName = "user";
f.Credentials.UserName.Password = "pwd";
// VAI f.Credentials.Certificate.SetCertificate(...);

var serviceProxy = f.CreateChannel();

var rst = new RequestSecurityToken() {
    RequestType = "http://docs.oasis-open.org/ws-sx/ws-trust/200512/Issue",
    AppliesTo = new EndpointReference("URN:VISSDEV:DIT:WS"),
};

var request = Message.CreateMessage(MessageVersion.Soap12WSAddressing10, "http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue", new WSTrustRequestBodyWriter(rst, new WSTrustRequestSerializer(), new WSTrustSerializationContext()));
var result = serviceProxy.ProcessTrust13IssueAsync(request);
var ser = new WSTrustResponseSerializer();
var rstr = ser.ReadXml(result.GetReaderAtBodyContents(), new WSTrustSerializationContext());

2. MyClientCredentials
      public override SecurityTokenManager CreateSecurityTokenManager()
        {
            return new MyClientCredentialsSecurityTokenManager(Clone());
        }


MyClientCredentialsSecurityTokenManager
public override SecurityTokenProvider CreateSecurityTokenProvider(SecurityTokenRequirement tokenRequirement) {
	if (tokenRequirement is InitiatorServiceModelSecurityTokenRequirement initiatorRequirement) {
		retuen new SimpleSecurityTokenProvider() : SecurityTokenProvider
	}
}

public class SimpleSecurityTokenProvider : SecurityTokenProvider {
 public SimpleSecurityTokenProvider(SecurityToken token, SecurityTokenRequirement tokenRequirement)
{
	if (token == null)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("token");
	}
	GenericXmlSecurityToken genericXmlSecurityToken = token as GenericXmlSecurityToken;
	if (genericXmlSecurityToken != null)
	{
		_securityToken = WrapWithAuthPolicy(genericXmlSecurityToken, tokenRequirement);
	}
	else
	{
		_securityToken = token;
	}
}

protected override SecurityToken GetTokenCore(TimeSpan timeout)
{
	return _securityToken;
}

}