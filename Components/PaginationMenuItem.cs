namespace DiscordGUILib.Components;
public class PaginationMenuItem
{
    public PaginationMenuItem(string label, object value, string desctiption = "")
    {
        this.Label = label;
        this.Value = value;
        this.Description = "";
        this.Id = $"{label}_{Guid.NewGuid()}";
    }

    public string Label { get; init; }

    public object Value { get; init; }

    public string Id { get; init; }

    public string Description { get; set; } = "";
}
