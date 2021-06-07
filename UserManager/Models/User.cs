﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace UserManager.Models
{
    public class User : IEnumerable<String>
    {
        public int Id { get; }

        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }

        public DateTime CreationDate { get; }
        public bool IsDeleted { get; set; }

        public User(int id, string login, string firstName, string lastName, string patronymic, DateTime creationDate, bool isDeleted)
        {
            Id = id;

            Login = login;
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
            
            CreationDate = creationDate;
            IsDeleted = isDeleted;
        }

        public IEnumerator<string> GetEnumerator()
        {
            yield return Id.ToString();
            yield return Login;
            yield return FirstName;
            yield return LastName;
            yield return Patronymic;
            yield return CreationDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
            yield return IsDeleted.ToString();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public override string ToString()
        {
            return $"{Id},{Login},{LastName},{FirstName},{Patronymic},{CreationDate},{IsDeleted}";
        }
    }
}