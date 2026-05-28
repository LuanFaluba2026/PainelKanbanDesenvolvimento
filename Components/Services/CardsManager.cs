using PainelKanbanDesenvolvimento.Components.Models.Kanban;

namespace PainelKanbanDesenvolvimento.Components.Services;
public class CardsManager
{
    public static List<Card> GetCardsDisponiveis() => DbContext.GetAll<Card>("Cards");
    public static Card GetCardById(string id) => DbContext.GetById<Card>("Cards", id)!;
}