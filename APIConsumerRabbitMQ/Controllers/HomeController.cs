using APIConsumerRabbitMQ.Bus;
using APIConsumerRabbitMQ.Relatorios;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace APIConsumerRabbitMQ.Controllers
{
    internal static class HomeController
    {
        public static void AddApiEndpoints(this WebApplication app)
        {
            app.MapPost("solicitar-relatorio/{name}", async (string name, IPublishBus bus, CancellationToken ct = default) => 
            {
                var solicitacao = new SolicitacaoRelatorio()
                {
                    Id = Guid.NewGuid(),
                    Nome = name,
                    Status = "Pendente",
                    ProcessedTime = null
                };

                Lista.Relatorios.Add(solicitacao);

                var eventRequest = new RelatorioSolicitadoEvent(solicitacao.Id, solicitacao.Nome);

                await bus.PublishAsync(eventRequest, ct);
                //publicou na fila

                return Results.Ok(solicitacao);
            });

            app.MapGet("relatorios", () => Lista.Relatorios);
        }
    }
}
