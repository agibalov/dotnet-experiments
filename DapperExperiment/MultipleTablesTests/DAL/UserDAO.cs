using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace DapperExperiment.MultipleTablesTests.DAL
{
    public class UserDAO
    {
        public UserRow CreateUser(IDbConnection connection, string userName)
        {
            var rowGuid = Guid.NewGuid();
            connection.Execute(
                "insert into Users(RowGuid, UserName) " +
                "values (@rowGuid, @userName)",
                new { rowGuid, userName });

            var userRow = connection.Query<UserRow>(
                "select UserId, UserName " +
                "from Users " +
                "where RowGuid = @rowGuid",
                new { rowGuid }).Single();

            return userRow;
        }

        public UserRow GetUser(IDbConnection connection, int userId)
        {
            var userRow = connection.Query<UserRow>(
                "select UserId, UserName " +
                "from Users " +
                "where UserId = @userId",
                new { userId }).SingleOrDefault();

            return userRow;
        }

        public UserRow GetUserOrThrow(IDbConnection connection, int userId)
        {
            var userRow = GetUser(connection, userId);
            if (userRow == null)
            {
                throw new NoSuchUserException();
            }

            return userRow;
        }

        public IList<UserRow> GetUsers(IDbConnection connection, IList<int> userIds)
        {
            var userRows = connection.Query<UserRow>(
                "select UserId, UserName " + 
                "from Users " + 
                "where UserId in @userIds",
                new {userIds}).ToList();

            return userRows;
        }

        public IList<UserRow> GetUsersOrThrow(IDbConnection connection, IList<int> userIds)
        {
            var userRows = GetUsers(connection, userIds);
            if (userRows.Count != userIds.Count)
            {
                throw new NoSuchUserException();
            }

            return userRows;
        }

        public IList<UserRow> GetAllUsers(IDbConnection connection)
        {
            var userRows = connection.Query<UserRow>(
                "select UserId, UserName " +
                "from Users ").ToList();

            return userRows;
        }

        public int GetUserCount(IDbConnection connection)
        {
            var userCount = connection.Query<int>(
                "select count(*) " +
                "from Users").Single();
            
            return userCount;
        }

        public UserRow UpdateUser(IDbConnection connection, int userId, string userName)
        {
            connection.Execute(
                "update Users " +
                "set UserName = @userName " +
                "where UserId = @userId",
                new { userId, userName });

            return GetUserOrThrow(connection, userId);
        }

        public void DeleteUser(IDbConnection connection, int userId)
        {
            connection.Execute(
                "delete from Users " +
                "where UserId = @userId",
                new {userId});
        }

        public void DeleteUsers(IDbConnection connection, IList<int> userIds)
        {
            connection.Execute(
                "delete from Users " +
                "where UserId in @userIds",
                new { userIds });
        }
    }
}