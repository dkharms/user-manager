using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using UserManager.Models;

namespace UserManager.Controllers
{
    public static class UserController
    {
        public static List<User> UserList = new List<User>();

        /// <summary>
        /// Создание объекта пользователя и запись информации о нем в БД.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="patronymic"></param>
        /// <returns></returns>
        public static User CreateUser(string login, string firstName, string lastName, string patronymic)
        {
            int id = UserList.Count == 0 ? 1 : UserList.Max(x => x.Id) + 1;
            DateTime creationDate = DateTime.Now;
            User user = new User(id, login, firstName, lastName, patronymic, creationDate, false);

            UserList.Add(user);
            DatabaseController.CreateUserRecord(user);

            return user;
        }

        /// <summary>
        /// Создание объекта пользователя на основе информации, полученной из БД.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="login"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="patronymic"></param>
        /// <param name="creationDate"></param>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
        public static User AssembleUserFromRecord(int id, string login, string firstName, string lastName,
            string patronymic, DateTime creationDate, bool isDeleted)
        {
            User user = new User(id, login, firstName, lastName, patronymic, creationDate, isDeleted);
            UserList.Add(user);

            return user;
        }

        /// <summary>
        /// Обновление модели пользователя и последующее обновление информации о нем в БД.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="login"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="patronymic"></param>
        public static void UpdateUser(User user, string login, string firstName, string lastName, string patronymic)
        {
            user.Login = login;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Patronymic = patronymic;

            DatabaseController.UpdateUserRecord(user);
        }

        /// <summary>
        /// Изменение статуса пользователя (удален он или нет). 
        /// </summary>
        /// <param name="user"></param>
        public static void ChangeUserStatus(User user)
        {
            user.IsDeleted = !user.IsDeleted;

            DatabaseController.UpdateUserRecord(user);
        }


        /// <summary>
        /// Экспорт пользователей в .xlsx файл.
        /// </summary>
        /// <param name="filePath"></param>
        public static void ExportUsers(string filePath)
        {
            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(filePath)))
            {
                var allUsersWorksheet = excelPackage.Workbook.Worksheets.Add("ALL USERS");
                allUsersWorksheet.Cells[1, 6, UserList.Count + 1, 6].Style.Numberformat.Format = "mm-dd-yyyy hh:mm";
                var range = allUsersWorksheet.Cells["A1"].LoadFromCollection(UserList, true);
                range.AutoFitColumns();

                var nonDeletedUsersWorksheet = excelPackage.Workbook.Worksheets.Add("NON DELETED USERS");
                nonDeletedUsersWorksheet.Cells[1, 6, UserList.Count + 1, 6].Style.Numberformat.Format =
                    "mm-dd-yyyy hh:mm";
                range = nonDeletedUsersWorksheet.Cells["A1"]
                    .LoadFromCollection(UserList.Where(x => !x.IsDeleted), true);
                range.AutoFitColumns();

                excelPackage.Save();
            }
        }
    }
}