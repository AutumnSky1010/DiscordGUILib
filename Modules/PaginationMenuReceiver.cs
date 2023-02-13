using Discord;
using Discord.WebSocket;
using DiscordGUILib.Components;
using DiscordGUILib.Modules.Attributes;

namespace DiscordGUILib.Modules;

[ComponentModule("pagination")]
internal class PaginationMenuReceiver : ReceiverBase<PaginationMenu>
{

    public const string NEXT_MENU_ID = "next";

    public const string PREVIOUS_MENU_ID = "prev";

    public const string ON_SELECTED_ID = "on_select";


    [ComponentReceiver(NEXT_MENU_ID)]
    public async Task OnClickNext(SocketMessageComponent component)
        => await ChangeQuizMenuPage(component);

    [ComponentReceiver(PREVIOUS_MENU_ID)]
    public async Task OnClickPrevious(SocketMessageComponent component)
        => await ChangeQuizMenuPage(component);

    [ComponentReceiver(ON_SELECTED_ID)]
    public async Task OnSelected(SocketMessageComponent component)
    {
        string id = string.Join(',', component.Data.Values);
        if (!TryGetClickedMenu(component, out var menuNullable) || menuNullable is null)
        {
            await OnErrorAsync(component);
            return;
        }

        if (menuNullable.CanceledHandler is not null && id == menuNullable.CancelId)
        {
            await menuNullable.CanceledHandler(menuNullable, component);
            Unregister(menuNullable.ComponentId);
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

    private async Task ChangeQuizMenuPage(SocketMessageComponent component)
    {
        string buttonLabel = "";
        foreach (ActionRowComponent rowComponent in component.Message.Components)
        {
            foreach (IMessageComponent messageComponent in rowComponent.Components)
            {
                if (messageComponent.Type is ComponentType.Button)
                {
                    var button = (ButtonComponent)messageComponent;
                    if (messageComponent.CustomId == component.Data.CustomId)
                    {
                        buttonLabel = button.Label;
                    }
                }
            }
        }

        if (!TryGetClickedMenu(component, out var menuNullable) || menuNullable is null)
        {
            return;
        }
        await component.Message.ModifyAsync(msg =>
        {
            msg.Components = menuNullable.GetComponentBuilder(int.Parse(buttonLabel) - 1).Build();
            msg.Content = component.Message.Content;
        });
        await component.DeferAsync();
    }

    private bool TryGetClickedMenu(SocketMessageComponent component, out PaginationMenu? menu)
    {
        var id = GUILibCustomIdFactory.CreateFromString(component.Data.CustomId);
        var result = IdComponentPairs.TryGetValue(id.ComponentId, out menu);
        return result;
    }
}

