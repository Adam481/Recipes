using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Recipes.Common.DotNetFramework
{
    public static class RegExLearning
    {
        // regex engine does matching one character at a time - lets analyze following example
        // pattern: cat         (pattern is move through the subject from left to right until 
        // subject: dogcat       character comparison succeed or there is no subject to check
        //
        // 1. fail        2. fail             3. fail     4. ok     5. ok       6.  ok      
        //       cat            cat            cat           cat         cat        cat
        //       |              |              |             |            |           |
        //       dogcat        dogcat        dogcat       dogcat      dogcat     dogcat
        // 
        // result: pattern match OK
        //
        // Match give us some information beyond weather or not the pattern is match the subject
        // - where the in the subject the match was made
        // - how much of the subject match the pattern
        //
        //  Alternation logic
        //
        // 1. fail     2. alternetive: ok  3. ok   4. ok
        //      dog       cat             cat       cat  
        //      |         |                |          |
        //      catdog    catdog          catdog    catdog
        //
        // all alternatives must fail at a given position before the pattern is move to the
        // next position
        //
        // "dog|cat", "zcatdog"
        // 1.fail       2. fail    3. fail  4. ok   5. ok ......
        //     dog      cat        dog      cat       cat
        //     |        |          |        |          |
        //     zcatdog  zcatdog   zcatdog  zcatdog   zcatdog
        //
        // Backtracking
        //
        // pattern: [abc] subject: [zzzc]
        //  1. save state  2. check if it is   3. if not backgrack and check the rest of regex
        //                 'a' 'b' or 'c'
        //  [abc]            [abc]              [abc]...
        //   |                |                      |
        //    zzzc            zzzc                   zzzc
        //
        // after this we can move to the next character - proces is repeated lot of times
        //  save state          - if the not match move to the saving state again..
        //  [abc]    [abc]
        //   |        |
        //  z zzc    zzzc
        //


        public static void Run()
        {
            //SimpleRegExExamples();
            //NextMatchMethodTest();
            //StringEndLineEndPoint();
            //CharacterClasses();
            RegexGroupsAndCaptures();

        }

        private static void SimpleRegExExamples()
        {
            // concatination examples
            //ShowMatch("cat", "cat" );       // true
            //ShowMatch("cat", "dog");        // false
            //ShowMatch("cat", "dogcat");     // true
            //ShowMatch("cat", "catcat");     // true (first occurring  will be find)

            // alternation examples            - alternatives are check from left to right. if first 
            //                                   is succeed stop further checking.
            //ShowMatch("cat|dog", "cat");        // true
            //ShowMatch("cat|dog", "zzzdogzz");   // true
            //ShowMatch("dog|cat", "catdog");     // true  cat is left most match


            // repetition examples         
            //ShowMatch("a*", "a");       //ok
            //ShowMatch("a*", "aa");      //ok
            //ShowMatch("a*", "aaaaaaa"); //ok
            //ShowMatch("a*", "");        //ok
            //ShowMatch("cat*", "catcat");  // ok   but! mutch 'ca' with any number of 't' like: catttt
            //ShowMatch("(cat)*", "catcat");// ok   we use character grouping - match looks as expected
            //ShowMatch("(cat) * ", "dog"); // ok   dog occurs 0 times so its ok
            //ShowMatch("(cat){2}", "catcatcatcat");  // ok 
            //ShowMatch("(cat){1,3}", "catcatcatcat");// ok 
            //ShowMatch("(cat){0,3}", "dog");         // ok 
            //ShowMatch("(cat){0,1}", "cat");         // ok
            //ShowMatch("(cat){0,1}", "dog");         // ok
            //ShowMatch("(cat){0,1}", "catcat");      // ok  - only first occurs
            //ShowMatch("(cat)?", "catcat");          // as aboce ? - means {0,1}
            ////idiom:
            //ShowMatch("(cat)(cat)*", "catcatcat");    // ok
            //ShowMatch("(cat)(cat)*", "catcatcatcat"); // ok
            //ShowMatch("(cat)(cat)*", "cat");          // ok
            //ShowMatch("(cat)(cat)*", "dog");          // fail (first the cat musth occourse)
            //ShowMatch("(cat)+", "dog");               // fail the same as above
            //ShowMatch("(cat)+", "catcat");            // ok




        }

        private static void NextMatchMethodTest()
        {
            //ShowMatch("cat", "catcat");
            //ShowMatch("(cat)?", "dogcat");
            //ShowMatch("dog|cat", "catzzzdog");
            //ShowMatch("(dog)?|cat", "catzzzdog");// cat is never matched - dog always match all criteria (can occur 0 times)
        }

        private static void StringEndLineEndPoint()
        {
            //ShowMatch(@"cat", "dogcat");   // ok
            //ShowMatch(@"\Acat", "dogcat"); //fail
            //ShowMatch(@"\Acat", "catdog"); //ok
            //ShowMatch(@"\Acat", "dog\ncat"); //fail
            //ShowMatch(@"^cat", "dog\ncat"); //fail - the same as above
            //ShowMatch(@"(?m)^cat", "dog\ncat"); //ok check the begining of each line
            //ShowMatch(@"(?m)^cat", "cat\ndog\ncat"); //ok match two cats
            //ShowMatch(@"(?m)^cat|^dog", "cat\ndog\ncat"); //ok three matches
            //ShowMatch(@"(?m:^cat)|^dog", "cat\ndog\ncat");
            //ShowMatch(@"cat$", "cat");      // OK
            //ShowMatch(@"cat$", "catdog");   // fail
            //ShowMatch(@"cat$", "cat\n");    // ok
            //ShowMatch(@"cat$", "cat\n\n");  // fail - cat must be last statement in the string (only 1 new line is allowed)
            //ShowMatch(@"(?m)cat$", "dogcat\n"); //ok
            //ShowMatch(@"(?m)cat$", "dogcat\n\n"); // ok
            //ShowMatch(@"(?m:cat$)|dog$", "cat\ndog\ncat\n"); // ok  match cat at the end of each line and dog only ad the end of the string
            //ShowMatch(@"dog\z", "dog\n");   //fail 
            //ShowMatch(@"dog\z", "dog");     // true
            //ShowMatch(@"dog\n\z", "dog\n"); // true
            //ShowMatch(@"dog\Z", "dog\n");   // true
        }

        private static void CharacterClasses()
        {
            //ShowMatch(@"\(dog\)\?", @"(dog)?"); // to avoid this not readable format we can just use Excape method;

            //ex:
            //var pattern = @"(dog)?";
            //var subject = @"(dog)?";
            //var escapePattern = Regex.Escape(pattern);          // method will automatically figure out which character are special
            //Console.WriteLine($"Escape pattern: {escapePattern}");

            //ShowMatch(@"\b", "Pluralsight video courses are the bomb!");
            //ShowMatch(@"\B", "Pluralsight video courses are the bomb!");

            //ShowMatch(@".", "\n"); // false
            //ShowMatch(@".", "abcd");    
            //ShowMatch(@"...", "abcd");
            //ShowMatch(@".{4}", "aaaaaaaaa");
            //ShowMatch(@"c.t", "cut");
            //ShowMatch(@"c.t", "cat");
            //ShowMatch(@"(?s).", "\n");
        }

        public static void RegexGroupsAndCaptures()
        {


        }


        // quick guide
        // |        - alternative
        // ()       - group
        // {2}      - number of repeats (min number is 2 max is also 2)
        // {2,4}    - number of repeats (min 2 max 4)
        // *        - 0 or any number of repeats
        // +        - 1 or any number of repeats = (cat)(cat)*
        // ?        - 0 or 1 number of repeats = {0,1}
        // \n       - new line
        // \A       - pattern can match only at the begging of the string (it does not matter how many line string has)
        // ^        - the same as above (default)
        // (?m)     - allow as to mark the string as multi-line. (change the behavior of the above command to 
        //            check the begging of each line) example: "(?m)^cat" 
        //            pattern have NO IMPACT on \A  . modifier is declared for whole pattern: "(?m)^cat|^dog"
        //            "(?m:^cat)|^dog" - modifier has restricted scope only to cat
        // $        - last character of the string (by default)
        // (?m)     - we can also use this modifier with the '$' character to match the end of each line insted of
        //            the end of the string. "(?m:cat$) - restricted scope
        // \Z       - awals match at the end of the string (can not be affected by (?m) modifier              
        // \z       - as abouce but last character must be a part of the pattern
        // [abc]    - character classess allow us to specify more character (multi alternative)
        // [a-z0-9] - predefined class - match character a-z or digits
        // [^0-9A]  - negation - match any character that is not in the character class
        // \d       - any decimal [0-9]
        // \D       - negation   [^0-9]
        // \s       - any whitespace
        // \S       - negation - everything that is not whitespace
        // \w       - any character that we use in words [a-zA-Z0-9_]  letter, digits or underscore
        // \W       - negation [^a-zA-Z0-9_]
        // \        - we can search for special character when we put '\' before them  ex: \(dog\)\?
        // \b       - detect the boundary between the words and produce empty match.
        //            the boundary is created by \w and \W (character and non character class)
        //            boundary exist between character that are used for words and that are not used for words
        //            |Pluralsight| |video| |courses| |are| |the| |bomb|!
        // \B         produce empty matches at all the possition of the string that \b does not produced
        //            P|l|u|r|a|l|s|i|g|h|t v|i|d|e|o c|o|u|r|s|e|s a|r|e t|h|e b|o|m|b|!|
        // .          by default match any character expect the new line (wildcard character) 
        // (?s)       changes default operation  (. -character match all characters including the new line)
        //            limited scope: (?:c.t)|d.g

        // Regex Groups and Captures


        private static void ShowMatch(string pattern, string subject)
        {
            var regex = new Regex(pattern);
            var match = regex.Match(subject);

            while (match.Success)
            {
                string result = "";

                result += $"pattern: {pattern}  subject: {subject}" + Environment.NewLine;
                result += $"result: {match.Success}" + Environment.NewLine;
                result += $"index: {match.Index} length: {match.Length}" + Environment.NewLine;

                Console.WriteLine(result);

                match = match.NextMatch();
            }

            if (!match.Success)
            {
                string result = "";
                result += $"pattern: {pattern}  subject: {subject}" + Environment.NewLine;
                result += $"result: {match.Success}" + Environment.NewLine;

                Console.WriteLine(result);
            }

        }
    }
}
