using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaterScriptEngine
{
    public class Util
    {
        public static void Deconstruct(out byte w, out byte x, out byte y, out byte z, byte[] arr) => (w, x, y, z) = (arr[0], arr[1], arr[2], arr[3]);
        public static void Deconstruct(out byte x, out byte y, byte[] arr) => (x, y) = (arr[0], arr[1]);
    }
}
