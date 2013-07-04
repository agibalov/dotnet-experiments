using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using Dapper;

namespace DapperExperiment.MultipleTablesTests
{
    public class PostDAO
    {
        public PostRow CreatePost(SqlCeConnection connection, int userId, string postText)
        {
            var rowGuid = Guid.NewGuid();
            connection.Execute(
                "insert into Posts(RowGuid, PostText, UserId) " +
                "values(@rowGuid, @postText, @userId)",
                new { rowGuid, postText, userId });

            var postRow = connection.Query<PostRow>(
                "select PostId, PostText, UserId " +
                "from Posts where RowGuid = @rowGuid",
                new { rowGuid }).Single();

            return postRow;
        }

        public IList<PostRow> GetUserPosts(SqlCeConnection connection, int userId)
        {
            var postRows = connection.Query<PostRow>(
                "select PostId, PostText, UserId " +
                "from Posts " +
                "where UserId = @userId",
                new { userId }).ToList();

            return postRows;
        }
    }
}