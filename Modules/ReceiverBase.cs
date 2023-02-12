using DiscordGUILib.Components;

namespace DiscordGUILib.Modules;
internal class ReceiverBase<T>
{
    protected static Dictionary<ComponentId, T> IdComponentPairs { get; } = new();

    internal static void Register(ComponentId id, T component)
    {
        TryRegister(id, component);
    }

    internal static bool TryRegister(ComponentId id, T component)
    {
        if (IdComponentPairs.TryGetValue(id, out var value))
        {
            return false;
        }
        IdComponentPairs[id] = component;
        return true;
    }

    protected static void Unregister(ComponentId id)
    {
        IdComponentPairs.Remove(id);
    }
}
