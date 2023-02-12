using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordGUILib.Modules;
using DiscordGUILib.Modules.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscordGUILib;
public class GUIModuleInitializer
{
    static GUIModuleInitializer()
    {
        if (Assembly.GetAssembly(typeof(GUIModuleInitializer)) is not Assembly assembly)
        {
            return;
        }
        foreach (var type in assembly.GetTypes())
        {
            Attribute? attributeNullable = type.GetCustomAttribute<ComponentModuleAttribute>();
            if (attributeNullable is ComponentModuleAttribute moduleAttribute)
            {
                NameModulePairs[moduleAttribute.Name] = type;
            }
        }
    }

    private static Dictionary<string, Type> NameModulePairs { get; } = new();

    public static void Initialize(DiscordSocketClient client, IServiceProvider serviceProvider)
    {
        client.ButtonExecuted += Executed;
        client.SelectMenuExecuted += Executed;
    }

    private static async Task Executed(SocketMessageComponent arg)
    {
        var id = GUILibCustomIdFactory.CreateFromString(arg.Data.CustomId);
        if (!NameModulePairs.TryGetValue(id.ModuleName, out Type? moduleType) || 
            moduleType is null)
        {
            return;
        }
        var methods = moduleType.GetMethods();
        foreach (var method in methods)
        {
            Attribute? attribute = method.GetCustomAttribute<ComponentReceiverAttribute>();
            if (attribute is ComponentReceiverAttribute receiver &&
                receiver.Name == id.ReceiverName)
            {
                var instance = Activator.CreateInstance(moduleType);
                if (method.Invoke(instance, new object[] { arg }) is Task task)
                {
                    await task;
                }
                break;
            }
        }
    }
}
