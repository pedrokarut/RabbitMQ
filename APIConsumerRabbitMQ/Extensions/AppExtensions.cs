using APIConsumerRabbitMQ.Bus;
using MassTransit;

namespace APIConsumerRabbitMQ.Extensions
{
    internal static class AppExtensions
    {
        public static void AddRabbtMQService(this IServiceCollection services)
        {
            services.AddTransient<IPublishBus, PublishBus>();
            //interface criada pra colocar o serviço do rabbitmq pra rodar
            services.AddMassTransit(busconfigurator =>
            {
                //adicionando o consumer da fila
                busconfigurator.AddConsumer<RelatorioSolicitadoEventConsumer>();

                busconfigurator.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri("amqp://localhost:5672"), host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
            });
        }
    }
}
