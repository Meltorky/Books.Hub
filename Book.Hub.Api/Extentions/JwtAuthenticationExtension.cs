using Books.Hub.Appliction.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Books.Hub.Api.Extentions
{
    public static class JwtAuthenticationExtension
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            // Register authentication services and set JWT Bearer as the default scheme
            services.AddAuthentication(options =>
            {
                // Default scheme used by [Authorize] and the authentication middleware
                // to identify which handler to use for authenticating the incoming request.
                // In this case, it’s JwtBearer, so every [Authorize] will expect a JWT token.
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  // use third only

                // Default scheme used when the app needs to challenge (ask for credentials).
                // For example, when an unauthorized request is made, this determines
                // which handler will produce the 401 Unauthorized response.
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  // use third only

                // Default scheme used when automatic authentication happens.
                // This ensures any request automatically tries to authenticate using JWT.
                // No, you do NOT need to set all three.
                // Yes, you CAN use a simpler "third" approach — and it’s actually BETTER.
                // This is just a shortcut that sets both the authenticate and challenge schemes at once.
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Register the JWT Bearer authentication handler
            // Using DI-friendly overload that provides the service provider context.
            .AddJwtBearer();


            // Configure JwtBearerOptions using JwtOptions from DI
            services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .Configure<IOptions<JwtOptions>>((options, jwtOptionsAccessor) =>
                {
                    var jwtOptions = jwtOptionsAccessor.Value;

                    // Require HTTPS (recommended for production; prevents token over HTTP)
                    options.RequireHttpsMetadata = true;

                    // Saves the token into HttpContext after successful authentication.
                    // I can later access it via:
                    //   var token = await HttpContext.GetTokenAsync("access_token");
                    options.SaveToken = true;

                    // Configure how tokens are validated.
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Ensures the token was issued by a trusted authority (the Issuer)
                        ValidateIssuer = true,

                        // Ensures the token audience (who it’s intended for) matches your app
                        ValidateAudience = true,

                        // Ensures token hasn’t expired
                        ValidateLifetime = true,

                        // Ensures the token signature is valid (not tampered with)
                        ValidateIssuerSigningKey = true,

                        // The expected Issuer of the JWT (must match "iss" claim in token)
                        ValidIssuer = jwtOptions.Issuer,

                        // The expected Audience of the JWT (must match "aud" claim)
                        ValidAudience = jwtOptions.Audience,

                        // The secret key used to verify the signature of the token
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.Key)),

                        // Removes default 5-minute tolerance for token expiration.
                        // This makes expiration exact — if token expires at 12:00, it’s invalid at 12:00:01.
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }


        //    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration) 
        //    {
        //        // Bind JwtOptions the correct way
        //        services.Configure<JwtOptions>(configuration.GetSection("JWT"));

        //        services.AddAuthentication(options =>
        //        {
        //            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        //        })
        //        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        //        {
        //            // Retrieve configuration using DI (instead of singleton)
        //            var serviceProvider = services.BuildServiceProvider();
        //            var jwtOptions = serviceProvider.GetRequiredService<IOptions<JwtOptions>>().Value;

        //            options.RequireHttpsMetadata = false;
        //            options.SaveToken = true;
        //            options.TokenValidationParameters = new TokenValidationParameters
        //            {
        //                ValidateIssuer = true,
        //                ValidateAudience = true,
        //                ValidateLifetime = true,
        //                ValidateIssuerSigningKey = true,
        //                ValidIssuer = jwtOptions.Issuer,
        //                ValidAudience = jwtOptions.Audience,
        //                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
        //                ClockSkew = TimeSpan.Zero
        //            };
        //        });

        //        return services;

        //    }
        //}
    }
}
