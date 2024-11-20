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