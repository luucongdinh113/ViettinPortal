using System;

namespace EncryptionLibrary
{
    public class SimpleEncryption
    {
        public static string EnScriptPass(string strPass)
        {
            string str = string.Empty;
            int[] buffer = new int[5];
            Random randomizer = new Random();
            for (int i = 0; i < 5; i++)
            {
                buffer[i] = randomizer.Next(25) + 1;
            }

            for (int i = 0; i < strPass.Length; i++)
            {
                var temp = (int)strPass[i] + (i + 1) + buffer[(i + 1) % 5];
                string hexaTemp = temp.ToString("X").ToUpper();

                str += hexaTemp;
            }

            var prefix = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                prefix += (char)(byte)((int)(2 * buffer[i]) + 65);
            }

            str = str.Insert(0, prefix);

            return str;
        }

        public static string DeScriptPass(string strPass)
        {
            string str = string.Empty;
            int[] buffer = new int[5];
            int index = 0;
            for (index = 0; index < 5; index++)
            {
                buffer[index] = ((int)strPass[index] - 65) / 2;
            }

            for (index = 0; index < (strPass.Length - 5) / 2; index++)
            {
                string hexaTemp = strPass.Substring(5 + index * 2, 2);
                int temp = Convert.ToInt32(hexaTemp, 16);

                temp = temp - index - 1;
                temp = temp - buffer[(index + 1) % 5];

                str = str + (char)temp;
            }

            return str.TrimEnd();
        }
    }
}
