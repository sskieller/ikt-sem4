using NUnit.Framework;
using Lektion1SWT;
using NUnit.Framework.Constraints;

namespace Calculator.Unit.Test
{
    [TestFixture]
    public class CalculatorTestUnit
    {
        // Setting: 1/2/3 parameters testing: Same function name
        // TestFunction name: Function_SpecificOverload_StateUnderTest_ExpectedBehavior
        // TestCase name: "(Optional)Specific overload, State Under Test, Expected Behavior
        // Ex: "Accumulator, Add 2 By 2 And Add Accu By 4, Result 8"
        // Ex: "Add 2 By -2, Result = 0"

        #region Add functiontest
        [TestCase(5, ExpectedResult = 5, TestName = "Accumulator, Add 0 By 5, Result = 5")]
        [TestCase(0, ExpectedResult = 0, TestName = "Accumulator, Add 0 By 0, Result = 0")]
        [TestCase(-5, ExpectedResult = -5, TestName = "Accumulator, Add 0 By -5, Result = -5")]
        public double Add_Test(double a)
        {
            var uut = new CalculatorUnit();
            return uut.Add(a);
        }

        [TestCase(2,-2,ExpectedResult = 0,TestName = "Add 2 By -2, Result = 0")]
        [TestCase(2,4,ExpectedResult = 6, TestName = "Add 2 By 4, Result = 6")]
        [TestCase(2,-4,ExpectedResult = -2, TestName = "Add 2 By -4, Result = -2")]
        [TestCase(0,0,ExpectedResult = 0,TestName = "Add 0 By 0, Result = 0")]
        public double Add_Test(double a, double b)
        {
            var uut = new CalculatorUnit();
            return uut.Add(a, b);
        }

        [TestCase(2, 2, 4, ExpectedResult = 8, TestName = "Accumulator, Add 2 By 2 And Add 4, Result = 8")]
        [TestCase(2, 2, 0, ExpectedResult = 4, TestName = "Accumulator, Add 2 By 2 And Add 0, Result = 4")]
        [TestCase(0, 0, 0, ExpectedResult = 0, TestName = "Accumulator, Add 0 By 0 And Add 0, Result = 0")]
        [TestCase(0, 0, -2, ExpectedResult = -2, TestName = "Accumulator, Add 0 By 0 And Add -2, Result = -2")]
        [TestCase(0, -2, -2, ExpectedResult = -4, TestName = "Accumulator, Add 0 By -2 And Add -2, Result = -4")]
        [TestCase(-2, -2, 4, ExpectedResult = 0, TestName = "Accumulator, Add -2 By -2 And Add 4, Result = 0")]
        public double Add_Test(double a, double b, double c)
        {
            var uut = new CalculatorUnit();
            uut.Add(a, b);
            return uut.Add(c);
        }
        #endregion

        #region Subtract functiontest
        [TestCase(5, ExpectedResult = -5, TestName = "Accumulator, Sub 0 By 5, Result = -5")]
        [TestCase(0, ExpectedResult = 0, TestName = "Accumulator, Sub 0 By 0, Result = 0")]
        [TestCase(-5, ExpectedResult = 5, TestName = "Accumulator, Sub 0 By -5, Result = 5")]
        public double Subtract_Test(double a)
        {
            var uut = new CalculatorUnit();
            return uut.Subtract(a);
        }

        [TestCase(5,3,ExpectedResult = 2, TestName = "Sub 5 By 3, Result = 2")]
        [TestCase(5,5,ExpectedResult = 0, TestName = "Sub 5 By 5, Result = 0")]
        [TestCase(5,-2,ExpectedResult = 7, TestName = "Sub 5 By -2, Result = 7")]
        [TestCase(0,5,ExpectedResult = -5,TestName = "Sub 0 By 5 , Result = -5")]
        public double Subtract_Test(double a, double b)
        {
            var uut = new CalculatorUnit();
            return uut.Subtract(a, b);
        }
        #endregion

        #region Power functiontest
        [TestCase(2, ExpectedResult = 0, TestName = "Accumulator, 0 To Power Of 2, Result = 0")]
        [TestCase(0, ExpectedResult = 1, TestName = "Accumulator, 0 To Power Of 0, Result = 0")]
        [TestCase(-2, ExpectedResult = double.PositiveInfinity, TestName = "Accumulator, 0 To Power Of -2, Result = 0")]
        public double Power_Test(double exponent)
        {
            var uut = new CalculatorUnit();
            return uut.Power(exponent);
        }

        [TestCase(2, 2, ExpectedResult = 4, TestName = "2 To Power Of 2, Result = 4")]
        [TestCase(2, 0, ExpectedResult = 1, TestName = "2 To Power Of 0, Result = 1")]
        [TestCase(2, -2, ExpectedResult = 0.25, TestName = "2 To Power Of -2, Result = 0.25")]
        [TestCase(0, 2, ExpectedResult = 0, TestName = "0 To The Power Of 2, Result = 0")]
        [TestCase(0, 0, ExpectedResult = 1, TestName = "0 To The Power Of 0, Result = 1")]
        [TestCase(0, -2, ExpectedResult = double.PositiveInfinity, TestName = "0 To Power Of -2, Result = PositiveInfinity")]
        public double Power_Test(double a, double b)
        {
            var uut = new CalculatorUnit();
            return uut.Power(a, b);
        }
        #endregion

        #region Divide functiontest
        // Divide
        [TestCase(2, 2, ExpectedResult = 1, TestName = "Divide 2 By 2, Result = 1")]
        [TestCase(2, 0, ExpectedResult = double.PositiveInfinity, TestName = "2 Divided By 0, Result = PositiveInfinity")]
        [TestCase(2, -2, ExpectedResult = -1, TestName = "Divide 2 By -2, Result = -1")]
        [TestCase(0, -2, ExpectedResult = 0, TestName = "Divide 0 By -2, Result = 0")]
        [TestCase(0, 2, ExpectedResult = 0, TestName = "Divide 0 By 2, Result = 0")]
        [TestCase(0, 0, ExpectedResult = double.NaN, TestName = "Divide 0 By 0, Result = NaN")]
        public double Divide_Test(double a, double b)
        {
            var uut = new CalculatorUnit();
            return uut.Divide(a, b);
        }
        #endregion

        #region Multiply functiontest
        // Multiply
        [TestCase(2, 2, ExpectedResult = 4, TestName = "Multiply 2 By 2, Result = 4")]
        [TestCase(2, -2, ExpectedResult = -4, TestName = "Multiply 2 By -2, Result = -4")]
        [TestCase(-2, -2, ExpectedResult = 4, TestName = "Multiply -2 By -2, Result = 4")]
        [TestCase(2, 0, ExpectedResult = 0, TestName = "Multiply 2 By 0, Result = 0")]
        public double Multiply_Test(double a, double b)
        {
            var uut = new CalculatorUnit();
            return uut.Multiply(a, b);
        }
        #endregion
    }
}
