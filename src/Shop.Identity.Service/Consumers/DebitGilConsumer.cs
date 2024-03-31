using MassTransit;
using Microsoft.AspNetCore.Identity;
using Shop.Identity.Contracts;
using Shop.Identity.Service.Entities;
using Shop.Identity.Service.Exceptions;
using System.Threading.Tasks;

namespace Shop.Identity.Service.Consumers
{
    public class DebitGilConsumer : IConsumer<DebitGil>
    {

        private readonly UserManager<ApplicationUser> userManager;

        public DebitGilConsumer(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task Consume(ConsumeContext<DebitGil> context)
        {
            var message = context.Message;

            var user = await userManager.FindByIdAsync(message.UserId.ToString());

            if(user == null)
            {
                throw new UnknownUserException(message.UserId);
            }

            if(user.MessageIds.Contains(context.MessageId.Value))
            {
                await context.Publish(new GilDebited(message.CorrelationId));
                return;
            }

            user.Money -= message.Gil;

            if(user.Money < 0)
            {
                throw new InsufficientFundsException(message.UserId, message.Gil);
            }

            user.MessageIds.Add(context.MessageId.Value);
            await userManager.UpdateAsync(user);

            var gilDebitedTask = context.Publish(new GilDebited(message.CorrelationId));
            var userUpdatedTask = context.Publish(new UserUpdated(user.Id, user.Email, user.Money));
            await Task.WhenAll(userUpdatedTask, gilDebitedTask);

        }
    }
}
