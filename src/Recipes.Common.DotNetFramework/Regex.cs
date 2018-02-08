using System;
using System.Collections.Generic;

namespace Recipes.Common.DotNetFramework
{
    class Regex
    {
        static void Tests()
        {
            List<string> patterns = new List<string>()
            {
                "[a-g]",
                "[a-g]{3}",
                "^[a-g]{3}$",
                "^[a-g]{1,3}$",
                "^[0-9]{8}$",
                "^[a-z]{3}[0-9]{5}$",
                "^[a-zA-Z]{3}[0-9]{5}$",
            };

            List<string> texts = new List<string>()
            {
                "aaaaa",
                "1",
                "aaa",
                "a",
                "agc",
                "00000000",
                "aaa00000",
                "AAA00000"
            };

            ShowResult(patterns, texts);

            Console.Read();
        }

        static void ShowResult(List<string> patterns, List<string> texts)
        {
            Regex obj;

            foreach (string pattern in patterns)
            {
                obj = new Regex(pattern);
                foreach (string text in texts)
                {
                    Console.WriteLine("Patter: {0}  str: {1} : {2}",
                        pattern, text.PadRight(10, ' '), obj.IsMatch(text));
                }
                Console.WriteLine();
            }
        }

        /* 
        
        []             - what characters we are going to use
        {}             - how many characters we are going to use
        ^              - start
        $              - end
        [a-g]{3}       - characters from a to g But aaaa - also have 'aaa' so that the expresion is valid
        ^[a-g]{1,3}$   - min lenght 1 max 3
        ^[0-9]{8}$     - number with 8 digits
        ^[a-z]{3}[0-9]{5}$

         */

    }
}
}
