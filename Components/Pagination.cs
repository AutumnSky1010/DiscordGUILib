using Discord;
using Discord.WebSocket;
using DiscordGUILib.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordGUILib.Components;
public class Pagination : ComponentBase
{
    public Pagination(ComponentId componentId, int maxPage) : base(componentId)
    {
        this.MaxPage = maxPage;
        PaginationReceiver.Register(componentId, this);
    }

    public delegate Task PageChangedEventHandler(Pagination pagination, SocketMessageComponent component);

    internal PageChangedEventHandler? PageChangedHandler;

    public event PageChangedEventHandler PageChanged { add => this.PageChangedHandler += value; remove => this.PageChangedHandler -= value; }

    internal static ErrorEventHandler? OnErrorHandler;

    public static event ErrorEventHandler OnError
    {
        add => OnErrorHandler += value;
        remove => OnErrorHandler -= value;
    }

    public int MaxPage { get; set; }

    public int CurrentPage { get; private set; } = 1;

    public void Next()
    {
        if (this.MaxPage == this.CurrentPage)
        {
            return;
        }
        this.CurrentPage++;
    }

    public void Previous()
    {
        if (this.CurrentPage == 1)
        {
            return;
        }
        this.CurrentPage--;
    }

    internal override ComponentBuilder AddComponentTo(ComponentBuilder componentBuilder)
    {
        if (this.CurrentPage != 1)
        {
            componentBuilder.WithButton($"{this.CurrentPage - 1}", GUILibCustomIdFactory.CreateNew<PaginationReceiver>(
                PaginationReceiver.PREV,
                this.ComponentId).ToString());
        }
        componentBuilder.WithButton(GetNowPageButton());
        if (this.CurrentPage != this.MaxPage)
        {
            componentBuilder.WithButton($"{this.CurrentPage + 1}", GUILibCustomIdFactory.CreateNew<PaginationReceiver>(
                PaginationReceiver.NEXT,
                this.ComponentId).ToString());
        }
        return componentBuilder;
    }

    protected override void Unregister()
    {
        PaginationReceiver.Unregister(this.ComponentId);
    }

    private ButtonBuilder GetNowPageButton()
    {
        var button = new ButtonBuilder().WithLabel($"{Emoji.Parse(":notepad_spiral:")}{this.CurrentPage}/{this.MaxPage}");
        button.IsDisabled = true;
        button.Style = ButtonStyle.Danger;
        button.CustomId = this.ComponentId.ToString();
        return button;
    }
}
