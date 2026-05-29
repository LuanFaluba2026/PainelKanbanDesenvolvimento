using PainelKanbanDesenvolvimento.Components.Models.Enum;

namespace PainelKanbanDesenvolvimento.Components.Models.Kanban;
public class Etapa
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string IdProjeto { get; set; } = "";
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int ResponsavelIndex { get; set; }
    public StatusEtapa Status { get; set; }
    public DateTime? Prazo { get; set; }
    public DateTime? DataCriacao { get; set; } = DateTime.Now;
    public DateTime? DataConclusao { get; set; }
}