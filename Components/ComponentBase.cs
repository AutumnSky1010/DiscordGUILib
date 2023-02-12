using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordGUILib.Components;
public abstract class ComponentBase
{
    public delegate Task ErrorEventHandler(SocketMessageComponent component);
}
