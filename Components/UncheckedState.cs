using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordGUILib.Components;
internal class UncheckedState : IToggleButtonState
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
