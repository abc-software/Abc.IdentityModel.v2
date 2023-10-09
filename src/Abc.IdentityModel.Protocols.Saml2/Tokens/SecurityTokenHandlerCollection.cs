using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Abc.IdentityModel.Tokens {
    public class SecurityTokenHandlerCollection : Collection<SecurityTokenHandler> {
		private Dictionary<string, SecurityTokenHandler> handlersByIdentifier = new Dictionary<string, SecurityTokenHandler>();
		private Dictionary<Type, SecurityTokenHandler> handlersByType = new Dictionary<Type, SecurityTokenHandler>();

		public SecurityTokenHandlerCollection(IEnumerable<SecurityTokenHandler> handlers) {
            if (handlers is null) {
                throw new ArgumentNullException(nameof(handlers));
            }

            foreach (var handler in handlers) {
                this.Add(handler);
            }
        }

		public SecurityTokenHandler this[string tokenTypeIdentifier] {
			get {
				if (string.IsNullOrEmpty(tokenTypeIdentifier)) {
					return null;
				}

				handlersByIdentifier.TryGetValue(tokenTypeIdentifier, out var value);
				return value;
			}
		}

		public SecurityTokenHandler this[Type tokenType] {
			get {
				if (tokenType != null && 
					handlersByType.TryGetValue(tokenType, out SecurityTokenHandler value)) {
					return value;
				}

				return null;
			}
		}

		public IEnumerable<string> TokenTypeIdentifiers => handlersByIdentifier.Keys;

        public SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor) {
            if (tokenDescriptor is null) {
                throw new ArgumentNullException(nameof(tokenDescriptor));
            }

            var handler = this[tokenDescriptor.TokenType];
			if (handler == null) {
				throw new InvalidOperationException("ID4020"/*, tokenDescriptor.TokenType*/);
			}

			return handler.CreateToken(tokenDescriptor);
		}

		/// <inheritdoc/>
		protected override void InsertItem(int index, SecurityTokenHandler item) {
			base.InsertItem(index, item);
			
			try {
				this.AddToDictionaries(item);
			}
			catch {
				base.RemoveItem(index);
				throw;
			}
		}

		/// <inheritdoc/>
		protected override void SetItem(int index, SecurityTokenHandler item) {
			var handler = base.Items[index];
			base.SetItem(index, item);
			RemoveFromDictionaries(handler);

			try {
				AddToDictionaries(item);
			}
			catch {
				base.SetItem(index, handler);
				AddToDictionaries(handler);
				throw;
			}
		}

		/// <inheritdoc/>
		protected override void RemoveItem(int index) {
			var handler = base.Items[index];
			base.RemoveItem(index);
			RemoveFromDictionaries(handler);
		}

		/// <inheritdoc/>
		protected override void ClearItems() {
			base.ClearItems();
			handlersByIdentifier.Clear();
			handlersByType.Clear();
		}

        private void AddToDictionaries(SecurityTokenHandler handler) {
            if (handler is null) {
                throw new ArgumentNullException(nameof(handler));
            }

            bool addedIdentifier = false;

			/*
			var identifiers = handler.GetTokenTypeIdentifiers();
			if (identifiers != null) {
				foreach (string identifier in identifiers) {
					if (identifier != null) {
						handlersByIdentifier.Add(identifier, handler);
						addedIdentifier = true;
					}
				}
			}
			*/

			var tokenType = handler.TokenType;
            if (tokenType != null) {
                try {
                    handlersByType.Add(tokenType, handler);
                }
                catch {
                    if (addedIdentifier) {
                        RemoveFromDictionaries(handler);
                    }

                    throw;
                }
            }
        }

		private void RemoveFromDictionaries(SecurityTokenHandler handler) {
			/*
			var identifiers = handler.GetTokenTypeIdentifiers();
			if (identifiers != null) {
				foreach (string identifier in identifiers) {
					if (identifier != null) {
						handlersByIdentifier.Remove(identifier);
					}
				}
			}
			*/

            var tokenType = handler.TokenType;
            if (tokenType != null && handlersByType.ContainsKey(tokenType)) {
                handlersByType.Remove(tokenType);
            }
        }
	}
}
