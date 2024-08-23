using Couple_Quiz.Common.Interface.IFile;
using Couple_Quiz.Common.Service.FileService;
using Couple_Quiz.Interface.Repositories;
using Couple_Quiz.Repositories;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Couple_Quiz.Configuration
{
    public class ServiceConfiguration
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddSingleton<IFileValidation, FileValidationService>();
            services.AddScoped<IFileUpload, FileService>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
        }
    }
}
