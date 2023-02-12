using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordGUILib.Modules.Attributes;

[AttributeUsage(AttributeTargets.Method)]
internal class ComponentReceiverAttribute : Attribute
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="name">max length is 10.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public ComponentReceiverAttribute(string name)
    {
        if (name.Length > 10)
        {
            throw new ArgumentOutOfRangeException();
        }
        this.Name = name;
    }

    public string Name { get; }
}
