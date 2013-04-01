﻿using System;
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
        [Inject] public UpdatePostTransactionScript UpdatePostTransactionScript { get; set; }
        [Inject] public DeletePostTransactionScript DeletePostTransactionScript { get; set; }
        [Inject] public GetPostsRequestProcessor GetPostsRequestProcessor { get; set; }
        [Inject] public GetUserDetailsTransactionScript GetUserDetailsTransactionScript { get; set; }

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

        public ServiceResult<Page<PostDTO>> GetPosts(string sessionToken, int itemsPerPage, int page)
        {
            return ExecuteWithExceptionHandling(
                context => GetPostsRequestProcessor.GetPosts(
                    context, 
                    sessionToken, 
                    itemsPerPage, 
                    page));
        }

        public ServiceResult<PostDTO> UpdatePost(string sessionToken, int postId, string postText)
        {
            return ExecuteWithExceptionHandling(
                context => UpdatePostTransactionScript.UpdatePost(
                    context, 
                    sessionToken, 
                    postId, 
                    postText));
        }

        public ServiceResult<Object> DeletePost(string sessionToken, int postId)
        {
            return ExecuteWithExceptionHandling(
                context => DeletePostTransactionScript.DeletePost(
                    context, 
                    sessionToken, 
                    postId));
        }

        public ServiceResult<UserDetailsDTO> GetUserDetails(string sessionToken, int userId, int maxNumberOfRecentPosts)
        {
            return ExecuteWithExceptionHandling(
                context => GetUserDetailsTransactionScript.GetUserDetails(
                    context, 
                    sessionToken, 
                    userId, 
                    maxNumberOfRecentPosts));
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

        private static ServiceResult<Object> ExecuteWithExceptionHandling(Action<BlogContext> action)
        {
            return ExecuteWithExceptionHandling<Object>(context =>
                {
                    action(context);
                    return null;
                });
        }
    }
}