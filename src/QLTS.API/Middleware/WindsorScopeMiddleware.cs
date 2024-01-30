using Castle.MicroKernel.Lifestyle;
using QLTS.Contract;
using QLTS.Persistence.SessionManagement;

namespace QLTS.API.Middleware;

public class WindsorScopeMiddleware
{
    private readonly RequestDelegate next;
    public WindsorScopeMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        /// mở scope cho mỗi request
        using (var scope = IoC.Container.BeginScope())
        {
            //var nhibernateContext = IoC.Resolve<NhibernateSessionStore>();
            await next(context);
            //nhibernateContext.Dispose();
        }
    }
}
