using NUnit.Framework;
using Lektion1SWT;
using NUnit.Framework.Constraints;

namespace Calculator.Unit.Test
{
    [TestFixture]
    public class CalculatorTestUnit
    {
        [TestCase(2,-2,ExpectedResult = 0,TestName = "Add2AndMinus2_Return0")]
        [TestCase(2,4,ExpectedResult = 6, TestName = "Add2And4_Return6")]
        [TestCase(2,-4,ExpectedResult = -2, TestName = "Add2AndMinus4_ReturnMinus2")]
        [TestCase(0,0,ExpectedResult = 0,TestName = "Add0And0_Return0")]
        public double Add_Test(double a, double b)
        {
            var uut = new CalculatorUnit();
            return uut.Add(a, b);
        }

        [TestCase(5,3,ExpectedResult = 2, TestName = "Subtract3From5_Return2")]
        [TestCase(5,5,ExpectedResult = 0, TestName = "Subtract5from5_Return0")]
        [TestCase(5,-2,ExpectedResult = 7, TestName = "SubtractMinus2From5_Return7")]
        [TestCase(0,5,ExpectedResult = -5,TestName = "Subtract5From0_ReturnMinus5")]
        public double Subtract_Test(double a, double b)
        {
            var uut = new CalculatorUnit();
            return uut.Subtract(a, b);
        }

        [TestCase(2, 2, ExpectedResult = 4, TestName = "Power2ToThePowerOf2_Return4")]
        [TestCase(2, 0, ExpectedResult = 1, TestName = "Power2ToThePowerOf0_Return1")]
        [TestCase(2, -2, ExpectedResult = 0.25, TestName = "Power2ToThePowerOfMinus2_Return0.25")]
        [TestCase(0, 2, ExpectedResult = 0, TestName = "Power0ToThePowerOf2_Return0")]
        [TestCase(0, 0, ExpectedResult = 1, TestName = "Power0ToThePowerOf0_Return1")]
        [TestCase(0, -2, ExpectedResult = double.PositiveInfinity, TestName = "Power0ToThePowerOfMinus2_ReturnPositiveInfinity")]
        public double Power_Test(double a, double b)
        {
            var uut = new CalculatorUnit();
            return uut.Power(a, b);
        }

        [TestCase(2, 2, ExpectedResult = 1, TestName = "Divide2By2_Return1")]
        [TestCase(2, 0, ExpectedResult = double.PositiveInfinity, TestName = "Divide2By0_ReturnPositiveInfinity")]
        [TestCase(2, -2, ExpectedResult = -1, TestName = "Divide2ByMinus2_ReturnMinus1")]
        [TestCase(0, -2, ExpectedResult = 0, TestName = "Divide0ByMinus2_Return0")]
        [TestCase(0, 2, ExpectedResult = 0, TestName = "Divide0By2_Return0")]
        [TestCase(0, 0, ExpectedResult = double.NaN, TestName = "Divide0By0_ReturnNaN")]
        public double Divide_Test(double a, double b)
        {
            var uut = new CalculatorUnit();
            return uut.Divide(a, b);
        }

        [TestCase(2,2, ExpectedResult = 4, TestName = "Multiply 2*2 Result = 4")]
        [TestCase(2, -2, ExpectedResult = -4, TestName = "Multiply 2*-2 Result = -4")]
        [TestCase(-2, -2, ExpectedResult = 4, TestName = "Multiply -2*-2 Result = 4")]
        [TestCase(2, 0, ExpectedResult = 0, TestName = "Multiply 2*0 Result = 0")]
        public double Multiply_Test(double a, double b)
        {
            var uut = new CalculatorUnit();
            return uut.Multiply(a, b);
        }

        [TestCase(2, ExpectedResult = 0, TestName = "Power0By2_Return0")]
        [TestCase(0, ExpectedResult = 0, TestName = "Power0By0_Return0")]
        [TestCase(-2, ExpectedResult = 0, TestName = "Power0By-2_Return0")]
        public double Power_Test(double exponent)
        {
            var uut = new CalculatorUnit();
            return uut.Power(2);
        }
    }
}
