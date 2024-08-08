namespace Docs.Modules.Common.ColorGenerator;

public static class ColorGenerator
{
    public static string DrawColor( )
    {
        /*Random rnd = new();
        var hex = rnd.Next(0, 16777215).ToString("X6");
      return $"#{hex}";*/
      
      Random random = new();
      int r = random.Next(256);
      int g = random.Next(256);
      int b = random.Next(256);
      return $"#{r:X2}{g:X2}{b:X2}";
    }
}