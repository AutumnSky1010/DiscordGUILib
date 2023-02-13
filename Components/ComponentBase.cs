using Discord.WebSocket;
using DiscordGUILib.Modules;

namespace DiscordGUILib.Components;
public abstract class ComponentBase
{
    public ComponentBase(ComponentId componentId)
    {
        this.ComponentId = componentId;
    }

    public delegate Task ErrorEventHandler(SocketMessageComponent component);

    public ComponentId ComponentId { get; }

    internal static bool Exists<T>(ComponentId componentId) where T : ComponentBase
    {
        return ReceiverBase<T>.Exists(componentId);
    }
}
