using System;
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
        // TestCase name: "State Under Test, Expected Behavior
        // Ex: "Add 2 By 2 And Add Accu By 4, Result 8"
        // Ex: "Add 2 By -2, Result = 0"

        #region Add functiontest
        [TestCase(2, -2, ExpectedResult = 0, TestName = "Add 2 By -2, Result = 0")]
        [TestCase(2, 4, ExpectedResult = 6, TestName = "Add 2 By 4, Result = 6")]
        [TestCase(2, -4, ExpectedResult = -2, TestName = "Add 2 By -4, Result = -2")]
        [TestCase(0, 0, ExpectedResult = 0, TestName = "Add 0 By 0, Result = 0")]
        public double Add(double a, double b)
        {
            var uut = new CalculatorUnit();
            return uut.Add(a, b);
        }
        #endregion

        #region Add_Accumulator functiontest
        [TestCase(5, ExpectedResult = 5, TestName = "Add 0 By 5, Result = 5")]
        [TestCase(0, ExpectedResult = 0, TestName = "Add 0 By 0, Result = 0")]
        [TestCase(-5, ExpectedResult = -5, TestName = "Add 0 By -5, Result = -5")]
        public double Add_Accumulator_OneParameter(double a)
        {
            var uut = new CalculatorUnit();
            return uut.Add(a);
        }
        
        [TestCase(2, 2, 4, ExpectedResult = 8, TestName = "Add 2 By 2 And Add 4, Result = 8")]
        [TestCase(2, 2, 0, ExpectedResult = 4, TestName = "Add 2 By 2 And Add 0, Result = 4")]
        [TestCase(0, 0, 0, ExpectedResult = 0, TestName = "Add 0 By 0 And Add 0, Result = 0")]
        [TestCase(0, 0, -2, ExpectedResult = -2, TestName = "Add 0 By 0 And Add -2, Result = -2")]
        [TestCase(0, -2, -2, ExpectedResult = -4, TestName = "Add 0 By -2 And Add -2, Result = -4")]
        [TestCase(-2, -2, 4, ExpectedResult = 0, TestName = "Add -2 By -2 And Add 4, Result = 0")]
        public double Add_Accumulator(double a, double b, double c)
        {
            var uut = new CalculatorUnit();
            uut.Add(a, b);
            return uut.Add(c);
        }
        #endregion

        #region Subtract functiontest
        [TestCase(5, 3, ExpectedResult = 2, TestName = "Sub 5 By 3, Result = 2")]
        [TestCase(5, 5, ExpectedResult = 0, TestName = "Sub 5 By 5, Result = 0")]
        [TestCase(5, -2, ExpectedResult = 7, TestName = "Sub 5 By -2, Result = 7")]
        [TestCase(0, 5, ExpectedResult = -5, TestName = "Sub 0 By 5 , Result = -5")]
        public double Subtract(double a, double b)
        {
            var uut = new CalculatorUnit();
            return uut.Subtract(a, b);
        }
        #endregion

        #region Subtract_Accumulator functiontest
        [TestCase(5, ExpectedResult = -5, TestName = "Sub 0 By 5, Result = -5")]
        [TestCase(0, ExpectedResult = 0, TestName = "Sub 0 By 0, Result = 0")]
        [TestCase(-5, ExpectedResult = 5, TestName = "Sub 0 By -5, Result = 5")]
        public double Subtract_Accumulator_OneParameter(double a)
        {
            var uut = new CalculatorUnit();
            return uut.Subtract(a);
        }

        [TestCase(6, 3, 2, ExpectedResult = 1, TestName = "Sub 6 By 3 and Sub By 2, Result = 1")]
        [TestCase(6, 5, -2, ExpectedResult = 3, TestName = "Sub 6 By 5 And Sub By -2, Result = 3")]
        [TestCase(-2, 5, 5, ExpectedResult = -12, TestName = "Sub -2 By 5 And Sub By 5, Result = -12")]
        [TestCase(-2, 0, 5, ExpectedResult = -7, TestName = "Sub -2 By 0 And Sub By 5, Result = -7")]
        [TestCase(6, -10, 2, ExpectedResult = 14, TestName = "Sub 6 By 10 And Sub By 2, Result = 14")]
        [TestCase(0, 0, 0, ExpectedResult = 0, TestName = "Sub 0 By 0 And Sub By 0, Result = 0")]
        public double Subtract_Accumulator(double a, double b, double c)
        {
            var uut = new CalculatorUnit();
            uut.Subtract(a, b);
            return uut.Subtract(c);
        }
        #endregion

        #region Power functiontest
        [TestCase(2, 2, ExpectedResult = 4, TestName = "2 To Power Of 2, Result = 4")]
        [TestCase(2, 0, ExpectedResult = 1, TestName = "2 To Power Of 0, Result = 1")]
        [TestCase(2, -2, ExpectedResult = 0.25, TestName = "2 To Power Of -2, Result = 0.25")]
        [TestCase(0, 2, ExpectedResult = 0, TestName = "0 To The Power Of 2, Result = 0")]
        [TestCase(0, 0, ExpectedResult = 1, TestName = "0 To The Power Of 0, Result = 1")]
        [TestCase(0, -2, ExpectedResult = double.PositiveInfinity, TestName = "0 To Power Of -2, Result = PositiveInfinity")]
        public double Power(double a, double b)
        {
            var uut = new CalculatorUnit();
            return uut.Power(a, b);
        }
        #endregion

        #region Power_Accumulator functiontest
        [TestCase(2, ExpectedResult = 0, TestName = "0 To Power Of 2, Result = 0")]
        [TestCase(0, ExpectedResult = 1, TestName = "0 To Power Of 0, Result = 0")]
        [TestCase(-2, ExpectedResult = double.PositiveInfinity, TestName = "0 To Power Of -2, Result = 0")]
        public double Power_Accumulator_OneParameter(double exponent)
        {
            var uut = new CalculatorUnit();
            return uut.Power(exponent);
        }

        [TestCase(6, 3, 2, 46656, 0.5, TestName = "6 To The Power Of 3 and To The Power Of 2, Result = 46656, Tolerance:0.5%")]
        [TestCase(6, 5, -2, 1.65381717e-8, 0.5, TestName = "6 To The Power Of 5 And To The Power Of -2, Result = 1.65381717e-8, Tolerance:0.5%")]
        [TestCase(-2, 5, 5, -33554432, 0.5, TestName = "-2 To The Power Of 5 And To The Power Of 5, Result = -33554432, Tolerance:0.5%")]
        [TestCase(-2, 0, 5, 1, 0.0005, TestName = "-2 To The Power Of 0 And To The Power Of 5, Result = -1,Tolerance:0.0005%")]
        [TestCase(6, -3, 2, 0.00002143347, 0.00005, TestName = "6 To The Power Of 10 And To The Power Of 2, Result = 0.00002143347,Tolerance:0.00005%")]
        [TestCase(0, 0, 0, 1, 0, TestName = "0 To The Power Of 0 And To The Power Of 0, Result = 1,Tolerance:0%")]
        public void Power_Accumulator(double a, double b, double c, double result, double tolerance)
        {
            var uut = new CalculatorUnit();
            uut.Power(a, b);
            Assert.That(uut.Power(c), Is.EqualTo(result).Within(tolerance));
        }
        #endregion

        #region Divide functiontest
        [TestCase(2, 2, ExpectedResult = 1, TestName = "Divide 2 By 2, Result = 1")]
        [TestCase(2, -2, ExpectedResult = -1, TestName = "Divide 2 By -2, Result = -1")]
        [TestCase(0, -2, ExpectedResult = 0, TestName = "Divide 0 By -2, Result = 0")]
        [TestCase(0, 2, ExpectedResult = 0, TestName = "Divide 0 By 2, Result = 0")]
        public double Divide(double a, double b)
        {
            var uut = new CalculatorUnit();
            return uut.Divide(a, b);
        }

        [TestCase(2, 0, ExpectedResult = 0, TestName = "Divide 2 By 0, Result = 0")]
        [TestCase(0, 0, ExpectedResult = 0, TestName = "Divide 0 By 0, Result = 0")]
        public double Divide_ExceptionThrow(double a, double b)
        {
            var uut = new CalculatorUnit();
            Assert.That(() => uut.Divide(a, b), Throws.TypeOf<ArgumentException>());
            return 0;
        }
        #endregion

        #region Divide_Accumulator functiontest
        [TestCase(2, ExpectedResult = 0, TestName = "0 Divided By 2, Result = 0")]
        [TestCase(-2, ExpectedResult = 0, TestName = "0 Divided By -2, Result = 0")]
        public double Divide_Accumulator_OneParameter(double exponent)
        {
            var uut = new CalculatorUnit();
            return uut.Divide(exponent);
        }

        [TestCase(0, TestName = "0 Divided By 0, Result = Throw ArgumentException")]
        public void Divide_Accumulator_OneParameter_ExceptionTesting(double divisor)
        {
            var uut = new CalculatorUnit();
            Assert.That(() => uut.Divide(divisor), Throws.TypeOf<ArgumentException>());
        }

        [TestCase(6, 3, 2, 1, 0, TestName = "6 Divided By 3 and Divided By 2, Result = 1, Tolerance:0.5%")]
        [TestCase(6, 5, -2, -0.6, 0.05, TestName = "6 Divided By 5 And Divided By -2, Result = -0.6, Tolerance:0.05%")]
        [TestCase(-2, 5, 5, -0.08, 0.005, TestName = "-2 Divided By 5 And Divided By 5, Result = -0.08, Tolerance:0.005%")]
        [TestCase(6, -3, 2, -1, 0, TestName = "6 Divided By 10 And Divided By 2, Result = -1, Tolerance:0%")]
        public void Divide_Accumulator(double a, double b, double c, double result, double tolerance)
        {
            var uut = new CalculatorUnit();
            uut.Divide(a, b);
            Assert.That(uut.Divide(c), Is.EqualTo(result).Within(tolerance));
        }

        [TestCase(3, 3, 0, TestName = "3 Divided By 3 And Divided By 0, Result = Throw ArgumentException")]
        public void Divide_Accumulator_ExceptionTesting(double a, double b, double c)
        {
            var uut = new CalculatorUnit();
            uut.Divide(a, b);
            Assert.That(() => uut.Divide(c), Throws.TypeOf<ArgumentException>());
        }

        #endregion

        #region Multiply functiontest
        [TestCase(2, 2, ExpectedResult = 4, TestName = "Multiply 2 By 2, Result = 4")]
        [TestCase(2, -2, ExpectedResult = -4, TestName = "Multiply 2 By -2, Result = -4")]
        [TestCase(-2, -2, ExpectedResult = 4, TestName = "Multiply -2 By -2, Result = 4")]
        [TestCase(2, 0, ExpectedResult = 0, TestName = "Multiply 2 By 0, Result = 0")]
        public double Multiply(double a, double b)
        {
            var uut = new CalculatorUnit();
            return uut.Multiply(a, b);
        }
        #endregion

        #region Multiply_Accumulator functiontest
        [TestCase(2, ExpectedResult = 0, TestName = "0 Multiplied By 2, Result = 0")]
        [TestCase(0, ExpectedResult = 0, TestName = "0 Multiplied By 0, Result = 0")]
        [TestCase(-2, ExpectedResult = 0, TestName = "0 Multiplied By -2, Result = 0")]
        public double Multiply_Accumulator_OneParameter(double exponent)
        {
            var uut = new CalculatorUnit();
            return uut.Multiply(exponent);
        }

        [TestCase(6, 3, 2, 36, 0, TestName = "6 Multiplied By 3 and Multiplied By 2, Result = 36, Tolerance:0%")]
        [TestCase(6, 5, -2, -60, 0, TestName = "6 Multiplied By 5 And Multiplied By -2, Result = -60, Tolerance:0%")]
        [TestCase(-2, 5, 5, -50, 0, TestName = "-2 Multiplied By 5 And Multiplied By 5, Result = -50, Tolerance:0%")]
        [TestCase(-2, 0, 5, 0, 0, TestName = "-2 Multiplied By 0 And Multiplied By 5, Result = 0, Tolerance:0%")]
        [TestCase(6, -3, 2, -36, 0, TestName = "6 Multiplied By 10 And Multiplied By 2, Result = -36, Tolerance:0%")]
        [TestCase(0, 0, 0, 0, 0, TestName = "0 Multiplied By 0 And Multiplied By 0, Result = 0, Tolerance:0%")]
        public void Multiply_Accumulator(double a, double b, double c, double result, double tolerance)
        {
            var uut = new CalculatorUnit();
            uut.Multiply(a, b);
            Assert.That(uut.Multiply(c), Is.EqualTo(result).Within(tolerance));
        }
        #endregion

        #region Clear_Accumulator_test

        [TestCase(0, 0, TestName = "No Function run, Expected 0 in Accumulator")]
        public void Clear_Accumulator(double a, double accumulatorValue)
        {
            var uut = new CalculatorUnit();
            uut.Clear();
            Assert.That(uut.Accumulator, Is.EqualTo(accumulatorValue));
        }

        [TestCase(5, 0, TestName = "Added_Function run once, Expected 0 in Accumulator")]
        public void Clear_Accumulator_Function(double a, double accumulatorValue)
        {
            var uut = new CalculatorUnit();
            uut.Add(a, a);
            uut.Clear();
            Assert.That(uut.Accumulator, Is.EqualTo(accumulatorValue));
        }

        [TestCase(5, 12, 0, TestName = "Added_Function run twice, Expected 0 in Accumulator")]
        public void Clear_Accumulator_Function2x(double a, double b, double accumulatorValue)
        {
            var uut = new CalculatorUnit();
            uut.Add(b, a);
            uut.Add(a, b);
            uut.Clear();
            Assert.That(uut.Accumulator, Is.EqualTo(accumulatorValue));
        }

        #endregion
    }
}
