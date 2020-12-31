using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace SpeechChatAssistant.Demo.Web
{
    public class Startup
    {
        private JsonSerializerSettings jsonSerializerSetting = new JsonSerializerSettings()
        {
            //ContractResolver = new CamelCasePropertyNamesContractResolver(),//ʹ���շ���ʽ��key
            ContractResolver = new DefaultContractResolver(),//��ʹ���շ���ʽ��key
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,//����ѭ������
            DateFormatString = "yyyy-MM-dd"//����ʱ���ʽ
        };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            JsonConvert.DefaultSettings = () => { return jsonSerializerSetting; };

            services.AddControllersWithViews()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = jsonSerializerSetting.ReferenceLoopHandling;
                        options.SerializerSettings.ContractResolver = jsonSerializerSetting.ContractResolver;
                        options.SerializerSettings.DateFormatString = jsonSerializerSetting.DateFormatString;
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
}
