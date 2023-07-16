using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Category : ScriptableObject
{
    private string _name;
    private List<Unistroke> _prototypes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
       public Category(string name)
        {
            _name = name;
            _prototypes = null;
        }

        public Category(string name, Unistroke firstExample)
        {
            _name = name;
            _prototypes = new List<Unistroke>();
            AddExample(firstExample);
        }

        public Category(string name, List<Unistroke> examples)
        {
            _name = name;
            _prototypes = new List<Unistroke>(examples.Count);
            for (int i = 0; i < examples.Count; i++)
            {
                AddExample(examples[i]);
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public int NumExamples
        {
            get
            {
                return _prototypes.Count;
            }
        }

        /// <summary>
        /// Indexer that returns the prototype at the given index within
        /// this gesture category, or null if the gesture does not exist.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Unistroke this[int i]
        {
            get
            {
                if (0 <= i && i < _prototypes.Count)
                {
                    return _prototypes[i];
                }
                else
                {
                    return null;
                }
            }
        }

        public void AddExample(Unistroke p)
        {
            bool success = true;
            try
            {
                // first, ensure that p's name is right
                string name = ParseName(p.Name);
                if (name != _name)
                    throw new ArgumentException("Prototype name does not equal the name of the category to which it was added.");

                // second, ensure that it doesn't already exist
                for (int i = 0; i < _prototypes.Count; i++)
                {
                    if (p.Name == _prototypes[i].Name)
                        throw new ArgumentException("Prototype name was added more than once to its category.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                success = false;
            }
            if (success)
            {
                _prototypes.Add(p);
            }
        }

        /// <summary>
        /// Pulls the category name from the gesture name, e.g., "circle" from "circle03".
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ParseName(string s)
        {
            string category = String.Empty;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (!Char.IsDigit(s[i]))
                {
                    category = s.Substring(0, i + 1);
                    break;
                }
            }
            return category;
        }
}
