using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab3ParkhomenkoCSharp2019.Tools;
using Lab3ParkhomenkoCSharp2019.Tools.Managers;
using System.ComponentModel.DataAnnotations;

namespace Lab3ParkhomenkoCSharp2019.ViewModels.Date
{
    class Person : BaseViewModel
    {
        private string _name;
        private string _lastName;
        private DateTime? _birthDate;
        private string _email;

        public Person(string name, string lastName, DateTime? birthDate, string email)
        {
            _name = name;
            _lastName = lastName;
            _birthDate = birthDate;
            if(!new EmailAddressAttribute().IsValid(email))
                throw new EmailException("Incorrect email: ",email);
            _email = email;
        }

        public Person(string name, string lastName, string email) : this(name, lastName, DateTime.Now, email)
        {       
        }
        public Person(string name, string lastName, DateTime? birthDate) : this(name, lastName, birthDate, "")
        {
        }

        public string Name
        {
            get { return _name; }
        }

        public string LastName
        {
            get { return _lastName; }
        }

        public DateTime? BirthDate
        {
            get { return _birthDate; }
        }

        public string Email
        {
            get { return _email; }
        }

        private enum WesternZodiacSign
        {
            Aries,
            Taurus,
            Gemini,
            Cancer,
            Leo,
            Virgo,
            Libra,
            Scorpio,
            Sagittarius,
            Capricorn,
            Aquarius,
            Pisces
        }

        private enum ChineseZodiacSign
        {
            Monkey = 0,
            Rooster,
            Dog,
            Pig,
            Rat,
            Ox,
            Tiger,
            Rabbit,
            Dragon,
            Snake,
            Horse,
            Sheep,
        }

        public bool IsAdult()
        {
            int age = GetAge();
            if (age > 135)
                throw new PersonDiedException("User are too old, age: ",age);
            if(age < 1)
                throw new PersonTooYoungException("User are too young");
            return age > 17;
        }

        public string SunSign()
        {
            DateTime birthDate = Convert.ToDateTime(_birthDate);
            switch (birthDate.Month)
            {
                case 1:
                    if (birthDate.Day < 20)
                        return WesternZodiacSign.Capricorn.ToString();
                    return WesternZodiacSign.Aquarius.ToString();
                case 2:
                    if (birthDate.Day < 19)
                        return WesternZodiacSign.Aquarius.ToString();
                    return WesternZodiacSign.Pisces.ToString();
                case 3:
                    if (birthDate.Day < 21)
                        return WesternZodiacSign.Pisces.ToString();
                    return WesternZodiacSign.Aries.ToString();
                case 4:
                    if (birthDate.Day < 20)
                        return WesternZodiacSign.Aries.ToString();
                    return WesternZodiacSign.Taurus.ToString();
                case 5:
                    if (birthDate.Day < 21)
                        return WesternZodiacSign.Taurus.ToString();
                    return WesternZodiacSign.Gemini.ToString();
                case 6:
                    if (birthDate.Day < 22)
                        return WesternZodiacSign.Gemini.ToString();
                    return WesternZodiacSign.Cancer.ToString();
                case 7:
                    if (birthDate.Day < 23)
                        return WesternZodiacSign.Cancer.ToString();
                    return WesternZodiacSign.Leo.ToString();
                case 8:
                    if (birthDate.Day < 23)
                        return WesternZodiacSign.Leo.ToString();
                    return WesternZodiacSign.Virgo.ToString();
                case 9:
                    if (birthDate.Day < 23)
                        return WesternZodiacSign.Virgo.ToString();
                    return WesternZodiacSign.Libra.ToString();
                case 10:
                    if (birthDate.Day < 24)
                        return WesternZodiacSign.Libra.ToString();
                    return WesternZodiacSign.Scorpio.ToString();
                case 11:
                    if (birthDate.Day < 23)
                        return WesternZodiacSign.Scorpio.ToString();
                    return WesternZodiacSign.Sagittarius.ToString();
                case 12:
                    if (birthDate.Day < 22)
                        return WesternZodiacSign.Sagittarius.ToString();
                    return WesternZodiacSign.Capricorn.ToString();
                default:
                    return null;
            }
        }

        public string ChineseSign()
        {
            DateTime birthDate = Convert.ToDateTime(_birthDate);
            return ((ChineseZodiacSign)(birthDate.Year % 12)).ToString();
        }

        public bool IsBirthday()
        {
            DateTime currentDate = DateTime.Now;
            DateTime birthDate = Convert.ToDateTime(_birthDate);
            if (currentDate.Month == birthDate.Month && currentDate.Day == birthDate.Day)
            {
                return true;
            }

            return false;
        }

        private int GetAge()
        {
            DateTime birthDate = Convert.ToDateTime(_birthDate);
            DateTime currentDate = DateTime.Now;
            if ((birthDate.Year == currentDate.Year && birthDate.Month < currentDate.Month) ||
                (birthDate.Year == currentDate.Year && birthDate.Month == currentDate.Month && birthDate.Day < currentDate.Day))
                return 1;
            if ((birthDate.Year + 1 == currentDate.Year && birthDate.Month > currentDate.Month) ||
                (birthDate.Year + 1 == currentDate.Year && birthDate.Month == currentDate.Month && birthDate.Day > currentDate.Day))
                return 1;
            int age = currentDate.Year - birthDate.Year;

            if ((currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day) ||
                (currentDate.Month < birthDate.Month))
                return --age;
            return age;
        }
    }
}
