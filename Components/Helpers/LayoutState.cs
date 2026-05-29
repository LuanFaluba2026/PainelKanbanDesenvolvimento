namespace PainelKanbanDesenvolvimento.Components.Helpers;
public class LayoutState
{
    public bool KanbanViewToggle {get; set;} = true;
    public event Action? OnChange;
    public void ToggleKanbanView()
    {
        KanbanViewToggle = !KanbanViewToggle;
        OnChange?.Invoke();
    }
}