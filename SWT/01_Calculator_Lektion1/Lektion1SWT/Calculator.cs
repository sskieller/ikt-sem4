using System;

namespace Lektion1SWT
{
    public static class Program
    {
        public static void Main()
        {
            // Do nothing
        }
    }
    public class CalculatorUnit
    {
        public double Add(double a, double b)
        {
            return a + b;
        }

        public double Subtract(double a, double b)
        {
            return a - b;
        }

        public double Power(double a, double b)
        {
            return Math.Pow(a, b);
        }

        public double Divide(double a, double b)
        {
            return a / b;
        }
    }
}
