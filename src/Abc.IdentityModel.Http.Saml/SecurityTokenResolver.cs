#if AZUREAD
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Abc.IdentityModel.Saml.Http {
    public abstract class SecurityTokenResolver {
        public SecurityToken ResolveToken(SecurityKeyIdentifierClause keyIdentifierClause) {
            if (keyIdentifierClause is null) {
                throw new ArgumentNullException(nameof(keyIdentifierClause));
            }

            if (!TryResolveTokenCore(keyIdentifierClause, out var token)) {
                throw new InvalidOperationException("UnableToResolveTokenReference");
            }

            return token;
        }

        public bool TryResolveToken(SecurityKeyIdentifierClause keyIdentifierClause, out SecurityToken token) {
            if (keyIdentifierClause is null) {
                throw new ArgumentNullException(nameof(keyIdentifierClause));
            }

            return TryResolveTokenCore(keyIdentifierClause, out token);
        }

        protected abstract bool TryResolveTokenCore(SecurityKeyIdentifierClause keyIdentifierClause, out SecurityToken token);

        public SecurityKey ResolveSecurityKey(SecurityKeyIdentifierClause keyIdentifierClause) {
            if (keyIdentifierClause is null) {
                throw new ArgumentNullException(nameof(keyIdentifierClause));
            }

            if (!TryResolveSecurityKeyCore(keyIdentifierClause, out var key)) {
                throw new InvalidOperationException("UnableToResolveKeyReference");
            }

            return key;
        }

        public bool TryResolveSecurityKey(SecurityKeyIdentifierClause keyIdentifierClause, out SecurityKey key) {
            if (keyIdentifierClause is null) {
                throw new ArgumentNullException(nameof(keyIdentifierClause));
            }

            return TryResolveSecurityKeyCore(keyIdentifierClause, out key);
		}

        protected abstract bool TryResolveSecurityKeyCore(SecurityKeyIdentifierClause keyIdentifierClause, out SecurityKey key);

        public static SecurityTokenResolver CreateDefaultSecurityTokenResolver(ReadOnlyCollection<SecurityToken> tokens) {
            return new SimpleTokenResolver(tokens);
        }

        private class SimpleTokenResolver : SecurityTokenResolver {
            private readonly ReadOnlyCollection<SecurityToken> tokens;

            public SimpleTokenResolver(ReadOnlyCollection<SecurityToken> tokens) {
                this.tokens = tokens ?? throw new ArgumentNullException(nameof(tokens));
            }

            protected override bool TryResolveSecurityKeyCore(SecurityKeyIdentifierClause keyIdentifierClause, out SecurityKey key) {
                if (keyIdentifierClause is null) {
                    throw new ArgumentNullException(nameof(keyIdentifierClause));
                }

                key = null;
                for (int i = 0; i < tokens.Count; i++) {
                    var securityKey = tokens[i].ResolveKeyIdentifierClause(keyIdentifierClause);
                    if (securityKey != null) {
                        key = securityKey;
                        return true;
                    }
                }

                return key != null;
            }

            protected override bool TryResolveTokenCore(SecurityKeyIdentifierClause keyIdentifierClause, out SecurityToken token) {
                if (keyIdentifierClause is null) {
                    throw new ArgumentNullException(nameof(keyIdentifierClause));
                }

                token = null;
                var securityToken = ResolveSecurityToken(keyIdentifierClause);
                if (securityToken != null) {
                    token = securityToken;
                }

                return token != null;
            }

            private SecurityToken ResolveSecurityToken(SecurityKeyIdentifierClause keyIdentifierClause) {
                if (keyIdentifierClause is null) {
                    throw new ArgumentNullException(nameof(keyIdentifierClause));
                }

                for (int i = 0; i < tokens.Count; i++) {
                    if (tokens[i].MatchesKeyIdentifierClause(keyIdentifierClause)) {
                        return tokens[i];
                    }
                }

                return null;
            }
        }
    }
}

#endif