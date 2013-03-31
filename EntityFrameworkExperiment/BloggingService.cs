using System;
using System.Collections.Generic;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DTO;
using EntityFrameworkExperiment.Exceptions;
using EntityFrameworkExperiment.TransactionScripts;
using Ninject;

namespace EntityFrameworkExperiment
{
    public class BloggingService
    {
        [Inject] public CreateUserTransactionScript CreateUserTransactionScript { get; set; }
        [Inject] public AuthenticateTransactionScript AuthenticateTransactionScript { get; set; }
        [Inject] public CreatePostTransactionScript CreatePostTransactionScript { get; set; }
        [Inject] public GetPostTransactionScript GetPostTransactionScript { get; set; }

        public ServiceResult<UserDTO> CreateUser(string userName, string password)
        {
            return ExecuteWithExceptionHandling(
                context => CreateUserTransactionScript.CreateUser(
                    context, 
                    userName, 
                    password));
        }

        public ServiceResult<SessionDTO> Authenticate(string userName, string password)
        {
            return ExecuteWithExceptionHandling(
                context => AuthenticateTransactionScript.Authenticate(
                    context, 
                    userName, 
                    password));
        }

        public ServiceResult<PostDTO> CreatePost(string sessionToken, string postText)
        {
            return ExecuteWithExceptionHandling(
                context => CreatePostTransactionScript.CreatePost(
                    context, 
                    sessionToken, 
                    postText));
        }

        public ServiceResult<PostDTO> GetPost(string sessionToken, int postId)
        {
            return ExecuteWithExceptionHandling(
                context => GetPostTransactionScript.GetPost(
                    context, 
                    sessionToken, 
                    postId));
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

        private static ServiceResult<T> ExecuteWithExceptionHandling<T>(Func<BlogContext, T> func)
        {
            try
            {
                using (var context = new BlogContext())
                {
                    var result = func(context);
                    return ServiceResult<T>.Success(result);
                }
            }
            catch (BlogServiceException blogServiceException)
            {
                return ServiceResult<T>.Failure(blogServiceException.Error);
            }
        }
    }
}