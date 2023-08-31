using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPIAutores.Filtros;

// necesito darlo de alta en Startup - services.AddTransient<MiFiltroDeAccion>();
// lo uso sobre cualquier endpoint - [ServiceFilter(typeof(MiFiltroDeAccion))]

public class MiFiltroDeAccion : IActionFilter
{
    private readonly ILogger<MiFiltroDeAccion> logger;

    public MiFiltroDeAccion(ILogger<MiFiltroDeAccion> logger)
    {
        this.logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        logger.LogInformation("Antes de ejecutar la acción");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        logger.LogInformation("Después de ejecutar la acción");

    }
}
