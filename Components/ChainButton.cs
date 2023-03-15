using Discord;
using DiscordGUILib.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordGUILib.Components;
public class ChainButton : ComponentBase
{
    public ChainButton(ComponentId componentId, IReadOnlyList<ChainButtonState> states) : base(componentId)
    {
        if (states.Count == 0)
        {
            throw new ArgumentException("states is empty.");
        }

        this.States = new List<ChainButtonState>(states);
        ChainButtonReceiver.Register(componentId, this);
    }

    internal static ErrorEventHandler? OnErrorHandler;

    public static event ErrorEventHandler OnError
    {
        add => OnErrorHandler += value;
        remove => OnErrorHandler -= value;
    }

    private List<ChainButtonState> States { get; }

    private int CurrentIndex { get; set; } = 0;

    public ChainButtonState CurrentState
    {
        get => this.States[this.CurrentIndex];
    }

    public ComponentBuilder GetComponentBuilder()
    {
        var receiverName = ChainButtonReceiver.PUSHED;
        var customId = GUILibCustomIdFactory.CreateNew<ChainButtonReceiver>(receiverName, this.ComponentId);
        var currentDefinition = this.CurrentState.Definition;
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

    public void Push()
    {
        if (this.CurrentIndex + 1 == this.States.Count)
        {
            this.CurrentIndex = 0;
            return;
        }
        this.CurrentIndex++;
    }
}
