// ----------------------------------------------------------------------------
// <copyright file="HttpMessagePart.cs" company="ABC Software Ltd">
//    Copyright © 2010-2019 ABC Software Ltd. All rights reserved.
//
//    This library is free software; you can redistribute it and/or.
//    modify it under the terms of the GNU Lesser General Public
//    License  as published by the Free Software Foundation, either
//    version 3 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with the library. If not, see http://www.gnu.org/licenses/.
// </copyright>
// ----------------------------------------------------------------------------

namespace Abc.IdentityModel.Http {
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>
    /// Describes an individual member of a message and assists in its serialization.
    /// </summary>
    [DebuggerDisplay("MessagePart {Name}")]
    internal class HttpMessagePart {
        private readonly PropertyInfo property;
        private readonly FieldInfo field;
        private readonly Type memberDeclaredType;
        private readonly TypeConverter typeConverter;

        internal HttpMessagePart(MemberInfo member) {
            Debug.Assert(member is FieldInfo || member is PropertyInfo, "Parameter must be field or property");

            this.field = member as FieldInfo; 
            this.property = member as PropertyInfo; 
            this.memberDeclaredType = (this.field != null) ? this.field.FieldType : this.property.PropertyType;

            TypeConverterAttribute attributeConverter = null;
            DefaultValueAttribute attributeDefault = null;

            // Find the interesting attributes in the collection 
            foreach (Attribute attribute in Attribute.GetCustomAttributes(member)) {
                if (attribute is TypeConverterAttribute) {
                    attributeConverter = (TypeConverterAttribute)attribute;
                }
                else if (attribute is MessagePartAttribute) {
                    var attributeMessagePart = (MessagePartAttribute)attribute;
                    this.Name = attributeMessagePart.Name ?? member.Name;
                    this.IsRequired = attributeMessagePart.IsRequired;
                    this.AllowEmpty = attributeMessagePart.AllowEmpty;
                }
                else if (attribute is DataMemberAttribute) {
                    var attributeDataMemeber = (DataMemberAttribute)attribute;
                    this.Name = attributeDataMemeber.Name ?? member.Name;
                    this.IsRequired = attributeDataMemeber.IsRequired;
                    this.AllowEmpty = !attributeDataMemeber.EmitDefaultValue;
                }
                else if (attribute is DescriptionAttribute) {
                    var attributeDescription = (DescriptionAttribute)attribute;
                    this.Description = attributeDescription.Description;
                }
                else if (attribute is DefaultValueAttribute) {
                    attributeDefault = (DefaultValueAttribute)attribute;
                }
 
                /*
                else if (attribute is ConfigurationValidatorAttribute) {
                    // There could be more then one validator attribute specified on a property 
                    // Currently we consider this an error since it's too late to fix it for whidbey 
                    // but the right thing to do is to introduce new validator type ( CompositeValidator ) that is a list of validators and executes  
                    // them all 

                    if (validator != null) {
                        throw new ConfigurationErrorsException(SR.GetString(SR.Validator_multiple_validator_attributes, info.Name));
                    }

                    attribValidator = (ConfigurationValidatorAttribute)attribute;
                    validator = attribValidator.ValidatorInstance;
                }
                 */
            }

            // Converter
            if (attributeConverter != null) {
                this.typeConverter = (TypeConverter)Activator.CreateInstance(Type.GetType(attributeConverter.ConverterTypeName));
            }
            else {
                this.typeConverter = TypeDescriptor.GetConverter(this.memberDeclaredType);
            }

            if (!this.typeConverter.CanConvertFrom(typeof(string)) || !this.typeConverter.CanConvertTo(typeof(string))) {
                // throw new ConfigurationErrorsException(SR.No_converter, _name, _type.Name);
            }

            // Constant
            object defaultValue = null;

            if (this.field != null && (this.field.IsInitOnly || this.field.IsLiteral || this.field.IsStatic)) {
                this.IsConstantValue = true;

                if (this.field.IsStatic) {
                    this.IsStaticConstantValue = true;
                    defaultValue = this.field.GetValue(null);
                }
            }
            else if (this.property != null && !this.property.CanWrite) {
                this.IsConstantValue = true;
            }

            // Default Value
            if (defaultValue == null && attributeDefault != null) {
                defaultValue = attributeDefault.Value;

                // If there was a default value in the prop attribute - check if we need to convert it from string 
                if (defaultValue != null && defaultValue is string && this.memberDeclaredType != typeof(string)) {
                    // Use the converter to parse this property default value 
                    try {
                        defaultValue = this.typeConverter.ConvertFromInvariantString((string)defaultValue);
                    }
                    catch (Exception ex) {
                        throw new InvalidOperationException(string.Format("The default value of the property '{0}' cannot be parsed. The error is: {1}", member.Name, ex.Message));
                    }
                }
            }

            if (defaultValue == null) {
                /*if (this.memberDeclaredType == typeof(string)) {
                    defaultValue = string.Empty;
                }
                else */if (this.memberDeclaredType.IsValueType) {
                    defaultValue = Activator.CreateInstance(this.memberDeclaredType);
                }
            }

            this.DefaultValue = defaultValue;
        }

        /// <summary>
        /// Gets the name to use when serializing or deserializing this parameter in a message.
        /// </summary>
        internal string Name { get; private set; }

        /// <summary>
        /// Gets the description to use when serializing or deserializing this parameter in a message.
        /// </summary>
        internal string Description { get; private set; }

        /// <summary>
        /// Gets the value indicating whether this message part is required for the
        /// containing message to be valid.
        /// </summary>
        internal bool IsRequired { get; private set; }

        /// <summary>
        /// Gets the value indicating whether the string value is allowed to be empty in the serialized message.
        /// </summary>
        internal bool AllowEmpty { get; private set; }

        /// <summary>
        /// Gets the value indicating whether the field or property must remain its default value.
        /// </summary>
        internal bool IsConstantValue { get; private set; }

        /// <summary>
        /// Gets the value indicating whether this part is defined as a constant field and can be read without a message instance.
        /// </summary>
        internal bool IsStaticConstantValue { get; private set; }

        /*
        /// <summary>
        /// Gets the static constant value for this message part without a message instance.
        /// </summary>
        internal string StaticConstantValue {
            get {
                Contract.Assume(this.IsStaticConstantValue);
                Contract.Assume(this.field != null);
                return this.field.GetValue(null).ToString();
            }
        }
        */    
    
        /// <summary>
        /// Gets the default value.
        /// </summary>
        internal object DefaultValue { get; private set; }

        /// <summary>
        /// Gets the normalized form of a value of a member of a given message.
        /// Used in serialization.
        /// </summary>
        /// <param name="message">The message instance to read the value from.</param>
        /// <returns>The string representation of the member's value.</returns>
        internal string GetValue(IHttpMessage message) {
            try {
                object value = GetOriginalValue(message);
                if (this.typeConverter != null && this.typeConverter.CanConvertTo(typeof(string))) {
                    return this.typeConverter.ConvertToString(value);
                }

                return value as string;
            }
            catch (FormatException ex) {
                // MessagePartWriteFailure
                throw new HttpMessageException(string.Format("Error while preparing message '{0}' parameter '{1}' for sending.", message.GetType().Name, this.Name), ex);
            }
        }

        /// <summary>
        /// Sets the member of a given message to some given value.
        /// Used in deserialization.
        /// </summary>
        /// <param name="message">The message instance containing the member whose value should be set.</param>
        /// /// <param name="value">The string representation of the value to set.</param>
        internal void SetValue(IHttpMessage message, string value) {
            try {
                if (this.IsConstantValue) {
                    string constantValue = this.GetValue(message);
                    if (!string.Equals(constantValue, value, StringComparison.OrdinalIgnoreCase)) {
                        // UnexpectedMessagePartValueForConstant
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Expected message {0} parameter '{1}' to have value '{2}' but had '{3}' instead.", message.GetType().Name, this.Name, constantValue, value));
                    }
                }
                else {
                    object val;
                    if (this.typeConverter != null && this.typeConverter.CanConvertFrom(typeof(string))) {
                        val = this.typeConverter.ConvertFromString(value);
                    }
                    else {
                        throw new InvalidCastException();
                    }

                    if (this.property != null) {
                        this.property.SetValue(message, val, null);
                    }
                    else {
                        this.field.SetValue(message, val);
                    }
                }
            }
            catch (Exception ex) {
                // MessagePartReadFailure
                throw new HttpMessageException(string.Format("Error while reading message '{0}' parameter '{1}' with value '{2}'", message.GetType().Name, this.Name, value), ex);
            }
        }

        /// <summary>
        /// Gets whether the value has been set to something other than its CLR type default value.
        /// </summary>
        /// <param name="message">The message instance to check the value on.</param>
        /// <returns>True if the value is not the CLR default value.</returns>
        internal bool IsNondefaultValueSet(IHttpMessage message) {
            var originalValue = this.GetOriginalValue(message);
            if (this.memberDeclaredType.IsValueType && Nullable.GetUnderlyingType(this.memberDeclaredType) == null) {
                return !originalValue.Equals(this.DefaultValue);
            }

            return originalValue != this.DefaultValue;
        }

        private object GetOriginalValue(IHttpMessage message) {
            object value;
            if (this.property != null) {
                value = this.property.GetValue(message, null);
            }
            else {
                value = this.field.GetValue(message);
            }

            return value;
        }
    }
}
