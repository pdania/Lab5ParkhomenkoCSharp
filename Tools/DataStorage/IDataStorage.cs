using System.Collections.Generic;
using Lab4ParkhomenkoCSharp2019.ViewModels.Date;

namespace Lab4ParkhomenkoCSharp2019.Tools.DataStorage
{
    internal interface IDataStorage
    {
        bool UserExists(string email);
        Person GetUserByEmail(string email);

        void AddUser(Person person);
        List<Person> UsersList { get; }
        void SaveChanges();
    }
}