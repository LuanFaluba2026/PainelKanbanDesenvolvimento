using PainelKanbanDesenvolvimento.Components.Models;
using PainelKanbanDesenvolvimento.Components.Models.Enum;
using PainelKanbanDesenvolvimento.Components.Models.Kanban;
using PainelKanbanDesenvolvimento.Components.RazorComponents;

namespace PainelKanbanDesenvolvimento.Components.Services;
public static class CardsManager
{
    public static List<Card> TodosCards { get; set; } = new();
    public static List<Card> CardsDisponiveis { get; set; } = new();

    public static List<Card> CardsBacklog { get; set; } = new();
    public static List<Card> CardsAndamento { get; set; } = new();
    public static List<Card> CardsRevisao { get; set; } = new();
    public static List<Card> CardsConcluido { get; set; } = new();
    public static event Action? OnChange;

    static CardsManager()
    {
        Reload();
    }

    public static void Reload()
    {
        TodosCards = DbContext.GetAll<Card>("Cards");
        CardsDisponiveis = new List<Card>(TodosCards);
        AssembleCards();
    }
    public static async Task AddNewCard(Card card)
    {
        await DbContext.InsertCardAsync(card);
        TodosCards.Add(card);
        Reload();
        OnChange?.Invoke();
    }
    public static async Task UpdateCardAsync(Card card)
    {
        var existing = TodosCards.FirstOrDefault(x => x.Id == card.Id);

        if(existing != null)
        {
            existing.Nome = card.Nome;
            existing.Descricao = card.Descricao;
            existing.CurrentStatus = card.CurrentStatus;
            existing.Prioridade = card.Prioridade;
            existing.Prazo = card.Prazo;
            existing.IndexResponsavel = card.IndexResponsavel;
            existing.IndexSetor = card.IndexSetor;
            existing.Observacao = card.Observacao;
            await DbContext.UpdateCardStatusAsync(existing);
        }
        Reload();
        OnChange?.Invoke();
    }
    public static async Task RemoveCardAsync(string cardId)
    {
        await DbContext.DeleteCardByIdAsync(cardId);
        TodosCards.RemoveAll(x => x.Id == cardId);
        Reload();
        OnChange?.Invoke();
    }
    private static void AssembleCards()
    {
        CardsBacklog = CardsDisponiveis.Where(x => x.CurrentStatus == Status.Backlog).ToList();
        CardsAndamento = CardsDisponiveis.Where(x => x.CurrentStatus == Status.EmAndamento).ToList();
        CardsRevisao = CardsDisponiveis.Where(x => x.CurrentStatus == Status.Revisao).ToList();
        CardsConcluido = CardsDisponiveis.Where(x => x.CurrentStatus == Status.Concluido).ToList();
    }

    public static void FiltrarCards(int? responsavelIdx = null, int? setorIdx = null, Prioridade? p = null)
    {
        IEnumerable<Card> query = TodosCards;

        if (responsavelIdx.HasValue)
            query = query.Where(c => c.IndexResponsavel == responsavelIdx.Value);

        if (setorIdx.HasValue)
            query = query.Where(c => c.IndexSetor == setorIdx.Value);

        if (p.HasValue)
            query = query.Where(c => c.Prioridade == p.Value);

        CardsDisponiveis = query.ToList();
        AssembleCards();
        OnChange?.Invoke();
    }

    public static int GetElapsedTimeByCardId(string id)
    {
        var card = CardsDisponiveis.FirstOrDefault(x => x.Id == id);
        if (card?.DataComecoProjeto == null) return 0;

        return (DateTime.Now - card.DataComecoProjeto.Value).Days;
    }

    public static bool IsAtrasado(Card card)
    {
        if (card.Prazo == null) return false;
        return card.Prazo < DateTime.Now;
    }

    public static Card? GetCardById(string id)
        => CardsDisponiveis.FirstOrDefault(x => x.Id == id);
}