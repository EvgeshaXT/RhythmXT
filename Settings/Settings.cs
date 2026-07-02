using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace RhythmXT.Settings;

internal static class SettingsManager
{
    internal static string Language { get; private set; } = "English";
    internal static bool ShowOriginalNames { get; private set; } = true;
    internal static Dictionary<string, string> Localisation { get; private set; } = [];

    internal static void Load()
    {
        using (FileStream fs = new("Settings/settings.json", FileMode.Open))
        {
            if (fs.Length == 0)
            {
                SettingsData defaultSettings = new();

                string json = JsonSerializer.Serialize(defaultSettings);
                File.WriteAllText("Settings/settings.json", json);
            }
            else
            {
                SettingsData settingsData = JsonSerializer.Deserialize<SettingsData>(fs);
                Language = settingsData.Language;
                ShowOriginalNames = settingsData.ShowOriginalNames;
            }
        }

        LoadLocalisation();
    }

    static void LoadLocalisation()
    {
        string filePath = $"Localisation/{Language}.txt";
        
        if (File.Exists(filePath))
        {
            foreach (string line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split('=');
                Localisation[parts[0]] = parts[1];
            }
        }
    }

    internal static string GetTranslation(string key)
    {
        return Localisation.TryGetValue(key, out string value) ? value : key;
    }
}

class SettingsData
{
    public string Language { get; set; } = "English";
    public bool ShowOriginalNames { get; set; } = true;
}