﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using System.Collections.Generic;
using System;

namespace PS4aDevelopmentTests
{
    [TestClass]
    public class DevelopmentTests
    {
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ThreeArg4()
        {
            Formula f = new Formula("x+y", s => s == "x" ? "z" : s, s => s != "z");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThreeArgWithNull1()
        {
            Formula f = new Formula(null, s => s == "x" ? "z" : s, s => s != "z");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThreeArgWithNull2()
        {
            Formula f = new Formula("x+y", null, s => s != "z");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThreeArgWithNull3()
        {
            Formula f = new Formula("x+y", s => s == "x" ? "z" : s, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThreeArgWithNull4()
        {
            Formula f = new Formula("y", s => "x", s => true);
            f.Evaluate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ThreeArgWithInvalidNormalize()
        {
            Formula f = new Formula("y", s => "800", s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ThreeArgWithInvalidValidate()
        {
            Formula f = new Formula("y", s => "x", s => false);
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void DivideByZero()
        {
            Formula f = new Formula("8 / xx");
            f.Evaluate(s => 0);
        }

        [TestMethod]
        public void ThreeArg7()
        {
            Formula f = new Formula("y", s => "x", s => true);
            Assert.AreEqual(1.0, f.Evaluate(s => (s == "x") ? 1 : 0), 1e-6);
        }

        [TestMethod]
        public void GetVars3()
        {
            Formula f = new Formula("a * b - c + d / e * 2.5e6");
            var expected = new HashSet<string>();
            expected.Add("a");
            expected.Add("b");
            expected.Add("c");
            expected.Add("d");
            expected.Add("e");
            var actual = f.GetVariables();
            Assert.IsTrue(expected.SetEquals(actual));
        }

        [TestMethod]
        public void ToString4()
        {
            Formula f1 = new Formula("a+b*(c-15)/2");
            Formula f2 = new Formula(f1.ToString());
            Assert.AreEqual(24.0, f2.Evaluate(s => char.IsLower(s[0]) ? 16 : 0), 1e-6);
        }
    }
}
