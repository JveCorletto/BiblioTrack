namespace SysBiblioteca.UI.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            // Verifica si la sesión está disponible antes de acceder a ella
            if (!httpContext.Session.IsAvailable)
            {
                Console.WriteLine("Sesión no disponible. Verifica el middleware de sesión.");
                return _next(httpContext);
            }

            var path = httpContext.Request.Path;
            var rol = httpContext.Session.GetString("Rol");

            if (path.HasValue && path.Value.StartsWith("/Home") == true && rol != null)
            {
                httpContext.Response.Redirect("/SysBiblioteca");
            }
            if (path.HasValue && path.Value.StartsWith("/Home") == false && rol == null)
            {
                httpContext.Response.Redirect("/Home");
            }
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}