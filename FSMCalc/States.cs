using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMCalc
{
    public delegate void ChangeDisplay(string result);

    public class States
    {
        public ChangeDisplay invoker;

        public string number = "";
        public string result = "";
        public double answer = 0;
        public char op = '.';

        public char[] operations = { '+', '-', '*', '/', 'n'};
        public char[] revers = { '±' };
        public char[] digits = { '0', '1','2','3','4','5','6','7','8','9', 'π' };
        public char[] nozerodigits = { '1','2','3','4','5','6','7','8','9', 'π' };
        public char[] zero = { '0' };
        public char[] separators = { '.', ',' };
        public char[] clear = { 'c' };
        public char[] equals = { '=' };
        public char[] root = { '√', '²' };

        public string stateName = "Zero";

        public void Process(char item)
        {
            switch (stateName)
            {
                case "Zero":
                    Zero(item, false);
                    break;
                case "AccumulateDigits":
                    AccumulateDigits(item, false);
                    break;            
                case "ComputePending":
                    ComputePending(item, false);
                    break;
                case "Compute":
                    Compute(item, false);
                    break;
            }
        }

        public void Zero(char item, bool isInput)
        {
            if (isInput)
            {
                result = "";
                number = "";
                op = '.';
                answer = 0;
                stateName = "Zero";
                invoker.Invoke(result);
            }
            else
            {
                if (separators.Contains(item))                
                    AccumulateDigits(item, true);                
                else if (digits.Contains(item))                
                    AccumulateDigits(item, true);                
            }
        }

        public void AccumulateDigits(char item, bool isInput)
        {
           if (isInput)
            {
                if (item == '.')
                {
                    if (result.Contains(","))
                        goto next;
                    if (result == "")
                        result = "0";
                    result += ',';
                }
                else if (item == '0')
                {
                    if (result != "0")
                        result += item;
                }
                else if (item == '±')
                {
                    if (result == "0")
                        goto next;
                    if (result.Contains("-"))
                        result = (-double.Parse(result)).ToString();
                    else
                        result = '-' + result;
                }
                else if (item == '√')
                    result = (Math.Sqrt(double.Parse(result))).ToString();
                else if (item == '²')
                    result = (double.Parse(result) * double.Parse(result)).ToString();
                else if (item == 'π')
                    result = (3.14159).ToString();
                else if (result == "0")
                {
                    result = "";
                    result += item;
                }
                else
                    result += item;
                stateName = "AccumulateDigits";
                invoker.Invoke(result);
            next:;
            }
            else
            {
                if (digits.Contains(item) || separators.Contains(item) || revers.Contains(item) || root.Contains(item))                
                    AccumulateDigits(item, true);                               
                else if (operations.Contains(item))                
                    ComputePending(item, true);                
                else if (equals.Contains(item))                
                    Compute(item, true);                
                else if (clear.Contains(item))                
                    Zero(item, true);                                
            }
        }

        public void ComputePending(char item, bool isInput)
        {
            if (isInput)
            {
                Tasks();                                                                          
                invoker.Invoke(answer.ToString());
                op = item;
                result = "";
                stateName = "ComputePending";
            }
            else
            {
                if (digits.Contains(item))                
                    AccumulateDigits(item, true);               
                else if (clear.Contains(item))                
                    Zero(item, true);                
            }
        }
        public void Compute(char item, bool isInput)
        {
            if (isInput)
            {
                Tasks();               
                result = answer.ToString();                
                answer = 0;
                op = '.';
                stateName = "Compute";
                invoker.Invoke(result);
            }
            else
            {
                if (nozerodigits.Contains(item))
                {
                    result = "";
                    invoker.Invoke(result);
                    AccumulateDigits(item, true);
                }
                else if (zero.Contains(item))                
                    Zero(item, true);                
                else if (operations.Contains(item))                
                    ComputePending(item, true);                
                else if (nozerodigits.Contains(item) || separators.Contains(item) || revers.Contains(item) || root.Contains(item))
                    AccumulateDigits(item, true);
                else if (clear.Contains(item))
                    Zero(item, true);
                else if (equals.Contains(item))
                    Compute(item, true);
            }
        }

        public void Tasks()
        {
            number = result;
            double num = double.Parse(number);
            if (op == '.')
                answer += num;
            if (op == '+')
                answer += num;
            else if (op == '-')
                answer -= num;
            else if (op == '*')
                answer *= num;
            else if (op == '/')
                answer /= num;
            else if (op == 'n')
                answer = Math.Pow(answer, num);
        }
    }
}
