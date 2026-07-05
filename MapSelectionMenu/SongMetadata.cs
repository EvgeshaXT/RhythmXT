using System.IO;

namespace RhythmXT.MapSelectionMenu;

class SongMetadata
{
    internal string ArtistOriginalName { get; private set; } = "";
    internal string ArtistName { get; private set; }
    internal string TitleOriginalName { get; private set; } = "";
    internal string TitleName { get; private set; }
    internal string ImagePath { get; private set; }

    internal static SongMetadata ParseXTFile(string songFolderPath)
    {
        SongMetadata songMetadata = new();

        string songFullName = Path.GetFileName(songFolderPath);

        string[] songFullNameParts = songFullName.Split(" - ");
        songMetadata.ArtistName = songFullNameParts[0];
        songMetadata.TitleName = songFullNameParts[1];

        string mainXTFilePath = $"{songFolderPath}/main.xt";
        string[] lines = File.ReadAllLines(mainXTFilePath);
        
        foreach (string line in lines)
        {
            if (line.StartsWith("Artist: "))
            {
                string[] parts = line.Split(": ");
                songMetadata.ArtistOriginalName = parts[1];
            }

            if (line.StartsWith("Title: "))
            {
                string[] parts = line.Split(": ");
                songMetadata.TitleOriginalName = parts[1];
            }

            if (line.StartsWith("Image: "))
            {
                string[] parts = line.Split(": ");
                songMetadata.ImagePath = $"Songs/{songFullName}/{parts[1]}";
            }
        }

        return songMetadata;
    }
}