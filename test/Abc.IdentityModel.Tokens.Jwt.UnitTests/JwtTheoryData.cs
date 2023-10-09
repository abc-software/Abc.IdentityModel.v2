namespace Abc.IdentityModel.Tokens.Jwt.UnitTests {
    public class JwtTheoryData : TokenTheoryData
    {
        public JwtSecurityTokenHandler Handler { get; set; } = new JwtSecurityTokenHandler();
        public ArgumentException ExpectedException { get; set; }    

        public MemoryStream MemoryStream { get; set; }  
    }
}
