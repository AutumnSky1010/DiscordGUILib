using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordGUILib.Components;

public delegate Task ButtonPushedEventHandler<in T>(T button, SocketMessageComponent component);