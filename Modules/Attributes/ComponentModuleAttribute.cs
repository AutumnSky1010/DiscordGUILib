namespace DiscordGUILib.Modules.Attributes;
[AttributeUsage(AttributeTargets.Class)]
internal class ComponentModuleAttribute : Attribute
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="name">max length is 20</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public ComponentModuleAttribute(string name)
    {
        if (name.Length > 20)
        {
            throw new ArgumentOutOfRangeException();
        }
        this.Name = name;
    }

    public string Name { get; }
}
