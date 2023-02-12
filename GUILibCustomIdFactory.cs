using DiscordGUILib.Components;
using DiscordGUILib.Modules.Attributes;
using System.Reflection;

namespace DiscordGUILib;
internal class GUILibCustomIdFactory
{
    public static GUILibCustomId CreateFromString(string id)
    {
        string[] components = id.Split('-');
        if (components.Length < 3)
        {
            throw new FormatException();
        }
        string moduleName = components[0];
        string receiverName = components[1];
        var componentId = new ComponentId(id.Replace($"{moduleName}-{receiverName}-", ""));
        return new GUILibCustomId(moduleName, receiverName, componentId);
    }

    public static GUILibCustomId CreateNew<T>(string receiverName, ComponentId componentId)
    {
        var attribute = typeof(T).GetCustomAttribute<ComponentModuleAttribute>();
        if (attribute is null)
        {
            throw new ArgumentException();
        }
        return new GUILibCustomId(attribute.Name, receiverName, componentId);
    }
}
