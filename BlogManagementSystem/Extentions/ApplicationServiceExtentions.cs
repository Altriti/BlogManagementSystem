using Repository.Repositories;
using Service;
using Service.Contract;
using Service.Implementation;
using Service.serviceInterfaces;
using Service.services;

namespace BlogManagementSystem.Extentions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IBlogService, BlogService>();

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddControllers();

            services.AddHttpContextAccessor();

            services.AddScoped<UserAccessor>();

            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommentService, CommentService>();

            return services;
        }
    }
}
