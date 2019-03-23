using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lab4ParkhomenkoCSharp2019.ViewModels.Date;
using Lab4ParkhomenkoCSharp2019.Tools.Managers;

namespace Lab4ParkhomenkoCSharp2019.Tools.DataStorage
{
    internal class SerializedDataStorage : IDataStorage
    {
        private readonly List<Person> _users;

        public SerializedDataStorage()
        {
            try
            {
                _users = SerializationManager.Deserialize<List<Person>>(FileFolderHelper.StorageFilePath);
            }
            catch (FileNotFoundException)
            {
                _users = new List<Person>();
            }
        }

        public bool UserExists(string email)
        {
            return _users.Exists(u => u.Email == email);
        }

        public Person GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);
        }

        public void AddUser(Person person)
        {
            _users.Add(person);
            SaveChanges();
        }
        public List<Person> UsersList
        {
            get { return _users.ToList(); }
        }

        public void SaveChanges()
        {
            SerializationManager.Serialize(_users, FileFolderHelper.StorageFilePath);
        }

    }
}