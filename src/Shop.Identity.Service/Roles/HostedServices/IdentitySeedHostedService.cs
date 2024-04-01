using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shop.Identity.Service.Entities;
using Shop.Identity.Service.Settings;
using System.Threading;
using System.Threading.Tasks;

namespace Shop.Identity.Service.Roles.HostedServices
{
    public class IdentitySeedHostedService : IHostedService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IdentitySettings settings;

        public IdentitySeedHostedService(
            IServiceScopeFactory serviceScopeFactory,
            IOptions<IdentitySettings> identityOptions)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            settings = identityOptions.Value;
        }

        /*
           запускается при запуске службы
            здесь мы хотим создать роль игрока и роль администратора.
            также мы хотим создать пользователя-администратора
            при этом наша служба уже запускается с этими двумя созданными ролями и созданным пользователем-администратором.
         */
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceScopeFactory.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //здесь мы собираемся эффективно создать две основные роли
            await CreateRoleIfNotExistsAsync(Roles.Admin, roleManager);
            await CreateRoleIfNotExistsAsync(Roles.Сustomer, roleManager);

            var adminUser = await userManager.FindByEmailAsync(settings.AdminUserEmail);

            if(adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = settings.AdminUserEmail,
                    Email = settings.AdminUserEmail
                };

                //здесь мы создаем пользователей-администраторов, а также назначаем ему роль администратора
                await userManager.CreateAsync(adminUser, settings.AdminUserPassword);
                await userManager.AddToRoleAsync(adminUser, Roles.Admin);
            }
        }

        /*
            запускается, когда службы заканчиваются
            в нашем случае нам не нужно ничего делать, когда услуга закончится
         */
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private static async Task CreateRoleIfNotExistsAsync(
            string role,
            RoleManager<ApplicationRole> roleManager
        )
        {
            var roleExists = await roleManager.RoleExistsAsync(role);

            if(!roleExists)
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = role });
            }
        }
    }
}
