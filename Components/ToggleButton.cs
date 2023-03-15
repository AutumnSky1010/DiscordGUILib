using Discord;
using Discord.WebSocket;
using DiscordGUILib.Modules;

namespace DiscordGUILib.Components;
public class ToggleButton : ComponentBase
{
    public ToggleButton(ComponentId componentId, ButtonDefinition @checked, ButtonDefinition unChecked, bool isChecked = false) : base(componentId)
    {
        IToggleButtonState state = isChecked ? new CheckedState(@checked, unChecked) : new UncheckedState(unChecked, @checked);
        this.State = state;
        ToggleButtonReceiver.Register(componentId, this);
    }

    internal static ErrorEventHandler? OnErrorHandler;

    public static event ErrorEventHandler OnError
    {
        add => OnErrorHandler += value;
        remove => OnErrorHandler -= value;
    }

    internal ButtonPushedEventHandler<ToggleButton>? CheckedHandler;

    internal ButtonPushedEventHandler<ToggleButton>? UncheckedHandler;

    public event ButtonPushedEventHandler<ToggleButton> Checked
    {
        add => this.CheckedHandler += value;
        remove => this.CheckedHandler -= value;
    }

    public event ButtonPushedEventHandler<ToggleButton> Unchecked
    {
        add => this.UncheckedHandler += value;
        remove => this.UncheckedHandler -= value;
    }

    public ButtonDefinition Current { get => this.State.Definition; }

    public ButtonDefinition NextDefinition { get => this.State.Next.Definition; }

    public ButtonDefinition CurrentDefinition { get => this.State.Definition; }

    private IToggleButtonState State { get; set; }

    public bool IsChecked => this.State.IsChecked;

    public void Toggle()
    {
        this.State = this.State.Next;
    }

    public void SetButtonDefinitions(ButtonDefinition @checked, ButtonDefinition unChecked)
    {
        IToggleButtonState state = this.IsChecked ? new CheckedState(@checked, unChecked) : new UncheckedState(unChecked, @checked);
        this.State = state;
    }

    public ComponentBuilder GetComponentBuilder()
    {
        // isChecked = trueのとき、チェックを外すので、UNCHECKED_IDを用いる。
        var receiverName = this.IsChecked ? ToggleButtonReceiver.UNCHECKED_ID : ToggleButtonReceiver.CHECKED_ID;
        var customId = GUILibCustomIdFactory.CreateNew<ToggleButtonReceiver>(receiverName, this.ComponentId);
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

    private interface IToggleButtonState
    {
        ButtonDefinition Definition { get; }

        bool IsChecked { get; }

        IToggleButtonState Next { get; }
    }

    private class CheckedState : IToggleButtonState
    {
        public CheckedState(ButtonDefinition current, ButtonDefinition next)
        {
            this.Definition = current;
            this.NextDefinition = next;
        }

        public ButtonDefinition Definition { get; }

        public bool IsChecked => true;

        private ButtonDefinition NextDefinition { get; }

        public IToggleButtonState Next
        {
            get => new UncheckedState(this.NextDefinition, this.Definition);
        }

    }

    private class UncheckedState : IToggleButtonState
    {
        public UncheckedState(ButtonDefinition current, ButtonDefinition next)
        {
            this.Definition = current;
            this.NextDefinition = next;
        }

        public ButtonDefinition Definition { get; }

        private ButtonDefinition NextDefinition { get; }

        public bool IsChecked => false;

        public IToggleButtonState Next
        {
            get => new CheckedState(this.NextDefinition, this.Definition);
        }
    }
}
