using Discord.WebSocket;

namespace DiscordGUILib.Components;
public class SelectedEventArgs
{
    public SelectedEventArgs(PaginationMenu menu, PaginationMenuItem selected, SocketMessageComponent component)
    {
        this.SelectedItem = selected;
        this.Component = component;
        this.PaginationMenu = menu;
    }

    public PaginationMenuItem SelectedItem { get; }

    public PaginationMenu PaginationMenu { get; }

    public SocketMessageComponent Component { get; }
}