namespace DiscordGUILib.Components;
public record ComponentId
{
    internal ComponentId(string value)
    {
        if (value.Length > 50)
        {
            throw new ArgumentOutOfRangeException();
        }
        this.Value = value;
    }

    private string Value { get; }

    public override string ToString()
    {
        return this.Value;
    }
}
