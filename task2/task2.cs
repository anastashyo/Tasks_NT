using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task
{
    class task2
    {
        static void Main(string[] args)
        {
            List<StreamReader> inputFiles = default;
            List<Point> pointsToCheck = default;
            Rectangle rectangle = default;
            try
            {
                inputFiles = ParseConsoleInput(args);
                ReadInputFiles(inputFiles, out rectangle, out pointsToCheck);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (pointsToCheck != null && rectangle != null)
            {
                foreach (Point point in pointsToCheck)
                {
                    Console.WriteLine((int)DefinePointPosition(point, rectangle));
                }
                Console.WriteLine("Ready!");
            }

        }

        struct Point
        {
            public float x;
            public float y;
        }

        enum PointPosition
        {
            vertex,
            side,
            inside,
            outside
        }

        class Rectangle
        {
            public readonly Point LeftTop, RightTop, LeftBottom, RightBottom;

            public Rectangle(Point left_bottom, Point left_top, Point right_top, Point right_bottom)
            {
                LeftTop = left_top;
                RightTop = right_top;
                LeftBottom = left_bottom;
                RightBottom = right_bottom;
            }

        }

        static private List<StreamReader> ParseConsoleInput(string[] args)
        {
            if (args.Length > 2)
                throw new Exception("Ожидался два аргумент - имя файла 1 и имя файла 2.");
            List<StreamReader> readers = new List<StreamReader>();
            foreach (string arg in args)
            {
                FileInfo file = new FileInfo(arg);
                if (file.Exists)
                {
                    StreamReader reader = new StreamReader(file.FullName);
                    readers.Add(reader);
                }
            }
            if (readers.Count != 0)
                return readers;
            throw new Exception("Файл не найден.");
        }

        static private void ReadInputFiles(List<StreamReader> inputFiles, out Rectangle rectangle, out List<Point> pointsToCheck)
        {
            pointsToCheck = new List<Point>();
            List<Point> rectPoints = new List<Point>();

            short linesCounter = 1;
            for (int i = 0; i < 2; i++)
            {
                while (!inputFiles[i].EndOfStream)
                {
                    Point point = new Point();
                    string[] line = inputFiles[i].ReadLine().Split();
                    linesCounter++;

                    if (line.Length > 2)
                        throw new Exception($@"В строке {linesCounter} более двух значений.");
                    if (!float.TryParse(line[0],NumberStyles.Any, CultureInfo.InvariantCulture, out point.x) || 
                        !float.TryParse(line[1], NumberStyles.Any, CultureInfo.InvariantCulture, out point.y))
                        throw new Exception($@"В строке {linesCounter} одно из значений не является числом с плавающей точкой.");

                    if (i == 0)
                        rectPoints.Add(point);
                    else
                        pointsToCheck.Add(point);
                }
            }

            rectangle = new Rectangle(rectPoints[0], rectPoints[1], rectPoints[2], rectPoints[3]);
        }

        static private bool CheckInside(Point p, Point rectLeftTop, Point rectRightBottom) 
            => rectLeftTop.x < p.x && rectRightBottom.x > p.x && rectLeftTop.y > p.y && rectRightBottom.y < p.y;

        static private bool CheckPointBelongsToLine(Point checkingPoint, Point lineStart, Point lineEnd)
        {
            double dxc = checkingPoint.x - lineStart.x;
            double dyc = checkingPoint.y - lineStart.y;

            double dxl = lineEnd.x - lineStart.x;
            double dyl = lineEnd.y - lineStart.y;

            double cross = dxc * dyl - dyc * dxl;
            if (cross != 0)
                return false;

            if (Math.Abs(dxl) >= Math.Abs(dyl))
                return dxl > 0 ?
                  lineStart.x <= checkingPoint.x && checkingPoint.x <= lineEnd.x :
                  lineEnd.x <= checkingPoint.x && checkingPoint.x <= lineStart.x;
            else
                return dyl > 0 ?
                  lineStart.y <= checkingPoint.y && checkingPoint.y <= lineEnd.y :
                  lineEnd.y <= checkingPoint.y && checkingPoint.y <= lineStart.y;
        }

        static private PointPosition DefinePointPosition(Point pointToCheck, Rectangle rectangle)
        {
            if (pointToCheck.Equals(rectangle.LeftTop) || 
                pointToCheck.Equals(rectangle.LeftBottom) || 
                pointToCheck.Equals(rectangle.RightTop) || 
                pointToCheck.Equals(rectangle.RightBottom))
                return PointPosition.vertex;

            if (CheckPointBelongsToLine(pointToCheck, rectangle.LeftBottom,rectangle.LeftTop) ||
                CheckPointBelongsToLine(pointToCheck, rectangle.LeftBottom, rectangle.RightBottom) ||
                CheckPointBelongsToLine(pointToCheck, rectangle.LeftTop, rectangle.RightTop) ||
                CheckPointBelongsToLine(pointToCheck, rectangle.RightBottom, rectangle.RightTop))
                return PointPosition.side;

            if (CheckInside(pointToCheck, rectangle.LeftTop, rectangle.RightBottom))
                return PointPosition.inside;

            return PointPosition.outside;
        }
    }
}
