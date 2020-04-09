using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task
{
    class task3
    {
        static void Main(string[] args)
        {
            List<StreamReader> inputFiles = default;
            List<float> result = new List<float>();
            try
            {
                inputFiles = ParseConsoleInput(args);
                foreach (var inputFile in inputFiles)
                {
                    List<float> readedInput = ReadInputToList(inputFile);
                    if (result.Count == 0)
                        result = readedInput;
                    else
                        result = AggregateSameCountList(result, readedInput);
                }
                int maxIndex = 0;
                int rCount = result.Count;
                for (int i = 0; i < rCount; i++)
                {
                    if (result[i] > result[maxIndex])
                        maxIndex = i;
                }
                Console.WriteLine(maxIndex+1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        static private List<StreamReader> ParseConsoleInput(string[] args)
        {
            if (args.Length > 1)
                throw new Exception("Ожидался один аргумент - имя каталога");
            List<StreamReader> readers = new List<StreamReader>();
            List<FileInfo> files = new List<FileInfo>();
            DirectoryInfo directory = new DirectoryInfo(args[0]);
            
            if (directory.Exists)
            {
                files.AddRange(directory.GetFiles("Cash?.txt"));
                if (files.Count != 0)
                    foreach (var file in files)
                        readers.Add(new StreamReader(file.FullName));

                if (readers.Count != 0)
                    return readers;
                throw new Exception("Файлы не найден.");
            }
            
            throw new Exception("Каталог не найден.");
        }

        static private List<float> AggregateSameCountList(List<float> source, List<float> adding)
        {
            int sCount = source.Count;
            int aCount = adding.Count;
            if (sCount != aCount)
                throw new Exception($@"Во входных данных совпадает количество элементо - {sCount} != {aCount}");
            List<float> result = new List<float>();
            for (int i = 0; i < sCount; i++)
                result.Add(source[i] + adding[i]);
            return result;
        }

        static private List<float> ReadInputToList(StreamReader inputFile)
        {
            short linesCounter = 1;
            List<float> result = new List<float>();
            while (!inputFile.EndOfStream)
            {
                float inputFloat;
                string currentLine = inputFile.ReadLine();
                linesCounter++;
                if (!float.TryParse(currentLine, NumberStyles.Any, CultureInfo.InvariantCulture, out inputFloat))
                    throw new Exception($@"В файле присутствует запись, не являющаяся целым числом в пределах от {float.MinValue} до {float.MaxValue}. ");
                result.Add(inputFloat);
            }
            return result;
        }
    }
}
