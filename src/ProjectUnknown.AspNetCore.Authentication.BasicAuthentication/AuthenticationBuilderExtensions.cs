using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProjectUnknown.Common;

namespace ProjectUnknown.AspNetCore.Authentication.BasicAuthentication
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddBasicAuthentication(this AuthenticationBuilder builder)
        {
            Ensure.IsNotNull(builder, nameof(builder));

            return AddBasicAuthenticationInternal(builder, BasicAuthenticationDefaults
                .AuthenticationScheme, null, null);
        }

        public static AuthenticationBuilder AddBasicAuthentication(this AuthenticationBuilder builder,
            string authenticationScheme)
        {
            Ensure.IsNotNull(builder, nameof(builder));
            Ensure.IsNotNullOrEmpty(authenticationScheme, nameof(authenticationScheme));

            return AddBasicAuthenticationInternal(builder, authenticationScheme, null, null);
        }

        public static AuthenticationBuilder AddBasicAuthentication(this AuthenticationBuilder builder,
            string authenticationScheme, string displayName)
        {
            Ensure.IsNotNull(builder, nameof(builder));
            Ensure.IsNotNullOrEmpty(authenticationScheme, nameof(authenticationScheme));
            Ensure.IsNotNullOrEmpty(displayName, nameof(displayName));

            return AddBasicAuthenticationInternal(builder, authenticationScheme, displayName, null);
        }

        public static AuthenticationBuilder AddBasicAuthentication(this AuthenticationBuilder builder,
            string authenticationScheme, Action<BasicAuthenticationOptions> configureOptions)
        {
            Ensure.IsNotNull(builder, nameof(builder));
            Ensure.IsNotNullOrEmpty(authenticationScheme, nameof(authenticationScheme));
            Ensure.IsNotNull(builder, nameof(configureOptions));

            return AddBasicAuthenticationInternal(builder, authenticationScheme, null, configureOptions);
        }

        public static AuthenticationBuilder AddBasicAuthentication(this AuthenticationBuilder builder,
            string authenticationScheme, string displayName, Action<BasicAuthenticationOptions> configureOptions)
        {
            Ensure.IsNotNull(builder, nameof(builder));
            Ensure.IsNotNullOrEmpty(authenticationScheme, nameof(authenticationScheme));
            Ensure.IsNotNullOrEmpty(displayName, nameof(displayName));
            Ensure.IsNotNull(builder, nameof(configureOptions));

            return AddBasicAuthenticationInternal(builder, authenticationScheme, displayName, configureOptions);
        }

        public static AuthenticationBuilder WithCredentialsValidator<TValidator>(this AuthenticationBuilder builder)
            where TValidator : class, ICredentialsValidator
        {
            Ensure.IsNotNull(builder, nameof(builder));

            builder.Services.TryAddSingleton<ICredentialsValidator, TValidator>();

            return builder;
        }

        public static AuthenticationBuilder WithCredentialsValidator(this AuthenticationBuilder builder,
            ICredentialsValidator validator)
        {
            Ensure.IsNotNull(builder, nameof(builder));
            Ensure.IsNotNull(builder, nameof(validator));

            builder.Services.TryAddSingleton(validator);

            return builder;
        }

        private static AuthenticationBuilder AddBasicAuthenticationInternal(AuthenticationBuilder builder,
            string authenticationScheme, string displayName, Action<BasicAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(authenticationScheme,
                displayName, configureOptions);
        }
    }
}
