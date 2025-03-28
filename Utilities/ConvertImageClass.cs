using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOADPRO.Utilities;

public static class ConvertImageClass
{
    public static Image ConvertByteArrayToImage(byte[] byteArray)
    {
        if (byteArray == null || byteArray.Length == 0)
            return null;

        using (MemoryStream ms = new MemoryStream(byteArray))
        {
            return Image.FromStream(ms);
        }
    }
}
