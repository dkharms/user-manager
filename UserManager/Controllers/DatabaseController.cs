using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using UserManager.Models;

namespace UserManager.Controllers
{
    public static class DatabaseController
    {
        
        /// <summary>
        /// Создание БД и таблицы, если она отсутствует.
        /// </summary>
        public static void CreateDatabase()
        {
            SQLiteConnection.CreateFile("Database.sqlite");
            var dbConnection = new SQLiteConnection("Data Source=Database.sqlite;Version=3;");
            dbConnection.Open();

            string sql =
                "CREATE TABLE users " + 
                "(id INTEGER PRIMARY KEY, login VARCHAR(20), first_name VARCHAR(20), " +
                "last_name VARCHAR(20), patronymic VARCHAR(20), creation_date TEXT, " +
                "is_deleted INTEGER DEFAULT 0);";
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();

            dbConnection.Close();
        }

        /// <summary>
        /// Создание записи в БД о пользователе.
        /// </summary>
        /// <param name="user"></param>
        public static void CreateUserRecord(User user)
        {
            var dbConnection = new SQLiteConnection("Data Source=Database.sqlite;Version=3;");
            dbConnection.Open();

            string sql =
                $"INSERT INTO users (id, login, first_name, last_name, patronymic, creation_date) " +
                $"values ('{user.Id}', '{user.Login}', '{user.FirstName}', " +
                $"'{user.LastName}', '{user.Patronymic}', '{user.CreationDate:yyyy-MM-dd HH:mm:ss.fff}');";

            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();

            dbConnection.Close();
        }

        /// <summary>
        /// Получение списка пользователей из БД и добавление их во внутреннее представление программы.
        /// </summary>
        public static void GetUsers()
        {
            var dbConnection = new SQLiteConnection("Data Source=Database.sqlite;Version=3;");
            dbConnection.Open();

            string sql = "SELECT * FROM users";
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            UserController.UserList.Clear();
            
            using (var sqlReader = command.ExecuteReader())
            {
                while (sqlReader.Read())
                {
                    int id = int.Parse(sqlReader["id"].ToString());

                    string login = sqlReader["login"].ToString();
                    string firstName = sqlReader["first_name"].ToString();
                    string lastName = sqlReader["last_name"].ToString();
                    string patronymic = sqlReader["patronymic"].ToString();

                    bool isDeleted = int.Parse(sqlReader["is_deleted"].ToString()) == 1;
                    DateTime creationDate = DateTime.Parse(sqlReader["creation_date"].ToString());
                    
                    UserController.AssembleUserFromRecord(id, login, firstName, lastName, patronymic, creationDate, isDeleted);
                }
            }

            dbConnection.Close();
        }

        /// <summary>
        /// Обновление данных о конкретном пользователе в БД.
        /// </summary>
        /// <param name="user"></param>
        public static void UpdateUserRecord(User user)
        {
            var dbConnection = new SQLiteConnection("Data Source=Database.sqlite;Version=3;");
            dbConnection.Open();

            string sql = "UPDATE users SET " +
                         $"login = '{user.Login}', first_name = '{user.FirstName}', last_name = '{user.LastName}', " +
                         $"patronymic = '{user.Patronymic}', is_deleted = '{(user.IsDeleted ? 1 : 0)}' " +
                         $"WHERE id = {user.Id};";

            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();
            
            dbConnection.Close();
        }
    }
}