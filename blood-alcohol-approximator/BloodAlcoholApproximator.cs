using System;
using System.Collections.Generic;

namespace blood_alcohol_approximator
{
    public class BloodAlcoholApproximator
    {
        private readonly static double _BallmerPeak = 0.129;

        static BloodAlcoholApproximator() { }

        /// <summary>
        /// Static method to calculate how much gramms of alcohol are needed to
        /// reach the Ballmer Peak
        /// </summary>
        /// <param name="person">Object of a person containing weight and gender</param>
        /// <returns></returns>
        public static double Approximate(Person person)
        {
            double r;

            if (person.Gender.Equals("women")) { r = 0.55; }
            else { r = 0.68; }
            double grammsAlcohol = (_BallmerPeak * person.Weight* 1000 * r) / 100;
            Console.WriteLine("Calculated Gramms of Alcohol: " + grammsAlcohol);

            return grammsAlcohol;
        }

        /// <summary>
        /// Method which suggests a list of common drinks to consume in order to
        /// reach the ballmer peak dependant on the gramms of alcohol needed.
        /// </summary>
        /// <param name="grammsAlcohol">Gramms of alcohol needed for the Ballmer Peak</param>
        /// <returns></returns>
        public static Dictionary<String, double> SuggestDrinks(double grammsAlcohol)
        {
            Dictionary<String, double> suggestedDrinks = new Dictionary<String, double>();
            String[] standardDrinks = { "Gin 1,5 oz", "Beer 12 oz", "Wine 5 oz"};

            foreach(String drink in standardDrinks)
            {
                suggestedDrinks.Add("Standard sized " + drink, grammsAlcohol / 14);
                Console.WriteLine("Calculated drink: " + drink + " " + grammsAlcohol / 14);
            }
            suggestedDrinks.Add("Pure Alcohol", grammsAlcohol);

            return suggestedDrinks;
        }
    }
}

