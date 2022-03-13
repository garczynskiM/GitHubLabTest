using System;
using System.Collections.Generic;

namespace KataStringCalculator
{
    public class Calculator
    {
        public int Add(string input)
        {
            int result = 0, tempValue, iterator = 0;
            string customSeparatorsString = "//";
            string leftSeparatorClosing = "[";
            string rightSeparatorClosing = "]";
            List<string> separators = new List<string>();
            separators.Add(",");
            separators.Add("\n");
            if(input.Contains(customSeparatorsString))
            {
                iterator = customSeparatorsString.Length;
                if(input.Contains(leftSeparatorClosing) && input.Contains(rightSeparatorClosing))
                {
                    iterator+=leftSeparatorClosing.Length;
                    int length = 0;
                    while(!char.IsDigit(input[iterator + length]))
                    {
                        length++;
                    }
                    
                    string separatorPart = input.Substring(iterator, length - rightSeparatorClosing.Length);
                    
                    foreach(string separator in separatorPart.Split(rightSeparatorClosing+leftSeparatorClosing, StringSplitOptions.RemoveEmptyEntries))
                    {
                        separators.Add(separator);
                    }
                    iterator += length;
                }
                else
                {
                    separators.Add(input[iterator].ToString());
                    iterator++;
                }       
            }
            string[] numbers = input.Substring(iterator).Split(separators.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach(string number in numbers)
            {
                tempValue = Int32.Parse(number);
                if (tempValue < 0)
                    throw new ArgumentException();
                if(tempValue <= 1000)
                    result += tempValue;
            }
            return result;
        }
    }
}