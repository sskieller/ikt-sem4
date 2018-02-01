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
	        Accumulator = a + b;
            return Accumulator;

        }

        public double Subtract(double a, double b)
        {
	        Accumulator = a - b;
            return Accumulator;
        }

        public double Power(double a, double b)
        {
	        Accumulator = Math.Pow(a, b);
            return Accumulator;
        }

        public double Multiply(double x, double y)
        {
	        Accumulator = x * y;
            return Accumulator;
        }

        public double Divide(double a, double b)
        {
	        Accumulator = a / b;
            return Accumulator;
        }

	    public double Accumulator { get; set; } = 0;

        public double Add(double addend)
        {
            return Accumulator + addend;
        }

        public double Subtract(double subtractor)
        {
            return Accumulator - subtractor;
        }

        public double Multiply(double multiplier)
        {
            return Accumulator * multiplier;
        }

        public double Divide(double divisor)
        {
            return Accumulator / divisor;
        }

        public double Power(double exponent)
        {
            return Math.Pow(Accumulator, exponent);
        }

    }
}
