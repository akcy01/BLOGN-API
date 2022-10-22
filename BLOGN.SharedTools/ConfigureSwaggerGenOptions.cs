using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOGN.SharedTools
{
    public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            options.AddSecurityDefinition(
                "Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Kimlik doğrulama için bu alanı kullanabilirsiniz. Bearer ön ekini girdikten sonra bir boşluk bırakarak token girişi yapabilirsiniz. Örneğin - 'Bearer mytokenmytokenmytoken ' ",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

        }
    }
}
