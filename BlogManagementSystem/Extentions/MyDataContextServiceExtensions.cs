using Microsoft.EntityFrameworkCore;
using Repository.DataContext;

namespace BlogManagementSystem.Extentions
{
    public static class MyDataContextServiceExtensions
    {
        public static IServiceCollection AddMyDataContextService(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<MyDataContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}
