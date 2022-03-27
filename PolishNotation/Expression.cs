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
    }
}
