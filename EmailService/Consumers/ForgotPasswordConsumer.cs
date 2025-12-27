using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailService.Interfaces;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Newtonsoft.Json;
using ModalLayer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace EmailService.Consumers
{
    public class ForgotPasswordConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ForgotPasswordConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "forgot-password-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (sender, args) =>
            {
                using var scope = _serviceProvider.CreateScope(); // scoped service here
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                try
                {
                    var json = Encoding.UTF8.GetString(args.Body.ToArray());
                    var message = JsonConvert.DeserializeObject<ForgotPasswordEvent>(json);

                    var resetLink = $"https://localhost:4200/reset-password?token={message.Token}";

                    await emailService.SendEmailAsync(
                        message.Email,
                        "Reset Your Password",
                      $" KWT Token To Reset Password : {resetLink}" 
                    );

                    channel.BasicAck(args.DeliveryTag, false);
                }
                catch (Exception)
                {
                    channel.BasicNack(args.DeliveryTag, false, true);
                }
            };

            channel.BasicConsume(
                queue: "forgot-password-queue",
                autoAck: false,
                consumer: consumer
            );

            return Task.CompletedTask;
        }

    }
}
