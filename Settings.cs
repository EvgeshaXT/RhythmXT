using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace RhythmXT;

internal static class Settings
{
    internal static string Language { get; private set; } = "English";
    internal static bool ShowOriginalNames { get; private set; } = true;
    internal static Dictionary<string, string> Localisation { get; private set; } = [];

    static readonly string settingsPath = "settings.json";

    internal static void Load()
    {
        if (!File.Exists(settingsPath))
        {
            SettingsData defaultSettings = new();
            JsonSerializerOptions jsonSerializerOptions = new()
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(defaultSettings, jsonSerializerOptions);
            File.WriteAllText(settingsPath, json);
        }

        using (FileStream fs = new(settingsPath, FileMode.Open))
        {
            SettingsData settingsData = JsonSerializer.Deserialize<SettingsData>(fs);

            Language = settingsData.Language;
            ShowOriginalNames = settingsData.ShowOriginalNames;
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