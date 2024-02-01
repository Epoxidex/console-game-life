using System;
using System.Collections.Generic;
using System.Text;

namespace GameLife
{
    public class Game
    {
        private const char AliveSymbol = '█';
        private const char DeadSymbol = ' ';
        private const char BorderSymbol = '#';

        public int Width { get; private set; }
        public int Height { get; private set; }

        private bool[,] _field;
        
        public Game(int width, int height)
        {
            Width = width;
            Height = height;

            _field = new bool[height, width];

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    _field[i, j] = false;
        }
        public void RunGame()
        {
            while (true)
            {
                PrintField();
                CalcNextGen();
                Console.ReadKey();
            }
        }

        public void PrintField()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(new String(BorderSymbol, Width + 2));

            for (int i = 0; i < Height; i++)
            {
                sb.Append(BorderSymbol);
                for (int j = 0; j < Width; j++)
                    sb.Append(_field[i, j] ? AliveSymbol : DeadSymbol);
                sb.AppendLine($"{BorderSymbol}");
            }

            sb.AppendLine(new String(BorderSymbol, Width + 2));

            Console.SetCursorPosition(0, 0);
            Console.Write(sb.ToString());
        }

        public void RandomFill(double frequency = 2)
        {
            Random rnd = new Random();
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    _field[i, j] = rnd.NextDouble() < 1 / frequency ? true : false;
        }

        public void CalcNextGen()
        {
            bool[,] newField = new bool[Height, Width];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int count = 0;

                    foreach (var offset in GetNeighborhoodOffsets())
                    {
                        int xn = (x + offset.Item1 + Width) % Width;
                        int yn = (y + offset.Item2 + Height) % Height;

                        if (_field[yn, xn]) count++;
                    }

                    if (_field[y, x] && (count == 2 || count == 3))
                        newField[y, x] = true;
                    else if (!_field[y, x] && count == 3)
                        newField[y, x] = true;
                }
            }

            _field = newField;
        }

        private IEnumerable<(int, int)> GetNeighborhoodOffsets()
        {
            yield return (-1, -1);
            yield return (-1, 0);
            yield return (-1, 1);
            yield return (0, -1);
            yield return (0, 1);
            yield return (1, -1);
            yield return (1, 0);
            yield return (1, 1);
        }

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var game = new Game(100, 20);
            game.RandomFill(10);
            game.RunGame();
        }
    }
}
