using System;
using System.Collections.Generic;
using System.Text;

namespace Agrin2.Generals
{
    public static class Security
    {
        private readonly static string alphaCaps = "QWERTYUIOPASDFGHJKLZXCVBNM";
        private readonly static string alphaLow = "qwertyuiopasdfghjklzxcvbnm";
        private readonly static string numerics = "1234567890";
        private readonly static string special = "@#$";
        //create another string which is a concatenation of all above
        private readonly static string allChars = alphaCaps + alphaLow + numerics + special;
        private readonly static Random r = new Random();
        public static string GenerateStrongPassword(int length)
        {
            String generatedPassword = "";

            if (length < 4)
                throw new Exception("Number of characters should be greater than 4.");

            // Generate four repeating random numbers are postions of
            // lower, upper, numeric and special characters
            // By filling these positions with corresponding characters,
            // we can ensure the password has atleast one
            // character of those types
            int pLower, pUpper, pNumber, pSpecial;
            string posArray = "0123456789";
            if (length < posArray.Length)
                posArray = posArray.Substring(0, length);
            pLower = getRandomPosition(ref posArray);
            pUpper = getRandomPosition(ref posArray);
            pNumber = getRandomPosition(ref posArray);
            pSpecial = getRandomPosition(ref posArray);


            for (int i = 0; i < length; i++)
            {
                if (i == pLower)
                    generatedPassword += getRandomChar(alphaCaps);
                else if (i == pUpper)
                    generatedPassword += getRandomChar(alphaLow);
                else if (i == pNumber)
                    generatedPassword += getRandomChar(numerics);
                else if (i == pSpecial)
                    generatedPassword += getRandomChar(special);
                else
                    generatedPassword += getRandomChar(allChars);
            }
            return generatedPassword;
        }

        private static string getRandomChar(string fullString)
        {
            return fullString.ToCharArray()[(int)Math.Floor(r.NextDouble() * fullString.Length)].ToString();
        }

        private static int getRandomPosition(ref string posArray)
        {
            int pos;
            string randomChar = posArray.ToCharArray()[(int)Math.Floor(r.NextDouble()
                                           * posArray.Length)].ToString();
            pos = int.Parse(randomChar);
            posArray = posArray.Replace(randomChar, "");
            return pos;
        }
    }
}
