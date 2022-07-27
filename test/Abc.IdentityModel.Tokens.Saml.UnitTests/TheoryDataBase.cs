using Microsoft.IdentityModel.Logging;

namespace Abc.IdentityModel.Tokens.Saml.UnitTests {
    public class TheoryDataBase {
        public TheoryDataBase() {
            IdentityModelEventSource.ShowPII = true;
        }

        public TheoryDataBase(bool showPII) {
            IdentityModelEventSource.ShowPII = showPII;
        }

        public string TestId { get; set; }
    }
}
