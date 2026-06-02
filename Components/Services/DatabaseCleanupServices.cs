using System.Diagnostics;
using PainelKanbanDesenvolvimento.Components.Models.Kanban;

namespace PainelKanbanDesenvolvimento.Components.Services;
public class DatabaseCleanupServices : BackgroundService
{
    private readonly IServiceScopeFactory scopeFactory;
    public DatabaseCleanupServices(IServiceScopeFactory scopeFactory)
    {
        this.scopeFactory = scopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Excluindo dados incompletos");
        while(!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var etapas = DbContext.GetAll<Etapa>("Etapas").Where(x => !CardsManager.TodosCards.Any(z => z. Id == x.IdProjeto)).ToList();
            foreach(var etapa in etapas)
            {
                await DbContext.DeleteEtapa(etapa);
                if(EtapasManager.TodasEtapas.Contains(etapa))
                    EtapasManager.TodasEtapas.Remove(etapa);
            }
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}