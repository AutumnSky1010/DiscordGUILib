using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordGUILib.Components;
internal interface IToggleButtonState
{
    ButtonDefinition Definition { get; }

    bool IsChecked { get; }

    IToggleButtonState Next { get; }
}
