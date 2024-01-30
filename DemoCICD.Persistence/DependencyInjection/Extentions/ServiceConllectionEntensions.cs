using Castle.MicroKernel.Registration;
using Castle.Windsor.Installer;
using Castle.Windsor;
using Microsoft.Extensions.DependencyInjection;
using QLTS.Contract;
using QLTS.Persistence.SessionManagement;
using Microsoft.Extensions.Configuration;

namespace QLTS.Persistence.DependencyInjection.Extentions;

public static class ServiceConllectionEntensions
{

    /// <summary>
    /// khởi tạo container
    /// </summary>
    /// <param name="app"></param>
    /// <param name="services"></param>
    public static void InitializeContainer(this IServiceCollection services, IConfiguration configuration)
    {
        try
        {
            // đăng ký middleware để authen và tạo fxcontext
            //app.UseMiddleware<FxJwtMiddleware>();

            // Initialize Windsor
            var container = new WindsorContainer();

            /* đăng ký từ file config */
            // get config file
            var includes1 = configuration.GetRequiredSection("castle");
            var includes = new string[] { "services.config" };

            // install from config
            List<ConfigurationInstaller> installers = new List<ConfigurationInstaller>();
            foreach (var item in includes)
            {
                var configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", item);
                if (!File.Exists(configFile))
                    throw new Exception($"logging.config not found at: {configFile}");

                installers.Add(Configuration.FromXmlFile(configFile));
            }
            container.Install(installers.ToArray());

            IoC.Initialize(container);
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// khởi tạo tham số context
    /// </summary>
    /// <param name="app"></param>
    /// <param name="services"></param>
    public static void InitializeContext(this IServiceCollection services)
    {
        IoC.Container.Register(Component
                .For<NhibernateSessionFactoryStore>()
                .ImplementedBy<NhibernateSessionFactoryStore>()
                .LifestylePerThread())
            .Register(Component
                .For<NhibernateSessionStore>()
                .ImplementedBy<NhibernateSessionStore>()
                .LifestyleScoped()
            );
    }
}
