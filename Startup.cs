using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsERT_Demo_HS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration; // get access to configuration data
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //RazonRuntimeCompilation for easier debugging
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            //Making sure my app is configured to handle CORS for external domains, overriding the Same-Origin Policy.
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        //Add my domain to the policy
                        builder.WithOrigins("insertdemohs20230915211235.azurewebsites.net")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });
            services.AddControllersWithViews(); //Enable attribute routing
            services.AddHttpClient();
            services.AddSingleton<IConfiguration>(Configuration); //register for dependency injection
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //Use the specified CORS policy
            app.UseCors("AllowSpecificOrigin");

            //Add authentication middleware
            app.UseAuthentication(); 
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //enable attribute routing
                endpoints.MapControllers();

            });
        }
    }
}
