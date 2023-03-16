using Discord;
using Discord.WebSocket;
using DiscordGUILib.Components;
using DiscordGUILib.Modules.Attributes;

namespace DiscordGUILib.Modules;

[ComponentModule("pagination_menu")]
internal class PaginationMenuReceiver : ReceiverBase<PaginationMenu>
{
    public const string ON_SELECTED_ID = "on_select";

    [ComponentReceiver(ON_SELECTED_ID)]
    public async Task OnSelected(SocketMessageComponent component)
    {
        string id = string.Join(',', component.Data.Values);
        
        if (!TryGetClickedMenu(component, out var menuNullable) || menuNullable is null)
        {
            await OnErrorAsync(component);
            return;
        }

        if (id == menuNullable.CancelId)
        {
            if (menuNullable.CanceledHandler is not null)
            {
                await menuNullable.CanceledHandler(menuNullable, component);
            }
            Unregister(menuNullable.ComponentId);
            PaginationReceiver.Unregister(menuNullable.ComponentId);
            await component.Channel.DeleteMessageAsync(component.Message);
            return;
        }

        if (!menuNullable.TryGetItem(id, out var itemNullable) || itemNullable is null)
        {
            await OnErrorAsync(component);
            return;
        }

        var args = new SelectedEventArgs(menuNullable, itemNullable, component);
        if (menuNullable.SelectedHandler is not null)
        {
            await menuNullable.SelectedHandler(args);
        }
    }

    private async Task OnErrorAsync(SocketMessageComponent component)
    {
        if (PaginationMenu.OnErrorHandler is not null)
        {
            await PaginationMenu.OnErrorHandler(component);
        }
    }

    private bool TryGetClickedMenu(SocketMessageComponent component, out PaginationMenu? menu)
    {
        var id = GUILibCustomIdFactory.CreateFromString(component.Data.CustomId);
        var result = IdComponentPairs.TryGetValue(id.ComponentId, out menu);
        return result;
    }
}

