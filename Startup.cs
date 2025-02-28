﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyShop.Models;

namespace MyShop
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
            var connString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<MyShopDbContext>(options =>
            {
                options.UseSqlServer(connString);
            });

            services.AddMvc();
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
            }
             //изменили локализацию чтоб правильно валидировал сепаратор для децимал
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(new CultureInfo("es-AR")),
                SupportedCultures = new List<CultureInfo>
                {
                 new CultureInfo("en-US")
                },
                SupportedUICultures = new List<CultureInfo>
                {
                  new CultureInfo("en")
                }
            });
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area}/{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
