using System;

namespace blood_alcohol_approximator
{
    public class Person
    {
        public int Weight { get; }
        public String Gender { get; }

        /// <summary>
        /// Constructor to create a person containing the following information
        /// </summary>
        /// <param name="weight">Weight of person</param>
        /// <param name="gender">Gender of person</param>
        public Person(int weight, string gender)
        {
            Weight = weight;
            Gender = gender;
        }
    }
}

