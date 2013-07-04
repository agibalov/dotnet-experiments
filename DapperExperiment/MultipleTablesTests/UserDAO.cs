using System;
using System.Data.SqlServerCe;
using System.Linq;
using Dapper;

namespace DapperExperiment.MultipleTablesTests
{
    public class UserDAO
    {
        public UserRow CreateUser(SqlCeConnection connection, string userName)
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

        public UserRow GetUser(SqlCeConnection connection, int userId)
        {
            var userRow = connection.Query<UserRow>(
                "select UserId, UserName " +
                "from Users " +
                "where UserId = @userId",
                new { userId }).SingleOrDefault();

            return userRow;
        }

        public UserRow GetUserOrThrow(SqlCeConnection connection, int userId)
        {
            var userRow = GetUser(connection, userId);
            if (userRow == null)
            {
                throw new Exception("No such user");
            }

            return userRow;
        }
    }
}