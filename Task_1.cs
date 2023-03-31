using System;

namespace First
{
    class Program
    {
        static int inputNumber(string text)
        {
            Console.WriteLine(text);
            return Convert.ToInt32(Console.ReadLine());
        }

        static void Main()
        {
            int day = inputNumber("Enter a day of birth: ");
            int month = inputNumber("Enter a month of birth: ");
            int year = inputNumber("Enter a year of birth: ");
            DateTime dateBirth = new DateTime(year, month, day);
            DateTime fullBirth = dateBirth.AddYears(18);
            DateTime dateNow = DateTime.Now;
            int yourYears = dateNow.Year - dateBirth.Year;
            int d = 0;

            if ( dateNow.CompareTo(fullBirth) == -1)
            {
                Console.WriteLine("You're not adult! Try again!");
                return;
            }
            int yourMonths = ((yourYears) * 12) + (dateNow.Month - dateBirth.Month);
            if(((yourYears) * 12) > yourMonths)
            {
               yourYears--;
               d = dateBirth.DayOfYear - dateNow.DayOfYear;
            }
            else { 
                d = dateNow.DayOfYear - dateBirth.DayOfYear;
            }
            Console.WriteLine("Your years: " + yourYears);
            Console.WriteLine("Your days (without leap years): " + ((yourYears * 365) + d));
            Console.WriteLine("Your months: " + yourMonths);
            Console.WriteLine("The year of adult: " + fullBirth.ToString("yyyy"));
        }
    }
}