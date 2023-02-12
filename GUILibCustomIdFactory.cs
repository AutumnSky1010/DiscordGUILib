using DiscordGUILib.Modules.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
        string componentId = id.Replace($"{moduleName}-{receiverName}-", "");
        return new GUILibCustomId(moduleName, receiverName, componentId);
    }

    public static GUILibCustomId CreateNew<T>(string receiverName, string componentId)
    {
        var attribute = typeof(T).GetCustomAttribute<ComponentModuleAttribute>();
        if (attribute is null)
        {
            throw new ArgumentException();
        }
        return new GUILibCustomId(attribute.Name, receiverName, componentId);
    }
}
