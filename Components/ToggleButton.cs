using Discord;
using Discord.WebSocket;
using DiscordGUILib.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordGUILib.Components;
public class ToggleButton : ComponentBase
{
    public ToggleButton(string id, ButtonDefinition @checked, ButtonDefinition unChecked, bool isChecked = false) 
    {
        IToggleButtonState state = isChecked ? new CheckedState(@checked, unChecked) : new UncheckedState(unChecked, @checked);
        this.State = state;
        this.Id = id;
        ToggleButtonReceiver.Register(id, this);
    }

    internal static ErrorEventHandler? OnErrorHandler;

    public static event ErrorEventHandler OnError
    {
        add => OnErrorHandler += value;
        remove => OnErrorHandler -= value;
    }

    public delegate Task ToggledHandler(ToggleButton button, SocketMessageComponent component);

    internal ToggledHandler? CheckedHandler;

    internal ToggledHandler? UncheckedHandler;

    public event ToggledHandler Checked
    {
        add => this.CheckedHandler += value;
        remove => this.CheckedHandler -= value;
    }

    public event ToggledHandler Unchecked
    {
        add => this.UncheckedHandler += value;
        remove => this.UncheckedHandler -= value;
    }

    private IToggleButtonState State { get; set; }

    public bool IsChecked => State.IsChecked;

    public string Id { get; }

    public void Toggle()
    {
        this.State = this.State.Next;
    }

    
    public ComponentBuilder GetComponentBuilder()
    {
        var customId = GUILibCustomIdFactory.CreateNew<ToggleButtonReceiver>(ToggleButtonReceiver.CHECKED_ID, this.Id);
        var currentDefinition = this.State.Definition;
        var buttonBuilder = new ButtonBuilder()
            .WithLabel(currentDefinition.Label)
            .WithCustomId(customId.ToString())
            .WithStyle(currentDefinition.Style)
            .WithUrl(currentDefinition.Url)
            .WithEmote(currentDefinition.Emote)
            .WithDisabled(currentDefinition.IsDisabled);
        ComponentBuilder builder = new ComponentBuilder().WithButton(buttonBuilder);
        return builder;
    }
}
