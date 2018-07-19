using System;
using System.ComponentModel.Design;

namespace KnuthMorrisPratt
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Knuth-Morris-Pratt Algorithm Visualizer");

            string text = string.Empty, pattern = string.Empty;
            switch (args.Length)
            {
                case 0:
                    GetInputs(out text, out pattern);
                    break;

                case 2:
                    text = args[0];
                    pattern = args[1];
                    break;

                default:
                    Console.WriteLine("Usage: KnuthMorrisPratt [text pattern]");
                    Environment.Exit(1);
                    break;
            }

            text = text.Trim();
            pattern = pattern.ToLower();

            Console.WriteLine($"Text: '{text}'\nPattern: '{pattern}'");
            VisualizeKmp(text.ToCharArray(), pattern.ToCharArray());
        }

        private static void VisualizeKmp(char[] S, char[] W)
        {
            var T = GeneratePartialMatchTable(W);

            Console.WriteLine("Partial Match Table:");
            PrintTable(W, T);

            int j = 0, k = 0;
            while (j < S.Length)
            {
                PrintState(S, W, j, k);
                if (W[k] == S[j])
                {
                    j++;
                    k++;

                    if (k != W.Length) continue;
                    Console.WriteLine("Occurence found at index " + (j - k));
                    return;
                }

                //else
                k = T[k];
                if (k >= 0) continue;
                // else
                j++;
                k++;
            }
        }

        private static void PrintTable(char[] W, int[] T)
        {
            for (int i = 0; i < T.Length; i++)
                Console.Write("{0,5}", i);

            Console.WriteLine();
            foreach (var w in W)
                Console.Write("{0,5}", w);

            Console.WriteLine();
            foreach (var t in T)
                Console.Write("{0,5}", t);

            Console.WriteLine();
        }

        private static int[] GeneratePartialMatchTable(char[] pattern)
        {
            var table = new int[pattern.Length];
            int pos = 1, cnd = 0;
            table[0] = -1;

            while (pos < pattern.Length)
            {
                if (pattern[pos] == pattern[cnd])
                {
                    table[pos] = table[cnd];
                    pos++;
                    cnd++;
                }
                else
                {
                    table[pos] = cnd;

                    cnd = table[cnd];
                    while (cnd >= 0 && pattern[pos] != pattern[cnd]) cnd = table[cnd];

                    pos++;
                    cnd++;
                }
            }

            return table;
        }

        private static void PrintState(char[] text, char[] pattern, int j, int k)
        {
            int m = j - k;
            Console.WriteLine($"\nj={j}, k={k}, m={m}");

            // text index
            PrintColored("\tIndex j    ", ConsoleColor.Gray);
            PrintIndices(text.Length, j, m);

            // text
            PrintColored("\tText S:    ", ConsoleColor.Gray);
            foreach (var textChar in text)
                Console.Write("{0,5}", textChar);
            Console.WriteLine();

            // pattern left padding
            string patternLeftPad = new string(' ', m * 5);

            // pattern
            PrintColored("\tPattern W: " + patternLeftPad, ConsoleColor.Gray);
            foreach (var patternChar in pattern)
                Console.Write("{0,5}", patternChar);
            Console.WriteLine();

            // pattern index
            PrintColored("\tIndex k    " + patternLeftPad, ConsoleColor.Gray);
            PrintIndices(pattern.Length, k);
        }

        private static void PrintColored(string s, ConsoleColor color)
        {
            var oldColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.Write(s);

            Console.ForegroundColor = oldColor;
        }

        private static void PrintIndices(int length, int highlightedIndex, int otherHighlighted = -1)
        {
            var oldColor = Console.ForegroundColor;
            
            for (int index = 0; index < length; index++)
            {
                if (index == highlightedIndex) Console.ForegroundColor = ConsoleColor.Yellow;
                else if (index == otherHighlighted) Console.ForegroundColor = ConsoleColor.DarkRed;
                else Console.ForegroundColor = ConsoleColor.DarkGray;

                Console.Write("{0,5}", index);
            }

            Console.ForegroundColor = oldColor;
            Console.WriteLine();
        }

        private static void GetInputs(out string text, out string pattern)
        {
            while (Console.KeyAvailable) Console.ReadKey();
            Console.Write("Text: ");
            text = Console.ReadLine();

            while (Console.KeyAvailable) Console.ReadKey();
            Console.Write("Pattern: ");
            pattern = Console.ReadLine();
            Console.WriteLine();
        }
    }
}