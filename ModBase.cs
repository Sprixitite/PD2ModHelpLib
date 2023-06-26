using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PD2ModHelpLib;

public enum ModType {
    LocalizationReplacement,
    GenericCode
}

public enum MenuOptionType {
    Button,
    Divider,
    Toggle,
    Slider,
    MultiChoice,
    Keybind
}

public static class MenuOptionTypeExtensions {

    public static string ToJsonVal(this MenuOptionType self) {
        switch (self) {
            case MenuOptionType.Button: return "button";
            case MenuOptionType.Divider: return "divider";
            case MenuOptionType.Toggle: return "toggle";
            case MenuOptionType.Slider: return "slider";
            case MenuOptionType.MultiChoice: return "multiple_choice";
            case MenuOptionType.Keybind: return "keybind";
            default: throw new Exception("Failed to convert MenuOptionType to string!");
        }
    }

    public static MenuOptionType ToMenuOption(this string self) {
        if (self == "button") return MenuOptionType.Button;
        else if (self == "divider") return MenuOptionType.Divider;
        else if (self == "toggle") return MenuOptionType.Toggle;
        else if (self == "slider") return MenuOptionType.Slider;
        else if (self == "multiple_choice") return MenuOptionType.MultiChoice;
        else if (self == "keybind") return MenuOptionType.Keybind;
        else throw new Exception("Failed to convert string to MenuOptionType!");
    }

}

public abstract class Mod {

    internal Mod(string name, string path) {
        modName = name;

        // Remove trailing path separators
        modPath = path.TrimEnd('\\').TrimEnd('/');
    }

    // public static Mod Create(ModType which, string name, bool usesCustomAssets, string path = null) {

    //     path ??= Directory.GetCurrentDirectory();

    //     switch ( which ) {
    //         case ModType.LocalizationReplacement:
    //             break;
    //         case ModType.GenericCode:
    //             break;
    //     }

    // }

    public string modName {
        get;
        init;
    }

    public string modPath {
        get;
        init;
    }

    public string menuPath {
        get => Path.Combine( modPath, "menus" );
    }

    public virtual void Init() {
        Directory.CreateDirectory( menuPath );
        FileStream optionsStream = File.Create( Path.Combine( menuPath + "options.json" ) );
        StreamWriter optionsWriter = new StreamWriter(optionsStream);
        optionsWriter.Write("{\n}");
        optionsWriter.Close();
    }

    private List<Menu> menus;

    public string[] getMenuIDs() {
        string[] menuIDs = new string[menus.Count];
        foreach (Menu m in menus) {
            menuIDs[0] = m.menuID;
        }
        return menuIDs;
    }

    public virtual void AddMenu(string menuID, string parentMenuID, string titleLocID, string descriptionLocID) {
        string newMenuPath = Path.Combine( menuPath + menuID );
        if ( File.Exists( newMenuPath ) ) throw new IOException("Menu File Already Exists!");

        Menu newMenu = new Menu();
        newMenu.menuID = menuID;
        newMenu.parentMenuID = parentMenuID;
        newMenu.title = titleLocID;
        newMenu.description = descriptionLocID;

        Utf8JsonWriter jsonWriter = new Utf8JsonWriter( File.Create( newMenuPath ) );

        JsonDocument menuJSON = JsonSerializer.SerializeToDocument(newMenu);
        menuJSON.WriteTo(jsonWriter);

        jsonWriter.Dispose();
    }

    public virtual void RemoveMenu(string menuID) {
        string badMenuPath = Path.Combine( menuPath + menuID );
        if ( !File.Exists( badMenuPath ) ) throw new FileNotFoundException("Menu File Doesn't Exist!");
        File.Delete( badMenuPath );
    }

    public virtual void AddOption(
        string menuID,
        MenuItem option
    ) {
        string editingMenuPath = Path.Combine( menuPath, menuID );
        if ( !File.Exists( editingMenuPath ) ) throw new FileNotFoundException("Menu File Doesn't Exist!");

        Menu editingMenu = JsonSerializer.Deserialize<Menu>( File.Open( editingMenuPath, FileMode.Open ) );

        editingMenu.items.Add( option );

        Utf8JsonWriter jsonWriter = new Utf8JsonWriter( File.Open( editingMenuPath, FileMode.Open ) );

        JsonDocument menuJSON = JsonSerializer.SerializeToDocument( editingMenu );
        menuJSON.WriteTo(jsonWriter);

        jsonWriter.Dispose();
    }

}