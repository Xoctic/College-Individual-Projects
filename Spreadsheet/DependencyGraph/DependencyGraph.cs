// Skeleton implementation written by Joe Zachary for CS 3500, January 2018.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {
        Dictionary<string, HashSet<string>> dependees;
        Dictionary<string, HashSet<string>> dependents;

        int size = 0;

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return size; }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependents(string s)
        {
            bool hasDependents = false;
            if (s != null)
            {
                if (dependees.ContainsKey(s))
                {
                    hasDependents = true;
                }
            }
            return hasDependents;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            bool hasDependees = false;
            if (s != null)
            {
                if (dependents.ContainsKey(s))
                {
                    hasDependees = true;
                }
            }
            return hasDependees;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            HashSet<string> dependentsToReturn = new HashSet<string>();
            if (s != null)
            {
                if (dependees.ContainsKey(s))
                {
                    HashSet<string> dependentsSet = dependees[s];
                    foreach (string n in dependentsSet)
                    {
                        dependentsToReturn.Add(n);
                    }
                }
            }
            return dependentsToReturn;
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            HashSet<string> dependeesToReturn = new HashSet<string>();
            if (s != null)
            {
                if (dependents.ContainsKey(s))
                {
                    HashSet<string> dependeesSet = dependents[s];
                    foreach (string n in dependeesSet)
                    {
                        dependeesToReturn.Add(n);
                    }
                }
            }
            return dependeesToReturn;
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if (s != null && t != null)
            {
                if (!(dependees.ContainsKey(s)) && !(dependents.ContainsKey(t)))
                {
                    HashSet<string> dependentsSet = new HashSet<string>();
                    HashSet<string> dependeesSet = new HashSet<string>();
                    dependees.Add(s, dependentsSet);
                    dependents.Add(t, dependeesSet);
                    dependentsSet.Add(t);
                    dependeesSet.Add(s);
                    size++;
                }
                else if (dependees.ContainsKey(s) && !(dependents.ContainsKey(t)))
                {

                    HashSet<string> dependeesSet = new HashSet<string>();
                    dependents.Add(t, dependeesSet);
                    dependees[s].Add(t);
                    dependeesSet.Add(s);
                    size++;
                }
                else if (!(dependees.ContainsKey(s)) && dependents.ContainsKey(t))
                {


                    HashSet<string> dependentsSet = new HashSet<string>();
                    dependees.Add(s, dependentsSet);
                    dependents[t].Add(s);
                    dependentsSet.Add(t);
                    size++;
                }
            }
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if (s != null && t != null)
            {
                if (dependees.ContainsKey(s) && dependents.ContainsKey(t))
                {
                    dependees[s].Remove(t);
                    dependees.Remove(s);
                    dependents[t].Remove(s);
                    dependents.Remove(t);
                    if (size > 0)
                    {
                        size--;
                    }
                }
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (s != null && newDependents != null)
            {
                if (dependees.ContainsKey(s))
                {
                    for(int i = 0; i < dependees[s].Count; i++)
                    {
                        size--;
                    }
                    dependees[s].Clear();                   
                    foreach (string element in newDependents)
                    {
                        AddDependency(s, element);
                    }
                }
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if (t != null && newDependees != null)
            {
                if (dependents.ContainsKey(t))
                {
                    for (int i = 0; i < dependents[t].Count; i++)
                    {
                        size--;
                    }
                    dependents[t].Clear();
                    foreach (string element in newDependees)
                    {
                        AddDependency(element, t);
                    }
                }
            }
        }
    }
}
