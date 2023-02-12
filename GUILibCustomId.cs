using DiscordGUILib.Components;

namespace DiscordGUILib;
public class GUILibCustomId
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="moduleName"></param>
    /// <param name="receiverName"></param>
    /// <param name="componentId"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public GUILibCustomId(string moduleName, string receiverName, ComponentId componentId)
    {

        this.ModuleName = moduleName;
        this.ReceiverName = receiverName;
        this.ComponentId = componentId;
        if (ToString().Length > 100)
        {
            throw new ArgumentOutOfRangeException();
        }
    }

    public string ModuleName { get; }

    public string ReceiverName { get; }

    public ComponentId ComponentId { get; }

    public override string ToString()
    {
        return $"{this.ModuleName}-{this.ReceiverName}-{this.ComponentId}";
    }
}
