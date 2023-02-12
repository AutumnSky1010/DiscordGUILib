using Discord;

namespace DiscordGUILib.Components;
public class ButtonDefinition
{
    public ButtonDefinition(
        string? label = null,
        ButtonStyle style = ButtonStyle.Primary,
        string? url = null,
        IEmote? emote = null,
        bool isDisabled = false)
    {
        this.Label = label;
        this.Style = style;
        this.Url = url;
        this.Emote = emote;
        this.IsDisabled = isDisabled;
    }

    public string? Label { get; set; }

    public ButtonStyle Style { get; set; }

    public string? Url { get; set; }

    public IEmote? Emote { get; set; }

    public bool IsDisabled { get; set; }
}
