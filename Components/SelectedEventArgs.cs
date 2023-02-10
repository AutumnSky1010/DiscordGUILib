using Discord;

namespace DiscordGUILib.Components;
public class SelectedEventArgs
{
    public SelectedEventArgs(PaginationMenu menu, PaginationMenuItem selected, IInteractionContext context)
    {
        this.SelectedItem = selected;
        this.Context = context;
        this.PaginationMenu = menu;
    }

    public PaginationMenuItem SelectedItem { get; }

    public PaginationMenu PaginationMenu { get; }

    public IInteractionContext Context { get; }
}