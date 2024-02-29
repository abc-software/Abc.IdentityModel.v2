namespace Abc.IdentityModel.EidasLight.UnitTests {
    using System;
    using System.Xml;

    public class TestXmlResolver : XmlUrlResolver {
        public override Uri ResolveUri(Uri? baseUri, string? relativeUri) {
            if (relativeUri != null) {
                Uri uri = new Uri(relativeUri, UriKind.RelativeOrAbsolute);
                if (uri.IsAbsoluteUri && uri.Scheme == Uri.UriSchemeHttp) {
                    var u = new Uri(baseUri, System.IO.Path.GetFileName(relativeUri));
                    return u;
                }
            }

            return base.ResolveUri(baseUri, relativeUri);
        }

        public override object? GetEntity(Uri absoluteUri, string? role, Type? ofObjectToReturn) {
            var x = base.GetEntity(absoluteUri, role, ofObjectToReturn);
            return x;
        }
    }
}
