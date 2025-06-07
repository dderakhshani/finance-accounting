using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;


public static class JwtServiceCollection
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {

        JwtConfigurations jwtConfigurations = configuration.GetSection("Jwt").Get<JwtConfigurations>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>
                            {
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    ValidIssuer = jwtConfigurations.Issuer,
                                    ValidAudience = jwtConfigurations.Audience,
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigurations.Secret)),
                                    ClockSkew = TimeSpan.Zero
                                };
                                options.Events = new JwtBearerEvents
                                {
                                    OnMessageReceived = context =>
                                    {
                                        var accessToken = context.Request.Query["access_token"].ToString();

                                        if (!string.IsNullOrEmpty(accessToken))
                                        {
                                            context.Token = accessToken ?? context.Token;
                                        }

                                        return Task.CompletedTask;
                                    }
                                };
                            });

        return services;

    }
}
