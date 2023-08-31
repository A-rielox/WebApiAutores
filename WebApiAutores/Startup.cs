using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApiAutores.Servicios;

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
        services.AddControllers().AddJsonOptions(x => 
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));


        // cunado queran un IServicio => pasale un ServicioA
        // servicioA requiere ILogger y se le va a pasar automaticamente TODAS sus dependencias
        services.AddTransient<IServicio, ServicioA>(); 



        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

}
