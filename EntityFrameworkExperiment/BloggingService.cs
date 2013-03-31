using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DAL.Entities;
using EntityFrameworkExperiment.DTO;

namespace EntityFrameworkExperiment
{
    public class BloggingService
    {
        public ServiceResult<UserDTO> CreateUser(string userName, string password)
        {
            using (var context = new BlogContext())
            {
                var existingUser = context.Users.SingleOrDefault(u => u.UserName == userName);
                if (existingUser != null)
                {
                    return ServiceResult<UserDTO>.Failure(ServiceError.UserNameAlreadyRegistered);
                }

                var user = new User
                    {
                        UserName = userName, 
                        Password = password
                    };
                user = context.Users.Add(user);
                context.SaveChanges();

                return ServiceResult<UserDTO>.Success(MapUserToUserDTO(user));
            }
        }

        public ServiceResult<SessionDTO> Authenticate(string userName, string password)
        {
            using (var context = new BlogContext())
            {
                var user = context.Users.SingleOrDefault(u => u.UserName == userName);
                if (user == null)
                {
                    return ServiceResult<SessionDTO>.Failure(ServiceError.NoSuchUser);
                }

                if (user.Password != password)
                {
                    return ServiceResult<SessionDTO>.Failure(ServiceError.InvalidPassword);
                }

                var session = new Session
                    {
                        SessionToken = Guid.NewGuid().ToString(), 
                        User = user
                    };

                session = context.Sessions.Add(session);
                context.SaveChanges();

                return ServiceResult<SessionDTO>.Success(MapSessionToSessionDTO(session));
            }
        }

        public ServiceResult<PostDTO> CreatePost(string sessionToken, string postText)
        {
            using (var context = new BlogContext())
            {
                var session = context.Sessions.SingleOrDefault(s => s.SessionToken == sessionToken);
                if (session == null)
                {
                    return ServiceResult<PostDTO>.Failure(ServiceError.InvalidSession);
                }

                var post = new Post
                    {
                        User = session.User, 
                        Text = postText
                    };
                
                post = context.Posts.Add(post);
                context.SaveChanges();

                return ServiceResult<PostDTO>.Success(MapPostToPostDTO(post));
            }
        }

        public ServiceResult<PostDTO> GetPost(string sessionToken, int postId)
        {
            using (var context = new BlogContext())
            {
                var session = context.Sessions.SingleOrDefault(s => s.SessionToken == sessionToken);
                if (session == null)
                {
                    return ServiceResult<PostDTO>.Failure(ServiceError.InvalidSession);
                }

                var post = context.Posts.SingleOrDefault(p => p.PostId == postId);
                if (post == null)
                {
                    return ServiceResult<PostDTO>.Failure(ServiceError.NoSuchPost);
                }

                return ServiceResult<PostDTO>.Success(MapPostToPostDTO(post));
            }
        }

        public IList<PostDTO> GetPosts(string sessionToken, int itemsPerPage, int page)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<PostDTO> UpdatePost(string sessionToken, int postId, string postText)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<Object> DeletePost(string sessionToken, int postId)
        {
            throw new NotImplementedException();
        }

        private static UserDTO MapUserToUserDTO(User user)
        {
            return new UserDTO
            {
                UserId = user.UserId,
                UserName = user.UserName
            };
        }

        private static SessionDTO MapSessionToSessionDTO(Session session)
        {
            return new SessionDTO
                {
                    SessionToken = session.SessionToken,
                    User = MapUserToUserDTO(session.User)
                };
        }

        private static PostDTO MapPostToPostDTO(Post post)
        {
            return new PostDTO
                {
                    PostId = post.PostId,
                    PostText = post.Text,
                    Author = MapUserToUserDTO(post.User)
                };
        }
    }
}