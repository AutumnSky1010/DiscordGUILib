# DiscordGUILib

## 📔Overview
This supports interactive UI with using the message component.  

## 📑Documentation
In preparation.

## ⛰️Requirement
- .NET 6
- Discord.Net 3.9.0

## ⏬Installation
### NuGet

[DiscordGUILib](https://www.nuget.org/packages/DiscordGUILib/)

## 📱Usage

### Premise
Preparing to receive command with the use of Discord.Net.

### Initialize DiscordGUILib
You need to pass the client to `GUIModuleInitializer`.  
Will be becomes possible use DiscordGUILib.
```cs
// using Discord.WebSocket;
// using DiscordGUILib;

private void InitializeGUIModule(BaseSocketClient client)
{
    GUIModuleInitializer.Initialize(client);
}
```

### How to use components.
Give an examples of how to use components.  
This example is used interaction framework.
#### Toggle button
![image](https://user-images.githubusercontent.com/66455966/218487397-5326f98f-ad75-473e-9790-2f16139330ea.png)
```cs
using Discord.Interactions;
using DiscordGUILib.Components;

namespace Examples;

public class ToggleModule : InteractionModuleBase 
{
    [SlashCommand("toggle", "send the toggle button")]
    public async Task ToggleTest()
    {
        var checkedDef = new ButtonDefinition("Checked!!", ButtonStyle.Success, emote: Emoji.Parse(":ballot_box_with_check:"));
        var unCheckedDef = new ButtonDefinition("Unchecked", ButtonStyle.Danger, emote: Emoji.Parse(":blue_square:"));
        // Create the component id.
        var componentId = ComponentIdFactory<ToggleButton>.CreateFromGuid();
        // Or use, `var componentId = ComponentIdFactory<ToggleButton>.CreateNew("any string(max length is 50.)");`

        // Create the instance of toggle button.
        var toggleButton = new ToggleButton(componentId, checkedDef, unCheckedDef);

        // The event if button was checked by user.
        toggleButton.Checked += async (button, component) =>
        {
            await component.RespondAsync("Changed to checked.");
        };
        // The event if button was unchecked by user.
        toggleButton.Unchecked += async (button, component) =>
        {
            await ReplyAsync(component.Data.CustomId);
            await component.RespondAsync("Changed to unchecked.");
        };
        await RespondAsync("toggle button", components: toggleButton.GetComponentBuilder().Build());
    }
}
```
#### Pagination Menu
![image2](https://user-images.githubusercontent.com/66455966/218487504-4ff6ee79-bf18-4b3f-87da-a4979e5fc064.png)
```cs
using Discord.Interactions;
using DiscordGUILib.Components;

namespace Examples;

public class PaginationMenuModule : InteractionModuleBase 
{
    [SlashCommand("menu", "send the pagination menu.")]
    public async Task MenuTest()
    {
        // Create pagination items.
        var items = new List<PaginationMenuItem>();
        for (int i = 1; i <= 100; i++)
        {
            items.Add(new PaginationMenuItem($"label{i}", i, "description"));
        }
        var componentId = ComponentIdFactory<PaginationMenu>.CreateFromGuid();
        var pagination = new PaginationMenu(componentId, items);
        // The event if item was selected by user.
        pagination.OnSelected += async (args) =>
        {
            await args.Component.RespondAsync($"You select {args.SelectedItem.Label}.");
        };
        // The event if cancel was selected by user. 
        pagination.OnCanceled += async (menu, component) =>
        {
            await component.RespondAsync("canceled");
        };

        await RespondAsync("pagination menu", components: pagination.GetComponentBuilder().Build());
    }
}
```

### Error Handling of command
Can't use the component was sent before restart bot. It's because the instance of the component was lost when close the bot.  
```cs
public class ModuleTest : InteractionModuleBase
{
    static ModuleTest()
    {
        ToggleButton.OnError += OnError;
        PaginationMenu.OnError += OnError;
    }

    private static async Task OnError(SocketMessageComponent component)
    {
        await component.RespondAsync("Can't use the component was sent before restart bot.");
    }
}
```

## 👀Features
### Kind of the component
- Toggle button
- Pagination menu

## 🍄Author
[Twitter](https://twitter.com/DTB_AutumnSky)

## ©️License
DiscordGUILib is licensed under the MIT License.
