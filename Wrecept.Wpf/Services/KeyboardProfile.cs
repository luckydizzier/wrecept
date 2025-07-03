using System;
using System.IO;
using System.Text.Json;
using System.Windows.Input;

namespace Wrecept.Wpf.Services;

public class KeyboardProfile
{
    public Key Next { get; init; } = Key.Down;
    public Key Previous { get; init; } = Key.Up;
    public Key Confirm { get; init; } = Key.Enter;
    public Key Cancel { get; init; } = Key.Escape;

    public static KeyboardProfile Load(string path)
    {
        try
        {
            if (!File.Exists(path))
                return new KeyboardProfile();

            using var stream = File.OpenRead(path);
            using var doc = JsonDocument.Parse(stream);
            if (!doc.RootElement.TryGetProperty("Keyboard", out var k))
                return new KeyboardProfile();

            Key Parse(string name, Key fallback)
            {
                return k.TryGetProperty(name, out var prop) &&
                       Enum.TryParse<Key>(prop.GetString(), true, out var key)
                    ? key
                    : fallback;
            }

            return new KeyboardProfile
            {
                Next = Parse("Next", Key.Down),
                Previous = Parse("Previous", Key.Up),
                Confirm = Parse("Confirm", Key.Enter),
                Cancel = Parse("Cancel", Key.Escape)
            };
        }
        catch (Exception)
        {
            return new KeyboardProfile();
        }
    }
}
