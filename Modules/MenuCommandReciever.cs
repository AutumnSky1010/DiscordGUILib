using Discord.Interactions;
using Discord.WebSocket;
using Discord;
using DiscordGUILib.Components;
namespace DiscordGUILib.Modules;
public class MenuCommandReciever : InteractionModuleBase
{
    private static Dictionary<string, PaginationMenu> IdMenuPairs { get; } = new();

    public const string NEXT_MENU_ID = "MenuCommandReciever-28268999-c007-4fdb-9dd7-42f269fac4aa";

    public const string PREVIOUS_MENU_ID = "MenuCommandReciever-535a0e30-611a-4308-a501-7ca238a56be2";

    public const string ON_SELECTED_ID = "MenuCommandReciever-37e46ee2-e5b2-4250-ac21-a243924e37b3";

    [ComponentInteraction(NEXT_MENU_ID)]
    public async Task OnClickNext() => await this.ChangeQuizMenuPage(NEXT_MENU_ID);

    [ComponentInteraction(PREVIOUS_MENU_ID)]
    public async Task OnClickPrevious() => await this.ChangeQuizMenuPage(PREVIOUS_MENU_ID);

    [ComponentInteraction(ON_SELECTED_ID)]
    public async Task OnSelected(string[] selectedValues)
    {
        string id = selectedValues[0];
        if (!TryGetClickedMenu(out var menuNullable) || menuNullable is null ||
            !menuNullable.TryGetItem(id, out var itemNullable) || itemNullable is null)
        {
            if (PaginationMenu.OnErrorHandler is not null)
            {
                await PaginationMenu.OnErrorHandler(this.Context);
            }
            return;
        }

        if (menuNullable.CanceledHandler is not null && id == menuNullable.CancelId)
        {
            await menuNullable.CanceledHandler(menuNullable, this.Context);
            Unregister(menuNullable.Id);
            await DeleteOriginalResponseAsync();
        }

        var args = new SelectedEventArgs(menuNullable, itemNullable, this.Context);
        if (menuNullable.SelectedHandler is not null)
        {
            await menuNullable.SelectedHandler(args);
        }
    }

    private async Task ChangeQuizMenuPage(string buttonCustomId)
    {
        var component = (SocketMessageComponent)this.Context.Interaction;
        string buttonLabel = "";
        foreach (ActionRowComponent rowComponent in component.Message.Components)
        {
            foreach (IMessageComponent messageComponent in rowComponent.Components)
            {
                if (messageComponent.Type is ComponentType.Button)
                {
                    var button = (ButtonComponent)messageComponent;
                    if (messageComponent.CustomId == buttonCustomId)
                    {
                        buttonLabel = button.Label;
                    }
                }
            }
        }

        if (!this.TryGetClickedMenu(out var menuNullable) || menuNullable is null)
        {
            return;
        }

        await this.DeferAsync();
        await component.Message.ModifyAsync(msg => {
            msg.Components = menuNullable.GetComponentBuilder(int.Parse(buttonLabel) - 1).Build();
            msg.Content = component.Message.Content;
        });
    }

    public static void Register(string id, PaginationMenu menu)
    {
        if (IdMenuPairs.TryGetValue(id, out var value))
        {
            return;
        }
        IdMenuPairs[id] = menu;
    }

    public static void Unregister(string id)
    {
        IdMenuPairs.Remove(id);
    }

    private bool TryGetClickedMenu(out PaginationMenu? menu)
    {
        var component = (SocketMessageComponent)this.Context.Interaction;
        string menuId = "";
        foreach (ActionRowComponent rowComponent in component.Message.Components)
        {
            foreach (IMessageComponent messageComponent in rowComponent.Components)
            {
                if (messageComponent.Type is ComponentType.Button)
                {
                    var button = (ButtonComponent)messageComponent;
                    if (button.IsDisabled)
                    {
                        menuId = button.CustomId;
                    }
                }
            }
        }
        var result = IdMenuPairs.TryGetValue(menuId, out menu);
        return result;
    }
}

