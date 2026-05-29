using PainelKanbanDesenvolvimento.Components.Models.Enum;

namespace PainelKanbanDesenvolvimento.Components.Models.Kanban;
public class Card
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public Status CurrentStatus { get; set; } = Status.Backlog;
    public Prioridade Prioridade { get; set; }
    public DateTime? Prazo { get; set;}
    public DateTime? DataCriacao { get; set; } = DateTime.Now;
    public DateTime? DataComecoProjeto { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get;set; } = string.Empty;
    public string? Observacao { get; set; }
    public int IndexSetor { get; set;}
    public int IndexResponsavel { get; set; }
}