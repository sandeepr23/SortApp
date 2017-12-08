using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SortApp
{
    class Program
    {
        private static string longestWord = string.Empty;
        private static string secondlongestWord = string.Empty;
        private static List<String> InputList = new List<string>();
        private static List<String> ConcatenatedStrings = new List<string>();
        private static InputListOptmized inputListOptmized = new InputListOptmized();

        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            //Console.WriteLine("Attempting the first approach");
            //stopWatch.Start();
            //ProcessStringFirstAttempt();
            //stopWatch.Stop();
            //TimeSpan firstApproachTimeSpan = stopWatch.Elapsed;
            //Console.WriteLine($"Completed processing in {stopWatch.Elapsed}");
            //stopWatch.Reset();
            Console.WriteLine("Attempting the second approach");
            stopWatch.Start();
            ProcessStringUsingOptimizedList1();
            stopWatch.Stop();
            TimeSpan secondApproachTimeSpan = stopWatch.Elapsed;
            Console.WriteLine($"Completed processing in {stopWatch.Elapsed}");
            //Console.WriteLine($"First appraoch time span in {firstApproachTimeSpan}");
            Console.WriteLine($"Second appraoch time span in {secondApproachTimeSpan}");
            Console.ReadLine();
        }


        private static void ProcessStringFirstAttempt()
        {
            string inputString;
            long stringCount = 0;
            using (StreamReader fileReader = new StreamReader("NET Test 00.txt"))
            {

                while (!String.IsNullOrEmpty(inputString = fileReader.ReadLine()))
                {
                    ProcessString(inputString);
                    stringCount++;
                    Console.WriteLine($"Finished Processing {stringCount}");
                }

                Console.WriteLine($"Longest Word {longestWord}");
                Console.WriteLine($"Second Longest Word {secondlongestWord}");
                Console.WriteLine("Finished process file");
                Console.WriteLine($"Total of {stringCount} processed");
                //Console.ReadLine();
            }
        }

        private static void ProcessString(string inputString)
        {
            if (IsWordConcattenatedString(inputString))
            {
                if (inputString.Length > longestWord.Length)
                {
                    secondlongestWord = longestWord;
                    longestWord = inputString;
                }

                ConcatenatedStrings.Add(inputString);
            }
            InputList.Add(inputString);
            inputListOptmized.Add(inputString);
        }

        private static bool IsWordConcattenatedString(string inputString)
        {

            string startWord = GetStartingWord(inputString);

            if (startWord.Equals(inputString))
                return true;

            return !string.IsNullOrEmpty(startWord) ? IsWordConcattenatedString(inputString.Substring(startWord.Length)) : false;
        }

        private static string GetStartingWord(string inputString)
        {
            string startWord = string.Empty;

            foreach (String inputUniqueString in InputList)
            {
                if (inputUniqueString.Equals(inputString))
                {
                    return inputUniqueString;
                }

                if (inputString.StartsWith(inputUniqueString))
                {
                    if (inputUniqueString.Length > startWord.Length)
                    {
                        startWord = inputUniqueString;
                    }
                }
            }

            return startWord;
        }

        private static void ProcessStringUsingOptimizedList1()
        {
            string inputString;
            StringProcessor stringProcessor = new StringProcessor();
            using (StreamReader fileReader = new StreamReader("NET Test 00.txt"))
            {
                while (!String.IsNullOrEmpty(inputString = fileReader.ReadLine()))
                {
                    stringProcessor.ProcessString(inputString);
                    stringProcessor.StringProcessorStatics.coount++;
                    //Console.WriteLine($"Finished Processing {stringProcessor.StringProcessorStatics.coount}");
                }

                Console.WriteLine($"Longest Word {stringProcessor.StringProcessorStatics.longestWord}");
                Console.WriteLine($"Second Longest Word {stringProcessor.StringProcessorStatics.secondlongestWord}");
                Console.WriteLine("Finished processing file");
                Console.WriteLine($"Total count of words {StringProcessor.ConcatenatedStrings.Count} constructed of other words in the list");
                //Console.ReadLine();
            }
        }
    }



    class StringProcessor
    {
        private InputListOptmized inputListOptmized;
        private StringProcessorStatics stringProcessorStatics;
        public static List<String> ConcatenatedStrings = new List<string>();

        public StringProcessorStatics StringProcessorStatics
        {
            get { return stringProcessorStatics; }
        }

        public StringProcessor()
        {
            inputListOptmized = new InputListOptmized();
            stringProcessorStatics = new StringProcessorStatics();
        }

        public bool IsWordConcattenatedString(string inputString)
        {

            string startWord = inputListOptmized.GetMatchingWordStartingWith(inputString);

            if (startWord.Equals(inputString))
                return true;

            return !string.IsNullOrEmpty(startWord) ? IsWordConcattenatedString(inputString.Substring(startWord.Length)) : false;
        }

        public void ProcessString(string inputString)
        {
            if (IsWordConcattenatedString(inputString))
            {
                if (inputString.Length > stringProcessorStatics.longestWord.Length)
                {
                    stringProcessorStatics.secondlongestWord = stringProcessorStatics.longestWord;
                    stringProcessorStatics.longestWord = inputString;
                }

                ConcatenatedStrings.Add(inputString);
            }
            inputListOptmized.Add(inputString);
        }
    }

    class InputListOptmized
    {
        public InputListOptmized()
        {
            dictionary = new Dictionary<char, SortedSet<string>>();
        }

        public void Add(string inputString)
        {
            char key = inputString[0];
            SortedSet<string> inputBucket;
            if (dictionary.ContainsKey(key))
            {
                inputBucket = dictionary[key];
            }
            else
            {
                inputBucket = new SortedSet<string>();
                dictionary.Add(key, inputBucket);
            }

            inputBucket.Add(inputString);
        }

        public string GetMatchingWordStartingWith(string inputString)
        {
            string startWord = string.Empty;
            char key = inputString[0];
            if (dictionary.ContainsKey(key))
            {
                SortedSet<string> inputBucket = dictionary[key];
                foreach (String inputUniqueString in inputBucket)
                {
                    if (inputUniqueString.Equals(inputString))
                    {
                        return inputUniqueString;
                    }

                    if (inputString.StartsWith(inputUniqueString))
                    {
                        if (inputUniqueString.Length > startWord.Length)
                        {
                            startWord = inputUniqueString;
                        }
                    }
                }
            }

            return startWord;
        }

        private Dictionary<char, SortedSet<string>> dictionary;

    }
    class StringProcessorStatics
    {
        public string longestWord = string.Empty;
        public string secondlongestWord = string.Empty;
        public long coount = 0;
    }


    /*class InputListTreeOptimised
    {
        int _depth;
        //SortedDictionary<char, string> leafDictionary;
        SortedDictionary<char, SortedDictionary<char, List<string>>> leafBranchDictionary;
        SortedDictionary<char, SortedDictionary<char, SortedDictionary<char, List<string>>>> branchDictionary;
        SortedDictionary<char, SortedDictionary<char, SortedDictionary<char, List<string>>>> rootDictionary;
        public InputListTreeOptimised(int depth)
        {
            _depth = depth;
            Initialize();
        }

        private void Initialize()
        {
            rootDictionary = new SortedDictionary<char, SortedDictionary<char, SortedDictionary<char, List<string>>>>();
        }

        public void Add(string inputString)
        {
            SortedDictionary<char, List<string>> leafDictionary = GetLeafDictionary(inputString);
            int depthLength = _depth > inputString.Length ? _depth : inputString.Length;
            char key = inputString[depthLength - 1];
            List<string> list;

            if (leafDictionary.ContainsKey(key))
            {
                list = leafDictionary[key];

            }
            else
            {
                list = new List<string>();
                leafDictionary.Add(key, list);
            }
            list.Add(inputString);
        }

        private SortedDictionary<char, List<string>> GetLeafDictionary(string inputString)
        {
            SortedDictionary<char, List<string>> leafDictionary = new SortedDictionary<char, List<string>>();

            SortedDictionary<char, SortedDictionary<char, SortedDictionary<char, List<string>>>> contextDictionary = rootDictionary;

            for (int i = 0; i < inputString.Length; i++)
            {
                if (contextDictionary.ContainsKey(inputString[i]))
                {

                }
                else
                {

                }
            }


            return leafDictionary;

        }



        public string GetMatchingWordStartingWith(string inputString)
        {

        }


    }*/


}