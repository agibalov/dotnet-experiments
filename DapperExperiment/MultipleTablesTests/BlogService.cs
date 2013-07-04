using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace DapperExperiment.MultipleTablesTests
{
    public class BlogService
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly UserDAO _userDao = new UserDAO();
        private readonly PostDAO _postDao = new PostDAO();

        public BlogService(DatabaseHelper databaseHelper)
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

                connection.Execute(
                    "create table Posts(" +
                    "PostId int not null identity(1, 1) primary key, " +
                    "RowGuid uniqueidentifier not null, " +
                    "PostText nvarchar(256) not null, " + 
                    "UserId int not null)");
            }
        }

        public UserDTO CreateUser(string userName)
        {
            using (var connection = _databaseHelper.MakeConnection())
            {
                var userRow = _userDao.CreateUser(connection, userName);
                var postRows = _postDao.GetUserPosts(connection, userRow.UserId);
                var userDto = MakeUserDTO(userRow, postRows);
                return userDto;
            }
        }

        public PostDTO CreatePost(int userId, string postText)
        {
            using (var connection = _databaseHelper.MakeConnection())
            {
                var userRow = _userDao.GetUserOrThrow(connection, userId);
                var postRow = _postDao.CreatePost(connection, userId, postText);
                var briefUserDto = MakeBriefUserDTO(userRow);

                return new PostDTO
                    {
                        PostId = postRow.PostId,
                        PostText = postRow.PostText,
                        User = briefUserDto
                    };
            }
        }

        public UserDTO GetUser(int userId)
        {
            using (var connection = _databaseHelper.MakeConnection())
            {
                var userRow = _userDao.GetUserOrThrow(connection, userId);
                var postRows = _postDao.GetUserPosts(connection, userId);
                var userDto = MakeUserDTO(userRow, postRows);
                return userDto;
            }
        }

        private static UserDTO MakeUserDTO(UserRow userRow, IList<PostRow> postRows)
        {
            return new UserDTO
            {
                UserId = userRow.UserId,
                UserName = userRow.UserName,
                Posts = (from postRow in postRows
                         select new PostDTO
                         {
                             PostId = postRow.PostId,
                             PostText = postRow.PostText,
                             User = new BriefUserDTO
                             {
                                 UserId = userRow.UserId,
                                 UserName = userRow.UserName
                             }
                         }).ToList()
            };
        }

        private static BriefUserDTO MakeBriefUserDTO(UserRow userRow)
        {
            return new BriefUserDTO
                {
                    UserId = userRow.UserId,
                    UserName = userRow.UserName
                };
        }
    }
}