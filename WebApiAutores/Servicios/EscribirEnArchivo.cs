// ejecuta codigo recurrente
// se configura en startup - services.AddHostedService<EscribirEnArchivo>();

namespace WebAPIAutores.Servicios;

public class EscribirEnArchivo : IHostedService
{
    private readonly IWebHostEnvironment env;
    private readonly string nombreArchivo = "Archivo 1.txt";
    private Timer timer;

    public EscribirEnArchivo(IWebHostEnvironment env)
    {
        this.env = env;
    }

    // se ejecuta cuando arranca la web-api
    public Task StartAsync(CancellationToken cancellationToken)
    {
        timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        Escribir("Proceso iniciado. ");
        return Task.CompletedTask;
    }

    // se ejecuta cuando se apaga la web-api ( no necesariamente se ejecuta, xejemplo si se apaga por q se lanza un error )
    public Task StopAsync(CancellationToken cancellationToken)
    {
        timer.Dispose();
        Escribir("Proceso finalizado. ");
        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        Escribir("Proceso en ejecución: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
    }

    private void Escribir(string mensaje)
    {
        var ruta = $@"{env.ContentRootPath}\wwwroot\{nombreArchivo}";
        using (StreamWriter writer = new StreamWriter(ruta, append: true))
        {
            writer.WriteLine(mensaje);
        }
    }
}
