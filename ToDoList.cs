
using System.ComponentModel;
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata.Ecma335;

var entry_list = new List<string>(); var exit = false;
bool firstIteration = true;
int identifier_increment = 0;
//public int identifier_increment;
//

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

foreach (var entries in entry_list)
{
    Console.WriteLine(entries);
}

while (exit == false)
{
    if (firstIteration)
    {
        Console.WriteLine(Menu.DisplayMenu());
        firstIteration = false;
    }
    int menu_input = 0;
    bool validInput = false;
    while (!validInput)
    {
        Console.Write("> ");
        try
        {
            menu_input = Convert.ToInt32(Console.ReadLine());
            if (menu_input <= 9 && menu_input >= 1)
            {
                validInput = true;
            }
        }
        catch (System.FormatException)
        {
            Console.WriteLine("Error: Please enter a number from 1-7");
        }

        switch (menu_input)
        {
            case 1:
                entry_list.Add(Menu.AddEntry(identifier_increment));
                identifier_increment++;
                Console.WriteLine($"Successfully added Entry #{identifier_increment}");
                break;

            case 2:
                Menu.DeleteEntry(entry_list);
                break;

            case 3:
                Menu.ModifyStatus(entry_list);
                break;

            case 4:
                Menu.DisplayEntryTitle(entry_list);
                break;

            case 5:
                Menu.DisplayEntryDescription(entry_list);
                break;

            case 6:
                Menu.DisplayEntry(entry_list);
                break;

            case 7:
                Menu.DisplayEntryList(entry_list);
                break;

            case 8:
                Console.WriteLine(Menu.DisplayMenu());
                break;

            case 9:
                exit = true;
                Console.WriteLine("Exiting program...");
                break;
        }

    }

}

public class Menu
{
    public static string DisplayMenu()
    {
        string menu_output = (
        @"
        To-Do List
        -----------------------------
    
        Please select an option from the below by entering the line's numerical identifier (i.e. 1, 2, 3).
    
        1. Add To-Do Entry
        2. Delete To-Do Entry
        3. Modify Status of Entry
        4. Display Title of an Entry
        5. Display the Body of an Entry
        6. Display Whole Entry
        7. Display All Entries
        8. Display Menu
        9. Exit
        ");

        return menu_output;
    }
    public static string AddEntry(int identifier)
    {
        Console.Write(@"Title: ");
        var input_title = Console.ReadLine();
        Console.Write(@"Description: ");
        var input_desc = Console.ReadLine();
        Console.Write(@"Status: "); bool eXit = false;
        Status? input_status = null; string printed_status = "";
        while (eXit == false)
        {
            var status_desc = Console.ReadLine();
            switch (status_desc)
            {
                case "NotStarted":
                    input_status = Status.NotStarted; printed_status = "NotStarted"; eXit = true;
                    break;
                case "InProgress":
                    input_status = Status.InProgress; printed_status = "InProgress"; eXit = true;
                    break;
                case "Completed":
                    input_status = Status.Completed; printed_status = "Completed"; eXit = true;
                    break;
                default:
                    Console.Write("Please enter a valid status. (NotStarted, InProgress, Completed)\n");
                    Console.Write("Status: ");
                    break;
            }
        }
        Console.WriteLine($"Selected Status Entry Identifier: {input_status}");
#pragma warning disable CS8604 // Possible null reference argument.
        var entry = new Entry
        (
            identifier + 1, input_title, input_desc, printed_status, null, DateTime.Now, DateTime.Today.AddDays(30)
        );
#pragma warning restore CS8604 // Possible null reference argument.
        return entry.ToString();
    }

    public static void DeleteEntry(List<string> entry_list)
    {
        Console.Write("Enter the Entry # of the entry you'd like to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int entryId))
        {
            Console.WriteLine("Invalid input. Please enter a valid entry identifier.");
            return;
        }

        int indRemove = -1;

        for (int i = 0; i < entry_list.Count; i++)
        {
            var lines = entry_list[i].Split('\n');
            if (lines[1].Trim().StartsWith($"ID: {entryId}"))
            {
                indRemove = i;
                break;
            }
        }

        if (indRemove == -1)
        {
            Console.WriteLine($"No entry could be found with that entry identifier.");
            return;
        }

        entry_list.RemoveAt(indRemove);
        Console.WriteLine($"Entry #{entryId} has been removed successfully.");

        for (int i = indRemove; i < entry_list.Count; i++)
        {
            var lines = entry_list[i].Split('\n');
            int currentId = int.Parse(lines[1].Split(':')[1].Trim());
            var initialized_datetime = DateTime.Parse(lines[5].Trim().Substring(14));
            var initialized_deadline = DateTime.Parse(lines[6].Trim().Substring(10));

            Entry updatedEntry = new Entry(
                currentId - 1,
                lines[2].Trim().Substring(7),
                lines[3].Trim().Substring(13),
                lines[4].Trim().Substring(8),
                null,
                initialized_datetime,
                initialized_deadline
            );
            entry_list[i] = updatedEntry.ToString();
        }

        Console.WriteLine("All entries have been adjusted accordingly.");
    }

    public static void ModifyStatus(List<string> entry_list)
    {
        Console.WriteLine("Enter the Entry # of the entry you would like to change the status of: "); bool eXit = false;
        if (!int.TryParse(Console.ReadLine(), out int entryId))
        {
            Console.WriteLine("Invalid input. Please enter a valid entry identifier.");
            return;
        }

        int intModify = -1;

        for (int i = 0; i < entry_list.Count; i++)
        {
            var lines = entry_list[i].Split('\n');
            if (lines[1].Trim().StartsWith($"ID: {entryId}"))
            {
                intModify = i;
                break;
            }
        }

        if (intModify == -1)
        {
            Console.WriteLine($"No entry could be found with that entry identifier.");
            return;
        }

        Status? input_status = null; string printed_status = "";
        Console.WriteLine("Enter the new status of the entry: ");
        while (eXit == false)
        {
            var entry_status_input = Console.ReadLine();
            switch (entry_status_input)
            {
                case "NotStarted":
                    input_status = Status.NotStarted; printed_status = "NotStarted"; eXit = true;
                    break;
                case "InProgress":
                    input_status = Status.InProgress; printed_status = "InProgress"; eXit = true;
                    break;
                case "Completed":
                    input_status = Status.Completed; printed_status = "Completed"; eXit = true;
                    break;
                default:
                    Console.Write("Please enter a valid status. (NotStarted, InProgress, Completed)\n");
                    Console.Write("Status: ");
                    break;
            }
        }
        Console.WriteLine($"New Status Entry Identifier for entry: {input_status}");

        // NEED TO CONFIGURE

        // Iterate through the entry list and change the entry
        var modify_lines = entry_list[intModify].Split('\n');
        Console.WriteLine(modify_lines[4]);
        if (modify_lines[4].Trim().Contains("Status:"))
        {
            int currentId = int.Parse(modify_lines[1].Split(':')[1].Trim());
            var initialized_datetime = DateTime.Parse(modify_lines[5].Trim().Substring(14));
            var initialized_deadline = DateTime.Parse(modify_lines[6].Trim().Substring(10));

            Entry updatedEntry = new Entry(
                currentId,
                modify_lines[2].Trim().Substring(7),
                modify_lines[3].Trim().Substring(13),
                printed_status,
                null,
                initialized_datetime,
                initialized_deadline
            );
            entry_list[intModify] = updatedEntry.ToString();
            Console.WriteLine("Updated entry accordingly.");
            return;
        }
        Console.WriteLine("Reached invalid section of modifiable status function.");
        return;
    }

    public static string DisplayEntryTitle(List<string> entry_list)
    {
        Console.Write(@"Enter the Entry # you would like to display the title of: ");
        var entry_title_input = Console.Read();
        foreach (var entry in entry_list)
        {
            var lines = entry.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            if (lines[1].Trim().StartsWith($"ID: {entry_title_input}"))
            {
                Console.WriteLine("\n" + lines[2] + "\n");
                return lines[2];
            }
        }

        Console.WriteLine("No entry has been found with the given ID");
        return "";
    }

    public static string DisplayEntryDescription(List<string> entry_list)
    {
        Console.Write(@"Enter the Entry # you would like to display the title of: ");
        var entry_title_input = Console.ReadLine();
        foreach (var entry in entry_list)
        {
            var lines = entry.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            if (lines[1].Trim().StartsWith($"ID: {entry_title_input}"))
            {
                Console.WriteLine("\n" + lines[3] + "\n");
                return lines[3];
            }
        }

        Console.WriteLine("No entry has been found with the given ID");
        return "";
    }

    public static string DisplayEntry(List<string> entry_list)
    {
        Console.WriteLine("Enter the Entry # you would like to display the title of.\n> ");
        var entry_title_input = Console.ReadLine();
        foreach (var entry in entry_list)
        {
            var lines = entry.Split('\n');
            if (lines[1].Trim().StartsWith($"ID: {entry_title_input}"))
            {
                Console.WriteLine(lines);
                return entry;
            }
        }
        return "";
    }

    public static void DisplayEntryList(List<string> entry_list)
    {
        foreach (var entries in entry_list)
        {
            Console.WriteLine(entries);
        }
    }
}

public enum Status { NotStarted, InProgress, Completed }
public class Entry
{
    public int identifier { get; set; }
    public string title { get; set; }
    public string desc { get; set; }
    public object status { get; set; }
    public DateTime dateOfEntry { get; set; }
    public DateTime deadline { get; set; }
    public object? isExtensionOf { get; set; }

    public Entry(int Identifier,
        string Title,
        string Desc,
        object Status,
        object IsExtensionOf,
        DateTime DateOfEntry,
        DateTime Deadline)
    {
        identifier = Identifier;
        title = Title;
        desc = Desc;
        status = Status;
        isExtensionOf = IsExtensionOf;
        dateOfEntry = DateOfEntry;
        deadline = Deadline;
    }
    public override string ToString()
    {
        return $@"
        ID: {identifier}
        Title: {title}
        Description: {desc}
        Status: {status}
        Date Created: {dateOfEntry}
        Deadline: {deadline}
        ";
    }

}



