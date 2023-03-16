using DiscordGUILib.Components;

namespace DiscordGUILib.Modules;
internal class ReceiverBase<T> where T : ComponentBase
{
    protected static Dictionary<ComponentId, T> IdComponentPairs { get; } = new();

    public static void Register(ComponentId id, T component)
    {
        TryRegister(id, component);
    }

    public static bool TryRegister(ComponentId id, T component)
    {
        if (IdComponentPairs.TryGetValue(id, out var value))
        {
            return false;
        }
        IdComponentPairs[id] = component;
        return true;
    }

    public static void Unregister(ComponentId id)
    {
        IdComponentPairs.Remove(id);
    }

    public static bool Exists(ComponentId id)
    {
        if (IdComponentPairs.TryGetValue(id, out _))
        {
            return true;
        }
        return false;
    }
}
