using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace task
{

    class Task1
    {
        public static short LinesMaximum { get; set; } = 1000;
        static void Main(string[] args)
        {
            StreamReader inputFile = default;
            List<short> inputData = default;
            try
            {
                inputFile = ParseConsoleInput(args);
                inputData = ReadInputFile(inputFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (inputData != null)
            {
                inputData.Sort();
                Console.WriteLine("{0:0.00}",Calculate90Percentil(inputData));
                Console.WriteLine("{0:0.00}", CalculateMedian(inputData));
                Console.WriteLine("{0:0.00}", inputData[inputData.Count - 1]);
                Console.WriteLine("{0:0.00}", inputData[0]);
                Console.WriteLine("{0:0.00}", CalculateMiddle(inputData));
            }

        }

        private static double Calculate90Percentil(List<short> inputData)
        {
            int count = inputData.Count;
            double percentile = (count - 1) * 0.9 + 1;
            
            if (percentile == 1d) 
                return inputData[0];
            if (percentile == count) 
                return inputData[count - 1];

            int index = (int)percentile;
            double d = percentile - index;
            return inputData[index - 1] + d * (inputData[index] - inputData[index - 1]);

        }

        private static float CalculateMedian(List<short> inputData)
        {
            int count = inputData.Count;
            float median = 0;
            if (count != 0)
                if (count % 2 == 0)
                    median = inputData[(count + 1) / 2];
                else
                    median = (inputData[count / 2] + inputData[(count / 2) + 1]) / 2;
            return median;
        }

        private static float CalculateMiddle(List<short> inputData)
        {
            float middle = 0;
            foreach (short item in inputData)
                middle += item;
            middle /= inputData.Count;
            return middle;
        }

        static private StreamReader ParseConsoleInput(string[] args)
        {
            if (args.Length > 1)
                throw new Exception("Ожидался один аргумент - имя файла.");
            
            FileInfo file = new FileInfo(args[0]);
            if (file.Exists)
            {
                StreamReader reader = new StreamReader(file.FullName);
                return reader;
            }

            throw new Exception("Файл не найден.");
        }

        static private List<short> ReadInputFile(StreamReader inputFile)
        {
            List<short> inputData = new List<short>();
            short linesCounter = 1;
            while (!inputFile.EndOfStream)
            {
                if (linesCounter > LinesMaximum)
                    throw new Exception($@"Длина входного файла превышает {LinesMaximum} строк.");
                short inputShort;
                string currentLine = inputFile.ReadLine();
                linesCounter++;
                if (!short.TryParse(currentLine, out inputShort))
                    throw new Exception($@"В файле присутствует запись, не являющаяся целым числом в пределах от {short.MinValue} до {short.MaxValue}. ");
                inputData.Add(inputShort);
            }
            return inputData;
        }
    }
}
