using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger; 
using TodoBLL.Abstract;
using TodoBLL.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoDAL;
using UnitOfWorkRepository;

namespace TodoApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = @"Server=DESKTOP-0UTFTAA\MASUD;Database=EFCoreStudentDb;Trusted_Connection=True;";
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection)); 

            //Add dependency injection
            services.AddTransient<ITodoRepository, TodoRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "http://localhost:45049",
                        ValidAudience = "http://localhost:45049",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("IxrAjDoa2FqElO7IhrSrUJELhUckePEPVpaePlS_Xaw"))
                    };
                });

            // Add framework services.
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseAuthentication();
            app.UseMvc();
        }

    }
}
