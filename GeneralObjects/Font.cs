using System;

namespace RhythmXT.GeneralObjects;

class Font
{
    internal string Name { get; set; } = "Yu Gothic";
    internal int Size { get; set; } = 18;

    public static implicit operator Font(string value)
    {
        string[] parts = value.Split(", ");
        return new Font()
        {
            Name = parts[0],
            Size = Convert.ToInt32(parts[1])
        };
    }

    public static explicit operator string(Font font) => $"{font.Name}, {font.Size}";
}