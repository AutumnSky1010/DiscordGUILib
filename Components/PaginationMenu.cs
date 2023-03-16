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

    public PaginationMenu(ComponentId componentId, IReadOnlyList<PaginationMenuItem> items) : base(componentId)
    {
        this.MenuItems = items;
        OnSelected += async (args) => { };
        this.Pagination = new Pagination(componentId, this.PageCount);
        this.Pagination.PageChanged += async (pagination, component) =>
        {
            var newComponents = GetComponentBuilder().Build();
            await component.Channel.ModifyMessageAsync(component.Message.Id, msg => msg.Components = newComponents);
            await component.DeferAsync();
        };
        PaginationMenuReceiver.Register(componentId, this);
    }

    private Pagination Pagination { get; }

    public string CancelLabel { get; set; } = "cancel";

    public string CancelDescription { get; set; } = "";

    public string CancelId { get; set; } = "cancel";

    public bool HasCancelButton { get; set; } = true;

    public int CountPerPage { get; } = 10;

    public int CurrentPage { get => this.Pagination.CurrentPage; }

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

   internal override ComponentBuilder AddComponentTo(ComponentBuilder componentBuilder)
    {
        var menuBuilder = new SelectMenuBuilder()
        {
            CustomId = GUILibCustomIdFactory.CreateNew<PaginationMenuReceiver>(
                PaginationMenuReceiver.ON_SELECTED_ID,
                this.ComponentId)
                .ToString(),
        };

        // インデックスは０スタートなので、現在ページ - 1
        int itemsStartIndex = (this.Pagination.CurrentPage - 1) * this.CountPerPage;
        int currentIndex = itemsStartIndex;
        for (int i = 0; i < this.CountPerPage && this.MenuItems.Count > currentIndex; i++, currentIndex++)
        {
            var item = this.MenuItems[currentIndex];
            string? desctiption = item.Description.Length == 0 ? null : item.Description;
            menuBuilder.AddOption(item.Label, item.Id, desctiption);
        }

        string? description = this.CancelDescription.Length < 1 ? null : this.CancelDescription;
        if (this.HasCancelButton)
        {
            menuBuilder.AddOption(this.CancelLabel, this.CancelId, description);
        }
        componentBuilder.WithSelectMenu(menuBuilder);
        return this.Pagination.AddComponentTo(componentBuilder);
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

    protected override void Unregister()
    {
        PaginationMenuReceiver.Unregister(this.ComponentId);
    }
}

