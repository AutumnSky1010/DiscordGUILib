using Discord.Interactions;
using DiscordGUILib.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordGUILib.Modules;
public class ReceiverBase<T>
{
    protected static Dictionary<string, T> IdComponentPairs { get; } = new();

    internal static void Register(string id, T component)
    {
        TryRegister(id, component);
    }

    internal static bool TryRegister(string id, T component)
    {
        if (IdComponentPairs.TryGetValue(id, out var value))
        {
            return false;
        }
        IdComponentPairs[id] = component;
        return true;
    }

    protected static void Unregister(string id)
    {
        IdComponentPairs.Remove(id);
    }
}
