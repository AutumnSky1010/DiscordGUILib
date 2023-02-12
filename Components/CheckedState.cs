using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordGUILib.Components;
internal class CheckedState : IToggleButtonState
{
    public CheckedState(ButtonDefinition current, ButtonDefinition next)
    {
        this.Definition = current;
        this.NextDefinition= next;
    }

    public ButtonDefinition Definition { get; }

    public bool IsChecked => true;

    private ButtonDefinition NextDefinition { get; }

    public IToggleButtonState Next
    {
        get => new UncheckedState(this.NextDefinition, this.Definition);
    }
}
