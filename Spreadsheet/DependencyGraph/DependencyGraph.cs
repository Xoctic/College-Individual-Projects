// Skeleton implementation written by Joe Zachary for CS 3500, January 2018.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//Author:  Andrew Hare  u1033940

namespace Dependencies
{
    /// <summary>
   
    public class DependencyGraph
    {
        //Dictionaries used to store Dependencies ; duplicates cannot be stored
        //Each dictionary contains a string key and a HashSet value
        //Each Hashset value is a subset containing either dependess or dependents.
        Dictionary<string, HashSet<string>> dependees;
        Dictionary<string, HashSet<string>> dependents;

        //used to keep track of the size of the DependencyGraph
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
                else if(dependees.ContainsKey(s) && dependents.ContainsKey(t))
                {
                    if(!(dependees[s].Contains(t)))
                    {
                        dependees[s].Add(t);
                    }
                    if (!(dependents[t].Contains(s)))
                    {
                        dependents[t].Add(s);
                    }
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
                if(dependees.ContainsKey(s))
                {
                    if(dependees[s].Contains(t))
                    {
                        dependees[s].Remove(t);
                        size--;
                        if(dependees[s].Count == 0)
                        {
                            dependees.Remove(s);
                        }
                    }
                }
                if(dependents.ContainsKey(t))
                {
                    if(dependents[t].Contains(s))
                    {
                        dependents[t].Remove(s);
                        if(dependents[t].Count == 0)
                        {
                            dependents.Remove(t);
                        }
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
                   foreach(string dependent in dependees[s])
                   {
                        dependents[dependent].Remove(s);
                   }
                    dependees[s].Clear();                
                }
                foreach (string element in newDependents)
                {
                    AddDependency(s, element);
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
                    foreach(string dependee in dependents[t])
                    {
                        dependees[dependee].Remove(t);
                        size--;
                    }
                    dependents[t].Clear();
                }
                foreach (string element in newDependees)
                {
                    AddDependency(element, t);
                }
            }
        }
    }
}
