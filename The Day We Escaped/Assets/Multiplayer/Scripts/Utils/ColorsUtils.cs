using UnityEngine;

namespace Multiplayer.Scripts.Utils
{
    public class ColorsUtils
    {
        private static Color[] _Colors = new Color[]
        {
            Color.blue,
            Color.red,
            Color.gray,
            Color.yellow,
            Color.cyan,
            Color.white,
        };

        public static Color GetRandomColor()
        {
            var c = UnityEngine.Random.ColorHSV(0.2f, 1f, 0.2f, 1f, 0.2f, 1f, 1, 1);
            return _Colors[UnityEngine.Random.Range(0, _Colors.Length)] + c;
        }
        
        public static Color ConvertIntToColor(uint aCol)
        {
            Color c;
            c.b = (byte)((aCol) & 0xFF);
            c.g = (byte)((aCol>>8) & 0xFF);
            c.r = (byte)((aCol>>16) & 0xFF);
            c.a = (byte)((aCol>>24) & 0xFF);
            return c;
        }
    }
}
