using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PD2ModHelpLib;

public abstract class MenuItem {

    internal MenuItem(
        string id,
        string title,
        string description,
        string callback
    ) { this.id = id; this.title = title; this.description = description; this.callback = callback; }

    // Used so we can use MenuOptionType when interacting with the library
    // Better than using strings which are too freeform imo
    public MenuOptionType itemType { get => type.ToMenuOption(); }

    protected string type;
    
    public string id;
    public string title;
    public string description;
    public string callback;

    /*
        This key is contained within square brackets in the BLT docs, don't know how we'd de/reserialize this?
        Even if we could manage I think it's better to leave this dead, those who REALLY want to disable it can do so manually
        Localization is at worst slightly more irritating to set up, but considering we're automating that it's not something to worry about!

        !!! In conclusion: LOCALIZE YOUR GODDAMN MODS, I DON'T CARE IF IT'S ONLY IN ENGLISH RIGHT NOW, SET UP MOD LOCALIZATION YOU FUCKING SCOUNDRELS !!!

        Yours Truly - A Monolingual English Speaker
    */
    // public bool localized;

}

// !!! Below this point lies madness !!!

// Only used for better naming
// Also I like MenuItem being abstract
// :)
internal class MenuButton : MenuItem { public MenuButton(string id, string title, string desc, string callback) : base( id, title, desc, callback ) { type = MenuOptionType.Button.ToJsonVal(); } }

internal class MenuDivider : MenuItem {

    public MenuDivider(string id, string title, string desc, string callback, int size) : base( id, title, desc, callback ) {
        type = MenuOptionType.Divider.ToJsonVal();
        this.size = size;
    }

    public int size;
}

internal class MenuToggle : MenuItem {

    public MenuToggle(string id, string title, string desc, string callback, string value, bool defaultValue) : base( id, title, desc, callback ) {
        type = MenuOptionType.Toggle.ToJsonVal();
        this.value = value;
        this.defaultValue = defaultValue;
    }

    public string value;

    [JsonPropertyName("default_value")]
    public bool defaultValue;
}

internal class MenuSlider : MenuItem {

    public MenuSlider(string id, string title, string desc, string callback, string value, float min, float max, float step, float defaultValue) : base( id, title, desc, callback ) {
        type = MenuOptionType.Slider.ToJsonVal();
        this.value = value;
        this.min = min;
        this.max = max;
        this.step = step;
        this.defaultValue = defaultValue;
    }

    public string value;
    public float min;
    public float max;
    public float step;

    [JsonPropertyName("default_value")]
    public float defaultValue;
}

internal class MenuMultiChoice : MenuItem {

    public MenuMultiChoice(string id, string title, string desc, string callback, string value, int defaultValue, params string[] items) : base( id, title, desc, callback ) {
        type = MenuOptionType.MultiChoice.ToJsonVal();
        this.value = value;
        this.defaultValue = defaultValue;
        this.items = new List<string>(items);
    }

    public string value;
    public List<string> items;

    [JsonPropertyName("default_value")]
    public int defaultValue;
}

internal class MenuKeybind : MenuItem {

    public MenuKeybind(string id, string title, string desc, string callback, string func, string keybindID) : base( id, title, desc, callback ) {
        this.func = func;
        this.keybindID = keybindID;
    }

    public string func;

    [JsonPropertyName("keybind_id")]
    public string keybindID;
}