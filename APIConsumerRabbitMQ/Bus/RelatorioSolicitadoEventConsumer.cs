using System.Runtime.CompilerServices;
using APIConsumerRabbitMQ.Relatorios;
using MassTransit;

namespace APIConsumerRabbitMQ.Bus
{
    internal sealed class RelatorioSolicitadoEventConsumer : IConsumer<RelatorioSolicitadoEvent>
    {
        private readonly ILogger<RelatorioSolicitadoEventConsumer> _logger;

        public RelatorioSolicitadoEventConsumer(ILogger<RelatorioSolicitadoEventConsumer> logger)
        {
            //injeção de dependência
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<RelatorioSolicitadoEvent> context)
        {
            var message = context.Message;
            //context.message é a informação que chega pro consumer 
            _logger.LogInformation("Processando Relatório Id:{Id}, Nome:{Nome}", message.Id, message.Name);

            await Task.Delay(100);

            //atualizando relatório
            var relatorio = Lista.Relatorios.FirstOrDefault(x => x.Id == message.Id);
            if(relatorio != null)
            {
                relatorio.Status = "Completo";
                relatorio.ProcessedTime = DateTime.Now;
            }
            
            _logger.LogInformation("Relatório Processado Id:{Id}, Nome:{Nome}", message.Id, message.Name);
        }
    }
}
