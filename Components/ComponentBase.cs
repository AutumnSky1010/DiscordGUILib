using Discord.WebSocket;

namespace DiscordGUILib.Components;
public abstract class ComponentBase
{
    public ComponentBase(ComponentId componentId)
    {
        this.ComponentId = componentId;
    }

    public delegate Task ErrorEventHandler(SocketMessageComponent component);

    public ComponentId ComponentId { get; }
}
