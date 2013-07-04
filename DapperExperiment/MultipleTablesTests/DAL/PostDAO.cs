using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using Dapper;

namespace DapperExperiment.MultipleTablesTests.DAL
{
    public class PostDAO
    {
        public PostRow CreatePost(IDbConnection connection, int userId, string postText)
        {
            var rowGuid = Guid.NewGuid();
            connection.Execute(
                "insert into Posts(RowGuid, PostText, UserId) " +
                "values(@rowGuid, @postText, @userId)",
                new { rowGuid, postText, userId });

            var postRow = connection.Query<PostRow>(
                "select P.PostId, P.PostText, U.UserId, U.UserName " +
                "from Posts as P " + 
                "join Users as U on P.UserId = U.UserId " +
                "where P.RowGuid = @rowGuid",
                new { rowGuid }).Single();

            return postRow;
        }

        public IList<PostRow> GetUserPosts(IDbConnection connection, int userId)
        {
            var postRows = connection.Query<PostRow>(
                "select P.PostId, P.PostText, U.UserId, U.UserName " +
                "from Posts as P " +
                "join Users as U on P.UserId = U.UserId " +
                "where U.UserId = @userId",
                new { userId }).ToList();

            return postRows;
        }

        public PostRow GetPost(IDbConnection connection, int postId)
        {
            var postRow = connection.Query<PostRow>(
                "select P.PostId, P.PostText, U.UserId, U.UserName " +
                "from Posts as P " +
                "join Users as U on P.UserId = U.UserId " +
                "where P.PostId = @postId",
                new { postId }).SingleOrDefault();

            return postRow;
        }

        public PostRow GetPostOrThrow(IDbConnection connection, int postId)
        {
            var postRow = GetPost(connection, postId);
            if (postRow == null)
            {
                throw new NoSuchPostException();
            }

            return postRow;
        }

        public IList<PostRow> GetPostsForManyUsers(IDbConnection connection, IList<int> userIds)
        {
            var postRows = connection.Query<PostRow>(
                "select P.PostId, P.PostText, U.UserId, U.UserName " +
                "from Posts as P " +
                "join Users as U on P.UserId = U.UserId " +
                "where U.UserId in @userIds",
                new { userIds }).ToList();

            return postRows;
        }
    }
}