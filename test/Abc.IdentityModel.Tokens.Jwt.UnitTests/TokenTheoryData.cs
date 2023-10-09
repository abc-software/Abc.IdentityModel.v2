using Microsoft.IdentityModel.Tokens;
using System.Xml;

namespace Abc.IdentityModel.Tokens.Jwt.UnitTests {
    public class TokenTheoryData {
        public bool CanRead { get; set; }
        public string? Token { get; set; }
        public string TestId { get; set; }
        public SecurityToken? SecurityToken { get; set; }
        public TokenValidationParameters? ValidationParameters { get; set; }
        public XmlWriter? XmlWriter { get; set; }
        public XmlReader? XmlReader { get; set; }

    }
}
