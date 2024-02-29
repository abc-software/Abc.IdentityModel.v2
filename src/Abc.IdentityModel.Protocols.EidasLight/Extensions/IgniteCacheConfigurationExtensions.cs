// ----------------------------------------------------------------------------
// <copyright file="IgniteCacheConfigurationExtensions.cs" company="ABC software Ltd">
//    Copyright © ABC SOFTWARE. All rights reserved.
//
//    Licensed under the Apache License, Version 2.0.
//    See LICENSE in the project root for license information.
// </copyright>
// ----------------------------------------------------------------------------

using Apache.Ignite.Core.Client;
#if NETSTANDARD || NETCOREAPP2_0_OR_GREATER
using Microsoft.Extensions.Logging;
#endif
using System;

namespace Abc.IdentityModel.EidasLight.Ignite {
    public static class IgniteCacheConfigurationExtension {
        public static IgniteClientConfiguration GetIgniteClientConfiguration(
            this IgniteCacheConfiguration configuration
#if NETSTANDARD || NETCOREAPP2_0_OR_GREATER
            ,
                ILoggerFactory loggerFacrtory
#endif
            ) {
            var ignateClientConfiguration = new IgniteClientConfiguration {
                Endpoints = configuration.Endpoints,
                EnablePartitionAwareness = false, // NET library throw exception when calculate hash for key of the type string
#if NETFRAMEWORK
                Logger = new TraceSourceLogger(),
#endif
#if NETSTANDARD || NETCOREAPP2_0_OR_GREATER
                Logger = new IgniteLogger(loggerFacrtory),
#endif
            };

            if (configuration.Certificate != null) {
                ignateClientConfiguration.SslStreamFactory = new CertificateSslStreamFactory() {
                    Certificate = configuration.Certificate,
                    CheckCertificateRevocation = false,
                    SkipServerCertificateValidation = true,
                };
            }
            else if (configuration.CertificateFindValue != null) {
                ignateClientConfiguration.SslStreamFactory = new CertStoreSsl​Stream​Factory() {
                    FindValue = configuration.CertificateFindValue,
                    X509FindType = configuration.CertificateX509FindType,
                    StoreName = configuration.CertificateStoreName,
                    StoreLocation = configuration.CertificateStoreLocation,
                    CheckCertificateRevocation = false,
                    SkipServerCertificateValidation = true,
                };
            }
            else if (configuration.CertificatePath != null) {
                ignateClientConfiguration.SslStreamFactory = new Ssl​Stream​Factory() {
                    CertificatePassword = configuration.CertificatePassword,
                    CertificatePath = configuration.CertificatePath,
                    CheckCertificateRevocation = false,
                    SkipServerCertificateValidation = true,
                };
            }

            if (ignateClientConfiguration.SslStreamFactory == null) {
                throw new InvalidOperationException("Invalid configuration. Not set SSL stream factory");
            }

            return ignateClientConfiguration;
        }
    }
}