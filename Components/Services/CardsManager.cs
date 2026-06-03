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
            existing.DataComecoProjeto = card.DataComecoProjeto;
            existing.DataConclusao = card.DataConclusao;
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
    public static async Task ArquivarCardAsync(Card card)
    {
        var existing = TodosCards.FirstOrDefault(x => x.Id == card.Id);

        if(existing != null)
        {
            existing.DataArquivamento = card.DataArquivamento;
            existing.MotivoArquivamento = card.MotivoArquivamento;
            await DbContext.UpdateCardStatusAsync(existing);
        }
        Reload();
        OnChange?.Invoke();
    }
    public static async Task DesarquivarCardAsync(Card card)
    {
        var existing = TodosCards.FirstOrDefault(x => x.Id == card.Id);

        if(existing != null)
        {
            existing.DataArquivamento = null;
            existing.MotivoArquivamento = "";
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

    public static int GetElapsedTimeByCardId(string id)
    {
        var card = CardsDisponiveis.FirstOrDefault(x => x.Id == id);
        if (card?.DataComecoProjeto == null) return 0;

        return (DateTime.Now - card.DataComecoProjeto.Value).Days;
    }

    public static bool IsAtrasado(Card card)
    {
        if (card.Prazo == null) return false;
        if(card.CurrentStatus == Status.Concluido) return false;
        return card.Prazo < DateTime.Now;
    }
    public static string GetRemainingDays(Card card)
    {
        if(card.Prazo == null) return "";
        if(card.DataConclusao == null)
        {
            if(card.Prazo < DateTime.Now) return $"{Math.Abs((card.Prazo.Value.Date - DateTime.Today).Days)}d atrasado";
            else return (card.Prazo.Value.Date - DateTime.Today).Days + "d restantes" ;
        }
        else
        {
            if(card.Prazo < card.DataConclusao) return $"{Math.Abs((card.Prazo.Value.Date - card.DataConclusao.Value.Date).Days)}d atrasado";
            else return (card.Prazo.Value.Date - card.DataConclusao.Value.Date).Days + "d adiantado";
        }
    }
    public static string GetElapsedDays(Card card)
    {
        if(card.DataComecoProjeto == null) return "";
        return $"{Math.Abs((card.DataComecoProjeto.Value.Date - DateTime.Today).Days)}d percorridos";
    }
    public static decimal GetProgressPercentage(Card card, StatusEtapa status)
    {
        var etapas = EtapasManager.TodasEtapas
            .Where(x => x.IdProjeto == card.Id)
            .ToList();

        if (etapas.Count == 0)
            return 0;

        decimal total = etapas.Count;
        decimal concluidas = etapas.Count(x => x.Status == status);

        return (concluidas / total) * 100;
    }

    public static Card? GetCardById(string id) => CardsDisponiveis.FirstOrDefault(x => x.Id == id);

}