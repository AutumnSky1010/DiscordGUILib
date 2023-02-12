using Discord;
using Discord.WebSocket;
using DiscordGUILib.Modules;

namespace DiscordGUILib.Components;
public class PaginationMenu : ComponentBase
{
    public delegate Task SelectedEventHandler(SelectedEventArgs args);

    internal SelectedEventHandler? SelectedHandler;

    public delegate Task CanceledEventHandler(PaginationMenu paginationMenu, SocketMessageComponent component);

    internal CanceledEventHandler? CanceledHandler;

    internal static ErrorEventHandler? OnErrorHandler;

    public static event ErrorEventHandler OnError
    {
        add => OnErrorHandler += value;
        remove => OnErrorHandler -= value;
    }

    public event CanceledEventHandler OnCanceled
    {
        add => this.CanceledHandler += value;
        remove => this.CanceledHandler -= value;
    }

    public event SelectedEventHandler OnSelected
    {
        add => this.SelectedHandler += value;
        remove => this.SelectedHandler -= value;
    }

    public PaginationMenu(string componentId, IReadOnlyList<PaginationMenuItem> items)
    {
        this.ComponentId = componentId;
        this.MenuItems = items;
        PaginationMenuReceiver.Register(componentId, this);
        this.OnSelected += async (args) => { };
    }

    public string ComponentId { get; }

    public string CancelLabel { get; set; } = "cancel";

    public string CancelDescription { get; set; } = "";

    public string CancelId { get; set; } = "cancel";

    public bool HasCancelButton { get; set; } = true;

    public int CountPerPage { get; } = 10;

    public int PageCount
    {
        get
        {
            int count = this.MenuItems.Count / this.CountPerPage;
            if (this.MenuItems.Count % this.CountPerPage != 0)
            {
                count++;
            }
            return count;
        }
    }


    public IReadOnlyList<PaginationMenuItem> MenuItems { get; set; }

    public ComponentBuilder GetComponentBuilder(int index = 0)
    {
        index = index < 0 ? 0 : index;
        index = index > this.PageCount - 1 ? this.PageCount : index;

        var menuBuilder = new SelectMenuBuilder()
        {
            CustomId = GUILibCustomIdFactory.CreateNew<PaginationMenuReceiver>(
                PaginationMenuReceiver.ON_SELECTED_ID, 
                this.ComponentId)
                .ToString(),
        };

        int itemsStartIndex = index * this.CountPerPage;
        int currentIndex = itemsStartIndex;
        for (int i = 0; i < this.CountPerPage && this.MenuItems.Count > currentIndex; i++, currentIndex++)
        {
            var item = this.MenuItems[currentIndex];
            string? desctiption = item.Description.Length < 1 ? null : item.Description;
            menuBuilder.AddOption(item.Label, item.Id, desctiption);
        }

        string? description = this.CancelDescription.Length < 1 ? null : this.CancelDescription;
        if (this.HasCancelButton)
        {
            menuBuilder.AddOption(this.CancelLabel, this.CancelId, description);
        }

        return this.SetPaginationMenuComponent(index, menuBuilder);
    }

    public ComponentBuilder SetPaginationMenuComponent(int index, SelectMenuBuilder menuBuilder)
    {
        var builder = new ComponentBuilder();
        builder.WithSelectMenu(menuBuilder);
        if (index != 0)
        {
            builder.WithButton($"{index}", GUILibCustomIdFactory.CreateNew<PaginationMenuReceiver>(
                PaginationMenuReceiver.PREVIOUS_MENU_ID,
                this.ComponentId).ToString());
        }
        builder.WithButton(this.GetNowPageButton(index));
        if (index != this.PageCount - 1)
        {
            builder.WithButton($"{index + 2}", GUILibCustomIdFactory.CreateNew<PaginationMenuReceiver>(
                PaginationMenuReceiver.NEXT_MENU_ID,
                this.ComponentId).ToString());
        }
        return builder;
    }

    private ButtonBuilder GetNowPageButton(int index)
    {
        var button = new ButtonBuilder().WithLabel($"{Emoji.Parse(":notepad_spiral:")}{index + 1}/{this.PageCount}");
        button.IsDisabled = true;
        button.Style = ButtonStyle.Danger;
        button.CustomId = this.ComponentId;
        return button;
    }

    public bool TryGetItem(string id, out PaginationMenuItem? item)
    {
        var result = this.MenuItems.Where(item => item.Id == id).ToList();
        if (result.Count == 1)
        {
            item = result[0];
            return true;
        }
        item = null;
        return false;
    }
}

