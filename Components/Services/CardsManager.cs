using PainelKanbanDesenvolvimento.Components.Models.Enum;
using PainelKanbanDesenvolvimento.Components.Models.Kanban;

namespace PainelKanbanDesenvolvimento.Components.Services;
public static class CardsManager
{
    public static List<Card> CardsDisponiveis {get; set;} = new();
    public static List<Card> CardsBacklog {get; set;} = new();
    public static List<Card> CardsAndamento {get; set;} = new();
    public static List<Card> CardsRevisao {get; set;} = new();
    public static List<Card> CardsConcluido {get; set;} = new();
    static CardsManager()
    {
        CardsDisponiveis = CardsManager.GetCardsDisponiveis();
        AssembleCards();
    }
    private static void AssembleCards()
    {
        CardsBacklog = CardsDisponiveis.Where(x => x.CurrentStatus == Status.Backlog).ToList();
        CardsAndamento = CardsDisponiveis.Where(x => x.CurrentStatus == Status.EmAndamento).ToList();
        CardsRevisao = CardsDisponiveis.Where(x => x.CurrentStatus == Status.Revisao).ToList();
        CardsConcluido = CardsDisponiveis.Where(x => x.CurrentStatus == Status.Concluido).ToList();
    }
    public static List<Card> GetCardsDisponiveis() => DbContext.GetAll<Card>("Cards");
    public static Card GetCardById(string id) => DbContext.GetById<Card>("Cards", id)!;
    public static int GetElapsedTimeByCardId(string id)
    {
        var now = DateTime.Now;
        var card = GetCardsDisponiveis().FirstOrDefault(x => x.Id == id);
        if(card == null || card.DataComecoProjeto == null) return 0;
        var elapsed = (now - card.DataComecoProjeto.Value).Days;
        return elapsed;
    }
    public static bool isAtrasado(string id)
    {
        var card = GetCardsDisponiveis().FirstOrDefault(x => x.Id == id);
        if(card == null) return true;
        return card.Prazo < card.DataComecoProjeto;
    }
}