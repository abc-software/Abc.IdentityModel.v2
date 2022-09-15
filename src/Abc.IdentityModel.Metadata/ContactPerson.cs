// ----------------------------------------------------------------------------
// <copyright file="ContactPerson.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Metadata {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// The ContactPerson element specifies basic contact information about a person responsible in some
    /// capacity for a SAML entity or role. The use of this element is always optional. Its content is informative in
    /// nature and does not directly map to any core SAML elements or attributes.
    /// </summary>
    public class ContactPerson {
        private readonly Collection<Uri> emailAddresses = new Collection<Uri>();
        private readonly Collection<string> telephoneNumbers = new Collection<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPerson" /> class.
        /// </summary>
        public ContactPerson()
            : this(ContactType.Other) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPerson" /> class with the specified contact type.
        /// </summary>
        /// <param name="contactType">The contact type.</param>
        public ContactPerson(ContactType contactType) {
            this.ContactType = contactType;
        }

        /// <summary>
        /// Gets the type of contact.
        /// </summary>
        public ContactType ContactType { get; }

        /// <summary>
        /// Gets or sets the name of the company for the contact person.
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the given (first) name of the contact person.
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the surname of the contact person.
        /// </summary>
        public string SurName { get; set; }

        /// <summary>
        /// Gets the collection of mailto: URIs representing e-mail addresses belonging to the contact person.
        /// </summary>
        public ICollection<Uri> EmailAddresses => this.emailAddresses;

        /// <summary>
        /// Gets the collection of a telephone number of the contact person.
        /// </summary>
        public ICollection<string> TelephoneNumbers => this.telephoneNumbers;
    }
}