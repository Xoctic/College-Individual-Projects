using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;
using System.Collections.Generic;
using System;

namespace PS4DevelopmentTests
{
    [TestClass]
    public class DevelopmentTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null1()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull1()
        {
            DependencyGraph d = null;
            DependencyGraph d2 = new DependencyGraph(d);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull2()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "c");
            d.AddDependency("a", "z");
            d.HasDependents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull3()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "c");
            d.AddDependency("a", "z");
            d.HasDependees(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull4()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "c");
            d.AddDependency("a", "z");
            d.GetDependents(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull5()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "c");
            d.AddDependency("a", "z");
            d.GetDependees(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull6()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "c");
            d.AddDependency("a", "z");
            d.RemoveDependency(null, "c");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull7()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "c");
            d.AddDependency("a", "z");
            d.RemoveDependency("a", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull8()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "c");
            d.AddDependency("a", "z");
            d.ReplaceDependees("c", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull9()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "c");
            d.AddDependency("a", "z");
            d.ReplaceDependents("a", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull10()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "c");
            d.AddDependency("a", "z");
            d.ReplaceDependents("a", new HashSet<string>() { "a", "j", null, "m" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNull11()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("a", "c");
            d.AddDependency("a", "z");
            d.ReplaceDependees("c", new HashSet<string>() { "d", null, "y", "m" });
        }

        [TestMethod]
        public void Copy1()
        {
            var d1 = new DependencyGraph();
            var d2 = new DependencyGraph(d1);
            Assert.AreEqual(0, d1.Size);
            Assert.AreEqual(0, d2.Size);
        }

        [TestMethod]
        public void Copy5()
        {
            var d1 = new DependencyGraph();
            d1.AddDependency("a", "b");
            d1.AddDependency("d", "e");
            var d2 = new DependencyGraph(d1);
            d1.AddDependency("a", "c");
            d2.AddDependency("d", "f");
            Assert.AreEqual(2, new List<string>(d1.GetDependents("a")).Count);
            Assert.AreEqual(1, new List<string>(d1.GetDependents("d")).Count);
            Assert.AreEqual(2, new List<string>(d2.GetDependents("d")).Count);
            Assert.AreEqual(1, new List<string>(d2.GetDependents("a")).Count);
        }

        [TestMethod]
        public void Copy100()
        {
            var d1 = new DependencyGraph();
            d1.AddDependency("l", "m");
            d1.AddDependency("l", "n");
            d1.AddDependency("j", "k");
            var d2 = new DependencyGraph(d1);
            d1.AddDependency("l", "p");
            d2.AddDependency("a", "b");
            d2.AddDependency("a", "c");
            Assert.AreEqual(3, new List<string>(d1.GetDependents("l")).Count);
            Assert.AreEqual(1, new List<string>(d1.GetDependents("j")).Count);
            Assert.AreEqual(2, new List<string>(d2.GetDependents("l")).Count);
            Assert.AreEqual(2, new List<string>(d2.GetDependents("a")).Count);
            Assert.AreEqual(4, d1.Size);
            Assert.AreEqual(5, d2.Size);  
        }
    }
}
