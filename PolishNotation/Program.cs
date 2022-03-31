using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;


namespace PolishNotation
{
    public class Program
    {
        public static void Main()
        {
            MainCycle();
        }

        public static void MainCycle()
        {
            var isSaving = false;
            while (true)
            {
                var option = isSaving ? "Вкл" : "Выкл";
                Console.Clear();
                Console.WriteLine($"[i] - информация\n[x] - выход\n[s] - сохранение и сериализация ({option})");
                Console.WriteLine("\n\nВведите команду или выражение...");
                var c = Console.ReadLine();
                if (c[0] == 'x') break;
                if (c[0] == 'i')
                {
                    GetInfo();
                    continue;
                }
                if(c[0] == 's')
                {
                    isSaving = !isSaving;
                    continue;
                }

                Console.Clear();

                var exp = new Expression(c);

                var res = exp.Calculate(isSaving);
                Console.WriteLine($"Выражение:         {exp.InFix}");
                Console.WriteLine($"Обратная нотация:  {exp.GetPostFixExpression()}");
                Console.WriteLine($"Результат:         {exp.Result}");

                if(isSaving)
                {
                    var maxLen = exp.SolutionHistory[0].StringLeft.Length >= 14 ?
                        exp.SolutionHistory[0].StringLeft.Length : 14;
                    Console.WriteLine("\n\n\nИстория операций:\n\n");
                    Console.Write("Шаг\t|Остаток строки");
                    for (int i = 0; i < maxLen; i++) Console.Write(" ");


                    Console.Write("|Стэк\n\n");
                    foreach(var note in exp.SolutionHistory)
                    {

                        Console.Write($"{note.Step}\t{note.StringLeft}");
                        for (int i = 0; i < maxLen - note.StringLeft.Length; i++)
                            Console.Write(" ");
                        Console.Write($"\t\t{note.CurrentStack}\n");
                    }

                    Console.WriteLine("\n\n\nВведите название файла (.json)");
                    var filename = Console.ReadLine(); 
                    var serializer = JsonSerializer.Serialize(exp);
                    var file = new System.IO.StreamWriter(filename);
                    file.Write(serializer);
                    file.Close();
                }
                Console.WriteLine("\n---------------Нажмите любую кнопку---------------");
                Console.ReadKey();
                //switch (c[0])
                //{
                //    case '1':
                //        Console.Clear();
                //        Console.Clear();
                //        break;
                //    case '2':
                //        Console.Clear();
                //        Console.Clear();
                //        break;
                //    case 'i':
                //        Console.Clear();
                //        break;
                //    default:
                //        Console.WriteLine("НЕВЕРНАЯ КОММАНДА! Нажмите любую кнопку");
                //        Console.ReadKey();
                //        Console.Clear();
                //        break;
                //}

            }
        }

        private static void GetInfo()
        {
            Console.WriteLine("доступные функции и операторы:\n");
            foreach(var op in Expression.Priority.Keys)
            {
                Console.WriteLine(op);
            }
            Console.WriteLine("\n---------------Нажмите любую кнопку---------------");
            Console.ReadKey();
            
        }
    }
}
