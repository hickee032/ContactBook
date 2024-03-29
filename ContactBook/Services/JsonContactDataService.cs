﻿using ContactBook.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Services
{
    public class JsonContactDataService : IContactDataService
    {
        private readonly string _dataPath = "C:\\Users\\bluep\\source\\repos\\ContactBook\\ContactBook\\Resources\\contactdata.json";
        public IEnumerable<Contact> GetContacts() {
            if (!File.Exists(_dataPath)) {
                File.Create(_dataPath).Close();
            }

            var serializedContacts = File.ReadAllText(_dataPath);
            var contacts = JsonConvert.DeserializeObject<IEnumerable<Contact>>(serializedContacts);

            if (contacts == null) {
                return new List<Contact>();
            }
            return contacts;
        }

        public void Save(IEnumerable<Contact> contacts) {
            
            var serializedContacts = JsonConvert.SerializeObject(contacts);
            File.WriteAllText(_dataPath, serializedContacts);
        }
    }
}
