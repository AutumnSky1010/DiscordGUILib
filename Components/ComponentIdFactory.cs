namespace DiscordGUILib.Components;
public static class ComponentIdFactory<T> where T : ComponentBase
{
    public static ComponentId CreateNew(string value)
    {
        if (TryCreateNew(value, out ComponentId id))
        {
            return id;
        }
        throw new ArgumentException();
    }

    public static bool TryCreateNew(string value, out ComponentId componentId)
    {
        if (CanCreate(value))
        {
            componentId = new ComponentId(value);
            return true;
        }
        componentId = new ComponentId("");
        return false;
    }

    public static bool CanCreate(string value)
    {
        return !ComponentBase.Exists<T>(new ComponentId(value));
    }

    public static ComponentId CreateFromGuid()
    {
        string value;
        do
        {
            value = Guid.NewGuid().ToString();

        } while (!CanCreate(value));
        return new ComponentId(value);
    }
}
