using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace CalculatorService
{
    public class Startup
    {
        public string SecretKey { get; set; } = "98DAD3276CADE1F27204455467F20BEB11F2E98911A91399A5D98A4F9939FC93";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            //{
            //    byte[] key = Convert.FromBase64String(SecretKey);
            //    SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);

            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = "788e6e97-6209-4ebc-aaa9-7f01321fe90a",
            //        ValidAudience = "52b8fa2a-660e-4b7d-a799-b9cea7b3466a",
            //        IssuerSigningKey = securityKey,
            //        ClockSkew = TimeSpan.FromMinutes(0)
            //    };
            //});


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 options.Authority = "https://login.microsoftonline.com/788e6e97-6209-4ebc-aaa9-7f01321fe90a/v2.0";
                 options.RequireHttpsMetadata = false;

                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidIssuer = "https://login.microsoftonline.com/788e6e97-6209-4ebc-aaa9-7f01321fe90/v2.0",
                     ValidAudience = "52b8fa2a-660e-4b7d-a799-b9cea7b3466a",
                     ClockSkew = TimeSpan.FromMinutes(0),
                };

                 options.Events = new JwtBearerEvents()
                 {
                     OnAuthenticationFailed = context =>
                     {
                         context.Response.StatusCode = 401;
                         context.Response.ContentType = "application/json";
                         var err = context.Exception.ToString();
                         var result = JsonConvert.SerializeObject(new { err });
                         return context.Response.WriteAsync(result);
                     }
                 };
            });

            services.AddControllers();

            services.AddSwaggerGen(x => 
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Values Api",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Values Api V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public async Task<List<SecurityKey>> GetSecurityKeysAsync()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));

            var url = "https://assurwarelabs.b2clogin.com/assurwarelabs.onmicrosoft.com/b2c_1_susi_v2/discovery/v2.0/keys";
            HttpResponseMessage response = await client.GetAsync(url);

            var result = await response.Content.ReadAsStringAsync();

            var res = JsonConvert.DeserializeObject<JsonWebKeySet>(result);

            return GetSecurityKeys(res);
        }

        private List<SecurityKey> GetSecurityKeys(JsonWebKeySet jsonWebKeySet)
        {
            var keys = new List<SecurityKey>();

            foreach (var key in jsonWebKeySet.Keys)
            {
                if (key.Kty == "RSA")
                {
                    if (key.X5C != null && key.X5C.Length > 0)
                    {
                        string certificateString = key.X5C[0];
                        var certificate = new X509Certificate2(Convert.FromBase64String(certificateString));

                        var x509SecurityKey = new X509SecurityKey(certificate)
                        {
                            KeyId = key.Kid
                        };

                        keys.Add(x509SecurityKey);
                    }
                    else if (!string.IsNullOrWhiteSpace(key.E) && !string.IsNullOrWhiteSpace(key.N))
                    {
                        byte[] exponent = Encoding.ASCII.GetBytes(key.E);
                        byte[] modulus = Encoding.ASCII.GetBytes(key.N);

                        var rsaParameters = new RSAParameters
                        {
                            Exponent = exponent,
                            Modulus = modulus
                        };

                        var rsaSecurityKey = new RsaSecurityKey(rsaParameters)
                        {
                            KeyId = key.Kid
                        };

                        keys.Add(rsaSecurityKey);
                    }
                }
                else
                {
                    throw new NotImplementedException("Only RSA key type is implemented for token validation");
                }
            }

            return keys;
        }
    }
}
