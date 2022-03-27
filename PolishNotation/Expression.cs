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

        public string PostFix { get; private set; }

        public double Result { get; private set; }

        private static Dictionary<char, int> Priority = new Dictionary<char, int>
        {
            { '(', 0 },
            { '+', 1 },
            { '-', 1 },
            { '*', 2 },
            { '/', 2 },
            { '^', 3 },
            { '~', 4 } //унарный минус
        };
        public Expression(string infFixExpr)
        {
            InFix = infFixExpr;
            PostFix = InFixToPostFix(InFix);
        }

        private static string InFixToPostFix(string infExpr)
        {
            var res = "";
            var stack = new Stack<char>();

            for(int i=0; i<infExpr.Length; i++)
            {
                var currentChar = infExpr[i];

                if (Char.IsDigit(currentChar))
                    res += getNumberFromStr(infExpr, ref i) + " ";
                else if (currentChar == '(')
                    stack.Push(currentChar);
                else if (currentChar == ')')
                {
                    while (stack.Count > 0 && stack.Peek() != '(')
                        res += stack.Pop();
                    stack.Pop();
                }
                else if(Priority.ContainsKey(currentChar))
                {
                    char op = currentChar;
                    if (op == '-' && (i == 0 || (i > 1 && Priority.ContainsKey(infExpr[i - 1]))))
                        op = '~';


                    while (stack.Count > 0 && (Priority[stack.Peek()] >= Priority[op]))
                        res += stack.Pop();

                    stack.Push(op);
                }
            }

            foreach (var op in stack)
                res += op;
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

        public static string getFuncFromStr(string str, ref int index)
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

        private static double ExecuteOperator(double a, double b, char op)
        {
            double res = 0;
            switch (op)
            {
                case '+': return a + b;
                case '-': return a - b;
                case '*': return a * b;
                case '/':
                    if (b == 0) throw new DivideByZeroException();
                    return a / b;
                case '^': return Math.Pow(a,b);
                default: return 0;
            }
        }

        public double Make()
        {
            var stack = new Stack<double>();
            int counter = 0;
            for(int i=0; i< PostFix.Length; i++)
            {
                if (Char.IsDigit(PostFix[i]))
                    stack.Push(Convert.ToDouble(getNumberFromStr(PostFix, ref i)));
                else if(Priority.ContainsKey(PostFix[i]))
                {
                    counter++;

                    if(PostFix[i] =='~')
                    {
                        var last = stack.Count>0 ? stack.Pop() : 0;
                        stack.Push(ExecuteOperator(0, last, '-'));
                        continue;
                    }



                    var second = stack.Count > 0 ? stack.Pop() : 0;
                    var first = stack.Count > 0 ? stack.Pop() : 0;


                    stack.Push(ExecuteOperator(first, second, PostFix[i]));
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
