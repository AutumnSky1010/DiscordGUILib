using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordGUILib.Components;
public class ChainButtonState
{
    public ChainButtonState(ButtonDefinition buttonDefinition)
    {
        this.Definition = buttonDefinition;
    }

    public ButtonDefinition Definition { get; }

    internal ButtonPushedEventHandler<ChainButton>? PushedHandler;

    public event ButtonPushedEventHandler<ChainButton> Pushed { add => this.PushedHandler += value; remove => this.PushedHandler -= value; }
}
