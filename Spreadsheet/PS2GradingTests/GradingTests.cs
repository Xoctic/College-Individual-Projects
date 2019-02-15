using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Formulas
{
    [TestClass]
    public class GradingTests
    {
        // Tests of syntax errors detected by the constructor
        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test1()
        {
            Formula f = new Formula("        ");
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test2()
        {
            Formula f = new Formula("((2 + 5))) + 8");
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test3()
        {
            Formula f = new Formula("2+5*8)");
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test4()
        {
            Formula f = new Formula("((3+5*7)");
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test5()
        {
            Formula f = new Formula("+3");
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test6()
        {
            Formula f = new Formula("-y");
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test7()
        {
            Formula f = new Formula("*7");
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test8()
        {
            Formula f = new Formula("/z2x");
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test9()
        {
            Formula f = new Formula(")");
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test10()
        {
            Formula f = new Formula("(*5)");
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test11()
        {
            Formula f = new Formula("2 5");
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test12()
        {
            Formula f = new Formula("x5 y");
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test13()
        {
            Formula f = new Formula("((((((((((2)))))))))");
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test14()
        {
            Formula f = new Formula("$");
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test15()
        {
            Formula f = new Formula("x5 + x6 + x7 + (x8) +");
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test15a()
        {
            Formula f = new Formula("x1 ++ y1");
        }

        // Simple tests that throw FormulaEvaluationExceptions
        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test16()
        {
            Formula f = new Formula("2+x");
            f.Evaluate(s => { throw new UndefinedVariableException(s); });
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test17()
        {
            Formula f = new Formula("5/0");
            f.Evaluate(s => 0);
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test18()
        {
            Formula f = new Formula("(5 + x) / (y - 3)");
            f.Evaluate(s => 3);
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test18a()
        {
            Formula f = new Formula("(5 + x) / (3 * 2 - 12 / 2)");
            f.Evaluate(s => 3);
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test19()
        {
            Formula f = new Formula("x + y");
            f.Evaluate(s => { if (s == "x") return 0; else throw new UndefinedVariableException(s); });
        }

        [TestMethod, Timeout(1000)]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Test20()
        {
            Formula f = new Formula("x1 + x2 * x3 + x4 * x5 * x6 + x7");
            f.Evaluate(s => { if (s == "x7") throw new UndefinedVariableException(s); else return 1; });
        }

        // Simple formulas
        [TestMethod, Timeout(1000)]
        public void Test21()
        {
            Formula f = new Formula("4.5e1");
            Assert.AreEqual(45, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod, Timeout(1000)]
        public void Test21a()
        {
            Formula f = new Formula("4");
            Assert.AreEqual(4, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod, Timeout(1000)]
        public void Test22()
        {
            Formula f = new Formula("a05");
            Assert.AreEqual(10, f.Evaluate(s => 10), 1e-6);
        }

        [TestMethod, Timeout(1000)]
        public void Test22a()
        {
            Formula f = new Formula("a1b2c3d4e5f6g7h8i9j10");
            Assert.AreEqual(10, f.Evaluate(s => 10), 1e-6);
        }

        [TestMethod, Timeout(1000)]
        public void Test23()
        {
            Formula f = new Formula("5 + x");
            Assert.AreEqual(9, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod, Timeout(1000)]
        public void Test24()
        {
            Formula f = new Formula("5 - y");
            Assert.AreEqual(1, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod, Timeout(1000)]
        public void Test25()
        {
            Formula f = new Formula("5 * z");
            Assert.AreEqual(20, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod, Timeout(1000)]
        public void Test26()
        {
            Formula f = new Formula("8 / xx");
            Assert.AreEqual(2, f.Evaluate(s => 4), 1e-6);
        }

        [TestMethod, Timeout(1000)]
        public void Test27()
        {
            Formula f = new Formula("(5 + 4) * 2");
            Assert.AreEqual(18, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod, Timeout(1000)]
        public void Test28()
        {
            Formula f = new Formula("1 + 2 + 3 * 4 + 5");
            Assert.AreEqual(20, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod, Timeout(1000)]
        public void Test29()
        {
            Formula f = new Formula("(1 + 2 + 3 * 4 + 5) * 2");
            Assert.AreEqual(40, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod, Timeout(1000)]
        public void Test30()
        {
            Formula f = new Formula("((((((((((((3))))))))))))");
            Assert.AreEqual(3, f.Evaluate(s => 0), 1e-6);
        }

        [TestMethod, Timeout(1000)]
        public void Test31()
        {
            Formula f = new Formula("((((((((((((x))))))))))))");
            Assert.AreEqual(7, f.Evaluate(s => 7), 1e-6);
        }

        // Some more complicated formula evaluations
        [TestMethod, Timeout(1000)]
        public void Test32()
        {
            Formula f = new Formula("y*3-8/2+4*(8-9*2)/14*x");
            Assert.AreEqual(5.14285714285714, (double)f.Evaluate(s => (s == "x") ? 1 : 4), 1e-9);
        }

        [TestMethod, Timeout(1000)]
        public void Test32a()
        {
            Formula f = new Formula("a + b * c - d + 3 * 3.0 - 3.0e0 / 0.003e3");
            Assert.AreEqual(17, (double)f.Evaluate(s => 3), 1e-9);
        }

        [TestMethod, Timeout(1000)]
        public void Test33()
        {
            Formula f = new Formula("a+(b+(c+(d+(e+f))))");
            Assert.AreEqual(6, (double)f.Evaluate(s => 1), 1e-9);
        }

        [TestMethod, Timeout(1000)]
        public void Test34()
        {
            Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            Assert.AreEqual(12, (double)f.Evaluate(s => 2), 1e-9);
        }

        [TestMethod, Timeout(1000)]
        public void Test35()
        {
            Formula f = new Formula("a-a*a/a");
            Assert.AreEqual(0, (double)f.Evaluate(s => 3), 1e-9);
        }

        // Tests to make sure there can be more than one formula at a time
        [TestMethod, Timeout(1000)]
        public void Test36()
        {
            Formula f1 = new Formula("xx+3");
            Formula f2 = new Formula("xx-3");
            Assert.AreEqual(6, f1.Evaluate(s => 3), 1e-6);
            Assert.AreEqual(0, f2.Evaluate(s => 3), 1e-6);
        }

        [TestMethod, Timeout(1000)]
        public void Test37()
        {
            Test36();
        }

        [TestMethod, Timeout(1000)]
        public void Test38()
        {
            Test36();
        }

        [TestMethod, Timeout(1000)]
        public void Test39()
        {
            Test36();
        }

        [TestMethod, Timeout(1000)]
        public void Test40()
        {
            Test36();
        }

        // Stress test for constructor
        [TestMethod, Timeout(1000)]
        public void Test41()
        {
            Formula f = new Formula("(((((2+3*a)/(7e-5+b-c))*d+.0005e+92)-8.2)*3.14159) * ((e+3.1)-.00000000008)");
        }

        // Stress test for constructor, repeated five times to give it extra weight.
        [TestMethod, Timeout(1000)]
        public void Test42()
        {
            Test41();
        }

        // Stress test for constructor
        [TestMethod, Timeout(1000)]
        public void Test43()
        {
            Test41();
        }

        // Stress test for constructor
        [TestMethod, Timeout(1000)]
        public void Test44()
        {
            Test41();
        }

        // Stress test for constructor
        [TestMethod, Timeout(1000)]
        public void Test45()
        {
            Test41();
        }
    }
}
