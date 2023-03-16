using Discord.WebSocket;
using DiscordGUILib.Components;
using DiscordGUILib.Modules.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordGUILib.Modules;

[ComponentModule("pagination")]
internal class PaginationReceiver : ReceiverBase<Pagination>
{
    public const string NEXT = "next";

    public const string PREV = "prev";

    public const string DUMMY = "current";

    [ComponentReceiver(DUMMY)]
    public async Task Current(SocketMessageComponent component)
    {
        await component.DeferAsync();
    }

    [ComponentReceiver(NEXT)]
    public async Task Next(SocketMessageComponent component)
    {
        var id = GUILibCustomIdFactory.CreateFromString(component.Data.CustomId);
        if (!IdComponentPairs.TryGetValue(id.ComponentId, out Pagination? pagination) ||
            pagination is null)
        {
            if (Pagination.OnErrorHandler is not null) { await Pagination.OnErrorHandler(component); }
            return;
        }

        pagination.Next();
        await component.Channel.ModifyMessageAsync(component.Message.Id, msg => msg.Components = pagination.GetComponentBuilder().Build());

        if (pagination.PageChangedHandler is not null)
        {
            await pagination.PageChangedHandler(pagination, component);
        }
    }

    [ComponentReceiver(PREV)]
    public async Task Prev(SocketMessageComponent component)
    {
        var id = GUILibCustomIdFactory.CreateFromString(component.Data.CustomId);
        if (!IdComponentPairs.TryGetValue(id.ComponentId, out Pagination? pagination) ||
            pagination is null)
        {
            if (Pagination.OnErrorHandler is not null) { await Pagination.OnErrorHandler(component); }
            return;
        }

        pagination.Previous();
        await component.Channel.ModifyMessageAsync(component.Message.Id, msg => msg.Components = pagination.GetComponentBuilder().Build());
        if (pagination.PageChangedHandler is not null)
        {
            await pagination.PageChangedHandler(pagination, component);
        }
    }
}
