using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PD2ModHelpLib;

public class Menu {

    public string title { get; set; }
    public string description { get; set; }

    // Passthroughs to use C# naming conventions
    // (I hate C#'s naming conventions, I am only doing this for the sake of people editing this code later, be thankful)
    public string menuID {
        get => menu_id;
        set => menu_id = value;
    }

    public string parentMenuID {
        get => parent_menu_id;
        set => parent_menu_id = value;
    }

    private string menu_id;
    private string parent_menu_id;

    public List<MenuItem> items;

}