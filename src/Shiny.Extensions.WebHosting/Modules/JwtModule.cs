using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Shiny.Modules;

public class JwtModule
{
    public void Add(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization();
        
        // builder.Services.AddHealthChecks()
        // .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection"));
        
        var cfg = builder.Configuration;
        // builder
        //     .Services
        //     .AddAuthentication(options =>
        //     {
        //         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //         options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        //     })
        //     .AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
        //     {
        //         ValidIssuer = cfg["Jwt:Issuer"],
        //         ValidAudience = cfg["Jwt:Audience"],
        //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Jwt:Key"]!)),
        //         ValidateIssuer = false, // TODO
        //         ValidateAudience = false, // TODO
        //         ValidateLifetime = true,
        //         ValidateIssuerSigningKey = true
        //     });
    }

    public void Use(WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}