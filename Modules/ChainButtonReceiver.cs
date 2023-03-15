using Discord.WebSocket;
using DiscordGUILib.Components;
using DiscordGUILib.Modules.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordGUILib.Modules;

[ComponentModule("chainButton")]
internal class ChainButtonReceiver : ReceiverBase<ChainButton>
{
    public const string PUSHED = "PUSHED";

    [ComponentReceiver(PUSHED)]
    public async Task Pushed(SocketMessageComponent component)
    {
        var id = GUILibCustomIdFactory.CreateFromString(component.Data.CustomId);
        if (!IdComponentPairs.TryGetValue(id.ComponentId, out ChainButton? button) ||
            button is null)
        {
            if (ChainButton.OnErrorHandler is not null) { await ChainButton.OnErrorHandler(component); }
            return;
        }

        if (button.CurrentState.PushedHandler is not null)
        {
            await button.CurrentState.PushedHandler(button, component);
        }

        button.Push();
        await component.Channel.ModifyMessageAsync(component.Message.Id, msg => msg.Components = button.GetComponentBuilder().Build());
    }
}
