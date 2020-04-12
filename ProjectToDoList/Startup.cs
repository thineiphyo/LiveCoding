using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProjectToDoList.Models;
using ProjectToDoList.Repository;
using System.Net.Mime;
using ProjectToDoList.GlobalErrorHandling.Exception;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.OData.Json;
using Newtonsoft.Json;

namespace ProjectToDoList
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

            services.AddDbContext<ProjectTodoDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));
            services.AddControllers();
            services.AddTransient<IToDoTaskRepository, ToDoTaskRepository>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProjectToDoTask", Version = "v1" });
            });
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddControllers().ConfigureApiBehaviorOptions(options =>
                 {
                     options.InvalidModelStateResponseFactory = context =>
                     {
                         var result = new BadRequestObjectResult(context.ModelState);

                         // TODO: add `using System.Net.Mime;` to resolve MediaTypeNames
                         result.ContentTypes.Add(MediaTypeNames.Application.Json);


                         return result;
                     };
                 });
            services.AddControllers(mvcOptions =>
               mvcOptions.EnableEndpointRouting = false
               );

            services.AddOData();
            #region forSwagger
            services.AddMvc(options =>
            {
                ////https://github.com/OData/WebApi/issues/597
                ////https://q-a-assistant.info/computer-internet-technology/exception-connecting-excel-to-net-core-v1-1-odata-v4-add-at-least-one-media-type/3883239
                //// loop on each OData formatter to find the one without a supported media type
                foreach (var outputFormatter in options.OutputFormatters.OfType<Microsoft.AspNet.OData.Formatter.ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    // to comply with the media type specifications, I'm using the prs prefix, for personal usage
                    outputFormatter.SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<Microsoft.AspNet.OData.Formatter.ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    // to comply with the media type specifications, I'm using the prs prefix, for personal usage
                    inputFormatter.SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });
            #endregion
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
       
            //config.EnableDependencyInjection();
            app.ConfigureExceptionHandler(logger);
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjectToDoList V1");
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                routeBuilder.MapODataServiceRoute("odata", "api", GetEdmModel());
                routeBuilder.EnableDependencyInjection();

               
            });
           
           
        }
        IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<ToDoTask>("ToDoTasks");

            return odataBuilder.GetEdmModel();
        }
    }
   
}
