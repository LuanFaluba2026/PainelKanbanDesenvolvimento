using PainelKanbanDesenvolvimento.Components.Models.Kanban;
namespace PainelKanbanDesenvolvimento.Components.Services;
public static class EtapasManager
{
    public static List<Etapa> TodasEtapas {get; set;} = new();
    static EtapasManager()
    {
        AssembleEtapas();
    }
    static void AssembleEtapas()
    {
        TodasEtapas = DbContext.GetAll<Etapa>("Etapas");
    }
    public static async Task AddNewEtapaAsync(Etapa etapa)
    {
        TodasEtapas.Add(etapa);
        await DbContext.InsertEtapaAsync(etapa);
    }
    public static int GetElapsedTimeByCardId(string id)
    {
        var etapa = TodasEtapas.FirstOrDefault(x => x.Id == id);
        if (etapa?.DataCriacao == null) return 0;

        return (DateTime.Now - etapa.DataCriacao.Value).Days;
    }
}