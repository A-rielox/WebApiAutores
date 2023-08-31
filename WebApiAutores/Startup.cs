using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using WebApiAutores.Servicios;
using WebAPIAutores.Filtros;
using WebAPIAutores.Middlewares;
using WebAPIAutores.Servicios;

namespace WebApiAutores;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    //////////////////////////////////////////
    /////////////////////////////////////////////
    public void ConfigureServices(IServiceCollection services)
    {
        // AddJsonOptions p' evitar el error del cyclo al llamar entidades que se hacen referencia
        // ( x las q creo los DTO's )
        services.AddControllers(opciones =>
        {
            opciones.Filters.Add(typeof(FiltroDeExcepcion));
        }).AddJsonOptions(x => 
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));


        // cunado queran un IServicio => pasale un ServicioA
        // servicioA requiere ILogger y se le va a pasar automaticamente TODAS sus dependencias
        services.AddTransient<IServicio, ServicioA>();
        services.AddTransient<MiFiltroDeAccion>(); // transient xq no necesito mantener ni un tipo de estado
        services.AddHostedService<EscribirEnArchivo>();


        services.AddResponseCaching(); //           *** luego en un controlador a un endpoint le pongo [ResponseCache(Duration = 10)] son 10 segs

        // instalar Microsoft.AspNetCore.Authentication.JwtBearer
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(); //           * p' ocupar [Authorize] en controladores


        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        // p' guardar todas las respuestas http
        //app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
        app.UseLoguearRespuestaHTTP();



        // aplica el middleware solo si se va a /ruta1
        app.Map("/ruta1", app =>
        {
            app.Run(async contexto =>
            {
                await contexto.Response.WriteAsync("Estoy secuestrando la tuberia.");
            });
        });




        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();


        app.UseResponseCaching(); //           ***



        app.UseAuthorization(); //           *



        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

}
