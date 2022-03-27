using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PolishNotation
{
    public class Program
    {


        public static void Main()
        {
            Console.WriteLine();
            Console.ReadLine();
        }

        public static void MainCycle()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Что делаем?");
                Console.WriteLine("[1]  \n[2]  \n \n\n\n\n[x] Выход");
                var c = Console.ReadLine();
                if (c[0] == 'x') break;
                switch (c[0])
                {
                    case '1':
                        Console.Clear();
                        Console.Clear();
                        break;
                    case '2':
                        Console.Clear();
                        Console.Clear();
                        break;
                    case 'i':
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("НЕВЕРНАЯ КОММАНДА!");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }

            }
        }
    }
}
