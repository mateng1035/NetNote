using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetNode.Tools;

namespace NetNode
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;

            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //var connection = @"Server=192.168.101.81;Database=NetNote;UID=sa;PWD=sa123456;";
            var connection = @"Data Source=192.168.101.81;Initial Catalog=NetNode;User ID=sa;Password=sa123456;";
            services.AddDbContext<NoteContext>(
                options => options.UseSqlServer(connection, i =>
                {
                    i.EnableRetryOnFailure();
                    i.CommandTimeout(60);
                    i.UseRowNumberForPaging();
                })
                );
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<INoteTypeRepository, NoteTypeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Note}/{action=List}/{id?}");
            });
        }
    }
}
