using Discord.WebSocket;
using DiscordGUILib.Components;
using DiscordGUILib.Modules.Attributes;

namespace DiscordGUILib.Modules;

[ComponentModule("toggleButton")]
internal class ToggleButtonReceiver : ReceiverBase<ToggleButton>
{
    public const string CHECKED_ID = "checked";

    public const string UNCHECKED_ID = "unchecked";

    [ComponentReceiver(CHECKED_ID)]
    public async Task Checked(SocketMessageComponent component)
    {
        var buttonNullable = await ToggleAsync(component.Data.CustomId, component);
        if (buttonNullable is not null &&
            buttonNullable.CheckedHandler is not null)
        {
            await buttonNullable.CheckedHandler(buttonNullable, component);
        }
    }

    [ComponentReceiver(UNCHECKED_ID)]
    public async Task Unchecked(SocketMessageComponent component)
    {
        var buttonNullable = await ToggleAsync(component.Data.CustomId, component);
        if (buttonNullable is not null &&
            buttonNullable.UncheckedHandler is not null)
        {
            await buttonNullable.UncheckedHandler(buttonNullable, component);
        }
    }

    private async Task<ToggleButton?> ToggleAsync(string customId, SocketMessageComponent component)
    {
        var id = GUILibCustomIdFactory.CreateFromString(customId);
        if (!IdComponentPairs.TryGetValue(id.ComponentId, out ToggleButton? button) ||
            button is null)
        {
            if (ToggleButton.OnErrorHandler is not null) { await ToggleButton.OnErrorHandler(component); }
            return null;
        }
        button.Toggle();
        await component.Channel.ModifyMessageAsync(component.Message.Id, msg => msg.Components = button.GetComponentBuilder().Build());
        return button;
    }
}
