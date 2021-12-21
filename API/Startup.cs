using API.Extensions;
using API.Helpers;
using API.Middleware;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddAutoMapper(typeof(MappingProfiles)); //agregamos nuestro automapper como servicio - ponemos en el parentesis la clase mapper que se creo dentro de helpers
            services.AddControllers();
            //services.AddSwaggerGen();
            services.AddDbContext<StoreContext>(x => 
                x.UseSqlite(_config.GetConnectionString("DefaultConnection")));//AddDbContext significa que el servicio tendra vida por el tiempo de vida de la peticion
            
            services.AddAplicationServices(); //agregamos una extension

            //agregamos una extension para la documentacion del swagger
            services.AddSwaggerDocumentation();

            //agregarmos cors
            services.AddCors(opt=>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
                            
            //if (env.IsDevelopment())
            //{
                //app.UseDeveloperExceptionPage();
                //reemplazamos el middleware exception de arriba por nuestro ExceptionMiddleware
            //}

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();//permite cargar archivos estaticos en nuestro proyecto como imagenes por ejemplo

            //agregando cors policy 
            app.UseCors("CorsPolicy");
            
            app.UseAuthorization();

             //agregamos una extension para la documentacion del swagger
            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
