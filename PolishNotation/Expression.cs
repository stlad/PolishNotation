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

        public List<HistoryNote> SolutionHistory { get; private set; }

        public static Dictionary<string, int> Priority = new Dictionary<string, int>
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
            { "sqrt",4 },
            //-----------
            { "tg",4 },
            {"abs",4 },
            {"acos",4 },
            {"asin",4 },
            {"atan",4 }
        };
        public Expression(string infFixExpr)
        {
            SolutionHistory = new List<HistoryNote>();
            InFix = infFixExpr;
            PostFix = InFixToPostFix(InFix);
        }


        private static List<string> InFixToPostFix(string infExpr)
        {
            var res = new List<string>();
            var operatorStack = new Stack<string>();
            for(int i=0; i<infExpr.Length; i++)
            {
                var currentChar = infExpr[i];

                if (Char.IsDigit(currentChar))
                    res.Add(getNumberFromStr(infExpr, ref i));
                else if (Char.IsLetter(currentChar))
                    operatorStack.Push(getFuncFromStr(infExpr, ref i));
                else if (currentChar == '(')
                    operatorStack.Push(Convert.ToString(currentChar));
                else if (currentChar == ')')
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                        res.Add(operatorStack.Pop());
                    operatorStack.Pop();
                }
                else if (Priority.ContainsKey(Convert.ToString( currentChar)))
                {
                    var op = Convert.ToString( currentChar);
                    if (op == "-" && (i == 0 || (i > 1 && Priority.ContainsKey(Convert.ToString( infExpr[i-1])))))
                        op = "~";


                    while (operatorStack.Count > 0 && (Priority[operatorStack.Peek()] >= Priority[op]))
                        res.Add(operatorStack.Pop());

                    operatorStack.Push(op);
                }
            }

            foreach (var op in operatorStack)
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
                case "tg":
                    return Math.Tan(a);
                case "abs":
                    return Math.Abs(a);
                case "acos":
                    return Math.Acos(a);
                case "asin":
                    return Math.Asin(a);
                case "atan":
                    return Math.Atan(a);
                default: return 0;
            }
        }

        public double Calculate(bool isSaving)
        {
            var stack = new Stack<double>();
            int counter = 0;
            int step = 0;
            for (int i = 0; i < PostFix.Count; i++)
            {
                step++;
                if (isSaving) MakeHistoryNote(step, i, stack);
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

                    if(Char.IsLetter(PostFix[i][0]))
                    {
                        var last = stack.Count > 0 ? stack.Pop() : 0;
                        stack.Push(ExecuteOperator(last, 0, PostFix[i]));
                        continue;
                    }
                    var second = stack.Count > 0 ? stack.Pop() : 0;
                    var first = stack.Count > 0 ? stack.Pop() : 0;

                    stack.Push(ExecuteOperator(first, second, Convert.ToString(PostFix[i])));
                }
            }

            step++;
            Result = stack.Peek();
            return stack.Pop();
            
        }

        public string GetPostFixExpression() => string.Join(" ", this.PostFix);

        private void MakeHistoryNote(int step, int index, Stack<double> currentStack)
        {
            var leftStr = PostFix.Skip(index).ToList();
            var stackStr = string.Join(", ", currentStack);
            
            SolutionHistory.Add(new HistoryNote(step, string.Join(", ", leftStr), stackStr));
        }




    }




    public class HistoryNote
    {
        public int Step { get; private set; }
        public string StringLeft { get; private set; }
        public string CurrentStack { get; private set; }

        public HistoryNote(int s, string str, string stackLeft)
        {
            Step = s;
            StringLeft = str;
            CurrentStack = stackLeft;
        }
    }
        
}
