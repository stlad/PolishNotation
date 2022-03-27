using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolishNotation
{
    public class Expression
    {
        public string InFix { get; private set; }

        public List<string> PostFix { get; private set; }


        public double Result { get; private set; }

        private static Dictionary<string, int> Priority = new Dictionary<string, int>
        {
            { "(", 0 },
            { "+", 1 },
            { "-", 1 },
            { "*", 2 },
            { "/", 2 },
            { "^", 3 },
            { "~", 4 }, //унарный минус
            { "sin", 4 },
            { "cos", 4 },
            { "sqrt",4 }
        };
        public Expression(string infFixExpr)
        {
            InFix = infFixExpr;
            PostFix = InFixToPostFix(InFix);
        }


        private static List<string> InFixToPostFix(string infExpr)
        {
            var res = new List<string>();
            var stack = new Stack<string>();
            for(int i=0; i<infExpr.Length; i++)
            {
                var currentChar = infExpr[i];

                if (Char.IsDigit(currentChar))
                    res.Add(getNumberFromStr(infExpr, ref i));
                else if (Char.IsLetter(currentChar))
                    stack.Push(getFuncFromStr(infExpr, ref i));
                else if (currentChar == '(')
                    stack.Push(Convert.ToString(currentChar));
                else if (currentChar == ')')
                {
                    while (stack.Count > 0 && stack.Peek() != "(")
                        res.Add(stack.Pop());
                    stack.Pop();
                }
                else if (Priority.ContainsKey(Convert.ToString( currentChar)))
                {
                    var op = Convert.ToString( currentChar);
                    if (op == "-" && (i == 0 || (i > 1 && Priority.ContainsKey(Convert.ToString( infExpr[i-1])))))
                        op = "~";


                    while (stack.Count > 0 && (Priority[stack.Peek()] >= Priority[op]))
                        res.Add(stack.Pop());

                    stack.Push(op);
                }
            }

            foreach (var op in stack)
                res.Add(op);
            return res;
        }

        private static string getNumberFromStr(string str, ref int index)
        {
            var res = "";

            while(index < str.Length)
            {
                var c = str[index];

                if (Char.IsDigit(c))
                    res += c;
                else
                {
                    index--;
                    break;
                }

                index++;
            }

            return res;
        }

        private static string getFuncFromStr(string str, ref int index)
        {
            var res = "";

            while (index < str.Length)
            {
                var c = str[index];

                if (Char.IsLetter(c))
                    res += c;
                else
                {
                    index--;
                    break;
                }

                index++;
            }

            return res;
        }

        private static double ExecuteOperator(double a, double b, string op)
        {
            double res = 0;
            switch (op)
            {
                case "+": return a + b;
                case "-": return a - b;
                case "*": return a * b;
                case "/":
                    if (b == 0) throw new DivideByZeroException();
                    return a / b;
                case "^": return Math.Pow(a,b);
                case "sin": 
                    return Math.Sin(a);
                case "cos":
                    return Math.Cos(a);
                case "sqrt":
                    return Math.Sqrt(a);
                default: return 0;
            }
        }

        public double Make()
        {
            var stack = new Stack<double>();
            int counter = 0;
            for (int i = 0; i < PostFix.Count; i++)
            {
                if (Char.IsDigit(PostFix[i][0]))
                    stack.Push(Convert.ToDouble(PostFix[i]));
                else if(Priority.ContainsKey(PostFix[i]))
                {
                    counter++;
                    if (PostFix[i] == "~")
                    {
                        var last = stack.Count > 0 ? stack.Pop() : 0;
                        stack.Push(ExecuteOperator(0, last, "-"));
                        continue;
                    }

                    if(PostFix[i] == "sin")
                    {
                        var last = stack.Count > 0 ? stack.Pop() : 0;
                        stack.Push(ExecuteOperator(last, 0, "sin"));
                        continue;
                    }

                    if (PostFix[i] == "cos")
                    {
                        var last = stack.Count > 0 ? stack.Pop() : 0;
                        stack.Push(ExecuteOperator(last, 0, "cos"));
                        continue;
                    }

                    if (PostFix[i] == "sqrt")
                    {
                        var last = stack.Count > 0 ? stack.Pop() : 0;
                        stack.Push(ExecuteOperator(last, 0, "sqrt"));
                        continue;
                    }

                    var second = stack.Count > 0 ? stack.Pop() : 0;
                    var first = stack.Count > 0 ? stack.Pop() : 0;

                    stack.Push(ExecuteOperator(first, second, Convert.ToString(PostFix[i])));
                }
            }


            return stack.Pop();
            
        }





        /*
         Проходим постфиксную запись;

При нахождении числа, парсим его и заносим в стек;

При нахождении бинарного оператора, берём два последних значения из стека в обратном порядке;

При нахождении унарного оператора, в данном случае - унарного минуса, берём последнее значение из стека и вычитаем его из нуля, 
        так как унарный минус является правосторонним оператором;

Последнее значение, после отработки алгоритма, является решением выражения.*/
    }
}
