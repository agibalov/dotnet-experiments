using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace DapperExperiment.SingleTableTests
{
    public class UserDAO
    {
        private readonly DatabaseHelper _databaseHelper;

        public UserDAO(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public void CreateSchema()
        {
            using (var connection = _databaseHelper.MakeConnection())
            {
                connection.Execute(
                    "create table Users(" +
                    "UserId int not null identity(1, 1) primary key, " +
                    "RowGuid uniqueidentifier not null, " +
                    "UserName nvarchar(256) not null)");
            }
        }

        public UserRow CreateUser(string userName)
        {
            using (var connection = _databaseHelper.MakeConnection())
            {
                var rowGuid = Guid.NewGuid();

                connection.Execute(
                    "insert into Users(RowGuid, UserName) " + 
                    "values(@rowGuid, @userName)",
                    new { rowGuid, userName });

                var userRow = connection.Query<UserRow>(
                    "select UserId, UserName " +
                    "from Users " +
                    "where RowGuid = @rowGuid", 
                    new { rowGuid }).Single();

                return userRow;
            }
        }

        public UserRow GetUser(int userId)
        {
            using (var connection = _databaseHelper.MakeConnection())
            {
                var userRow = connection.Query<UserRow>(
                    "select UserId, UserName " + 
                    "from Users " + 
                    "where UserId = @userId", 
                    new { userId }).Single();

                return userRow;
            }
        }

        public IList<UserRow> GetUsers(IList<int> userIds)
        {
            using (var connection = _databaseHelper.MakeConnection())
            {
                var userRows = connection.Query<UserRow>(
                    "select UserId, UserName " + 
                    "from Users " + 
                    "where UserId in @userIds", 
                    new { userIds }).ToList();

                return userRows;
            }
        }

        public IList<UserRow> GetAllUsers()
        {
            using (var connection = _databaseHelper.MakeConnection())
            {
                var userRows = connection.Query<UserRow>(
                    "select UserId, UserName " + 
                    "from Users").ToList();

                return userRows;
            }
        }

        public Page<UserRow> GetAllUsers(int itemsPerPage, int page)
        {
            using (var connection = _databaseHelper.MakeConnection())
            {
                var userCount = connection.Query<int>(
                    "select count(*) " + 
                    "from Users").Single();

                var skip = page * itemsPerPage;
                if (skip >= userCount)
                {
                    throw new Exception("No such page");
                }

                var userRows = connection.Query<UserRow>(
                    "select UserId, UserName " + 
                    "from Users " + 
                    "order by UserId " +
                    "offset @skip rows " + 
                    "fetch next @take rows only",
                    new { skip, take = itemsPerPage }).ToList();

                return new Page<UserRow>
                    {
                        NumberOfItems = userCount,
                        NumberOfPages = userCount / itemsPerPage + (userCount % itemsPerPage > 0 ? 1 : 0),
                        CurrentPage = page,
                        Items = userRows
                    };
            }
        }

        public void DeleteUser(int userId)
        {
            using (var connection = _databaseHelper.MakeConnection())
            {
                connection.Execute(
                    "delete from Users " + 
                    "where UserId = @userId", 
                    new { userId });
            }
        }

        public void DeleteUsers(IList<int> userIds)
        {
            using (var connection = _databaseHelper.MakeConnection())
            {
                connection.Execute(
                    "delete from Users " + 
                    "where UserId in @userIds", 
                    new { userIds });
            }
        }

        public UserRow ChangeUserName(int userId, string userName)
        {
            using (var connection = _databaseHelper.MakeConnection())
            {
                connection.Execute(
                    "update Users " + 
                    "set UserName = @userName " + 
                    "where UserId = @userId", 
                    new { userId, userName });
                
                var userRow = connection.Query<UserRow>(
                    "select UserId, UserName " + 
                    "from Users " + 
                    "where UserId = @userId", 
                    new { userId }).Single();

                return userRow;
            }
        }

        public int GetUserCount()
        {
            using (var connection = _databaseHelper.MakeConnection())
            {
                var userCount = connection.Query<int>(
                    "select count(*) " + 
                    "from Users").Single();

                return userCount;
            }
        }
    }

    public class Page<T>
    {
        public int NumberOfItems { get; set; }
        public int NumberOfPages { get; set; }
        public int CurrentPage { get; set; }
        public IList<T> Items { get; set; }
    }
}