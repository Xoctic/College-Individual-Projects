using SS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Formulas;

namespace SpreadsheetTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod()]
        public void EmptyTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellContents("pp5"));
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void NullTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents(null, 50);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void NullTest2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents(null, "HI");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void NullTest3()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents(null, new Formula("50 + 20"));
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void NullTest4()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("c50", 10);
            s.GetCellContents(null);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidNameTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("ABC0500", new Formula("50 + 20"));
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidNameTest2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("HELLO", 25);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidNameTest3()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("A1A", "monkey");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidNameTest4()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("c50", 10);
            s.GetCellContents("c05");
        }

        [TestMethod()]
        public void SetCellContentsTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("B7", 150);
            Assert.AreEqual(150, (double)s.GetCellContents("B7"), 1e-9);
        }

        [TestMethod()]
        public void SetCellContentsTest2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("abcdefGHIJK500", "GOOD");
            Assert.AreEqual("GOOD", s.GetCellContents("abcdefGHIJK500"));
        }

        [TestMethod()]
        public void SetCellContentsTest3()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("A5", new Formula("9 + 5"));
            Formula f = (Formula)s.GetCellContents("A5");
            Assert.AreEqual(14, f.Evaluate(x => 0), 1e-6);
        }

        [TestMethod()]
        public void SetCellContentsTest4()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("B1", new Formula("3"));
            s.SetCellContents("B2", new Formula("B11"));
            s.SetCellContents("B3", new Formula("3"));
            s.SetCellContents("B4", new Formula("B10"));
            s.SetCellContents("B4", new Formula("3"));
        }

        [TestMethod()]
        public void SetCellContentsTest5()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", 50);
            s.SetCellContents("B1", 100);
            s.SetCellContents("C1", "");
            HashSet<string> expected = new HashSet<string>() { "A1", "B1" };
            expected.SetEquals(s.GetNamesOfAllNonemptyCells());
        }

        [TestMethod()]
        public void SetCellContentsTest6()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", 50);
            s.SetCellContents("B1", new Formula("A1"));
            s.SetCellContents("C1", new Formula("B1"));
            HashSet<string> expected = new HashSet<string>() { "D1", "A1", "B1", "C1" };
            HashSet<string> result = (HashSet<string>)s.SetCellContents("D1", new Formula("C1"));
            expected.SetEquals(result);
        }

        [TestMethod()]
        public void SetCellContentsTest7()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", 50);
            s.SetCellContents("B1", new Formula("A1"));
            s.SetCellContents("C1", new Formula("B1"));
            HashSet<string> expected = new HashSet<string>() { "D1", "A1", "B1", "C1" };
            HashSet<string> result = (HashSet<string>)s.SetCellContents("D1", "C1");
            expected.SetEquals(result);
        }

        [TestMethod()]
        public void SetCellContentsTest8()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", 50);
            s.SetCellContents("B1", new Formula("A1"));
            s.SetCellContents("C1", new Formula("B1"));
            HashSet<string> expected = new HashSet<string>() { "D1", "A1", "B1", "C1" };
            expected.SetEquals(s.SetCellContents("D1", 10));
        }

        [TestMethod()]
        public void ChangeCellContentsTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("H5", 150);
            s.SetCellContents("H5", 20);
            Assert.AreEqual(20, (double)s.GetCellContents("H5"), 1e-9);
        }

        [TestMethod()]
        public void ChangeCellContentsTest2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("H5", "ORIGINAL");
            s.SetCellContents("H5", "CHANGING");
            Assert.AreEqual("CHANGING", s.GetCellContents("H5"));
        }

        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void CircularExceptionTest1()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("C1", new Formula("C2"));
            s.SetCellContents("C2", new Formula("C1"));
        }

        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void CircularExceptionTest2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("A3"));
            s.SetCellContents("A2", new Formula("A3"));
            s.SetCellContents("A3", new Formula("A1"));
        }
    }
}
