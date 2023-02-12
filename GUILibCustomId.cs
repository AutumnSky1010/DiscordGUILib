using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public GUILibCustomId(string moduleName, string receiverName, string componentId)
    {

        this.ModuleName = moduleName;
        this.ReceiverName = receiverName;
        this.ComponentId = componentId;
        if (this.ToString().Length > 100)
        {
            throw new ArgumentOutOfRangeException();
        }
    }

    public string ModuleName { get; }

    public string ReceiverName { get; }

    public string ComponentId { get; }

    public override string ToString()
    {
        return $"{ModuleName}-{ReceiverName}-{ComponentId}";
    }
}
