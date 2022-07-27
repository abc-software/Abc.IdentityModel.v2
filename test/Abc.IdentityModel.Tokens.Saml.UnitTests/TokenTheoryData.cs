using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Abc.IdentityModel.Tokens.Saml.UnitTests {
    public class TokenTheoryData : TheoryDataBase {
        public TokenTheoryData() {

        }

        public TokenTheoryData(TokenTheoryData other) {
            Actor = other.Actor;
            ActorTokenValidationParameters = other.ActorTokenValidationParameters;
            Audiences = other.Audiences;
            CanRead = other.CanRead;
            ExpectedException = other.ExpectedException;
            //First = other.First;
            Issuer = other.Issuer;
            MemoryStream = other.MemoryStream;
            SecurityToken = other.SecurityToken;
            SigningCredentials = other.SigningCredentials;
            TestId = other.TestId;
            Token = other.Token;
            TokenDescriptor = other.TokenDescriptor;
            ValidationParameters = other.ValidationParameters;
            XmlWriter = other.XmlWriter;
        }

        public Exception ExpectedException { get; set; }

        public string Actor { get; set; }

        public TokenValidationParameters ActorTokenValidationParameters { get; set; }

        public IEnumerable<string> Audiences { get; set; }

        public bool CanRead { get; set; }

        public string Issuer { get; set; }

        public MemoryStream MemoryStream { get; set; }

        public SecurityToken SecurityToken { get; set; }

        public SigningCredentials SigningCredentials { get; set; }

        public string Token { get; set; }

        public SecurityTokenDescriptor TokenDescriptor { get; set; }

        public TokenValidationParameters ValidationParameters { get; set; }

        public XmlReader XmlReader { get; set; }

        public XmlWriter XmlWriter { get; set; }

        public override string ToString() {
            return $"{TestId}, {ExpectedException}";
        }
    }
}
