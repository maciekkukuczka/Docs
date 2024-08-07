namespace Docs.Modules.Common.ColorGenerator;

public static class ColorGenerator
{
    public static string DrawColor(this string color)
    {
        Random rnd = new();
        var hex = rnd.Next(0, 16777215).ToString("X6");
      return $"#{hex}";
    }
}