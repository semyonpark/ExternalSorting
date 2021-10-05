using System;
using System.Collections.Generic;
using System.IO;
using Data;
using System.Diagnostics;

namespace TestFileGenerator
{
    class Program
    {
        
        private const long MaxFileSize = 8589934592; //524288000;  (500МБ) // Размер файла в Байтах

        private const int MaxStringRepeatCount = 5; // Максимальное кол-во повторений одной и той же строки.

        private const float EqualStringsPercent = 0.5f; // Процент повторяющихся строк в зависимости от возможного максим. кол-ва строк.

        private const string DirectoryName = "Semyon_Park_Test_Task";

        private const string FileName = "UnsortedList.txt";

        private static string _resultPath;

        private static Random _random;

        private static string _directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DirectoryName);
        //private static string _directoryPath = Path.Combine("e:\\", DirectoryName);


        static void Main(string[] args)
        {
            Debug.WriteLine($"Start: {DateTime.Now.ToString("HH:mm:ss")}");

            
            IOUtils.CreateDirectory(_directoryPath);

            _resultPath = Path.Combine(_directoryPath, FileName);

            _random = new Random();

            // миним. кол-во строк (размер файла / максим. кол-во символов в строке)
            long theorMinLinesCount = MaxFileSize / SortItem.MaxLineLegth;

            // миним. кол-во строк которые будут повторяться
            long equalStringPhraseCount = (long)(theorMinLinesCount * EqualStringsPercent / 100);

            Dictionary<long, SortItem> repeatStrings = RepeatStrings(theorMinLinesCount, equalStringPhraseCount);

            long currentFileSize = 0;
            long lineIndex = 0;
            using (StreamWriter file = new StreamWriter(_resultPath, false))
            {
                bool isFileFull = false;
                while (!isFileFull)
                {
                    SortItem sortItem = null;
                    if (repeatStrings.ContainsKey(lineIndex))
                    {
                        sortItem = repeatStrings[lineIndex];
                    }
                    else
                    {
                        // Если это неповторяющаюся часть String, то генерим новый объект SortItem.
                        sortItem = new SortItem(_random.NextString(), _random.Next(int.MaxValue));
                    }
                    string strLine = sortItem.ToString();
                    currentFileSize += (strLine + file.NewLine).Length;

                    // Проверяем файл, на то превысил или нет макс. размер файла
                    if (currentFileSize > MaxFileSize)
                    {
                        isFileFull = true;
                    }
                    else
                    {
                        file.WriteLine(strLine);
                    }

                }
            }

            Debug.WriteLine($"Finish: {DateTime.Now.ToString("HH:mm:ss")}");
        }

        /// <summary>
        /// Возвращает Словарь, повторяющихся строк и его порядок в файле
        /// </summary>
        /// <param name="theorMinLinesCount">Возможное минимальное кол-во строк</param>
        /// <param name="equalStringPhraseCount">Кол-во повторяющихся фраз</param>
        /// <returns>Словарь - порядковый номер строки в файле и объект строки в файле</returns>
        private static Dictionary<long, SortItem> RepeatStrings(long theorMinLinesCount, long equalStringPhraseCount)
        {
            Debug.WriteLine($"RepaeatStrings Start: {DateTime.Now.ToString("HH:mm:ss")}");

            Dictionary<long, SortItem> result = new Dictionary<long, SortItem>();

            for (int i = 0; i < equalStringPhraseCount; i++)
            {
                string stringPart = _random.NextString();

                int stringRepeatCount = _random.Next(1, MaxStringRepeatCount);
                List<int> usedNumbers = new List<int>();
                for (int j = 0; j < stringRepeatCount; j++)
                {
                    bool isLineIndexUsed = true;
                    long lineIndex = 0;
                    while (isLineIndexUsed)
                    {
                        lineIndex = _random.NextLong(1, theorMinLinesCount);
                        isLineIndexUsed = result.ContainsKey(lineIndex);
                    }
                    int digitalPart = _random.Next(1, int.MaxValue, usedNumbers);

                    result.Add(lineIndex, new SortItem(stringPart, digitalPart));
                }
            }

            Debug.WriteLine($"RepeatStrings Finish: {DateTime.Now.ToString("HH:mm:ss")}");

            return result;
        }

        private void Test1 { 
        }

        private void Test2 { }
    

        private void Test3 { }


        private void Main { }
    }

}
