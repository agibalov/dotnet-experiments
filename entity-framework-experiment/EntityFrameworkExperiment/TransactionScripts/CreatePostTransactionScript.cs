using System;
using System.Collections.Generic;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DAL.Entities;
using EntityFrameworkExperiment.DTO;
using EntityFrameworkExperiment.Mappers;

namespace EntityFrameworkExperiment.TransactionScripts
{
    public class CreatePostTransactionScript
    {
        private readonly AuthenticationService _authenticationService;
        private readonly PostToPostDTOMapper _postToPostDtoMapper;

        public CreatePostTransactionScript(
            AuthenticationService authenticationService,
            PostToPostDTOMapper postToPostDtoMapper)
        {
            _authenticationService = authenticationService;
            _postToPostDtoMapper = postToPostDtoMapper;
        }

        public PostDTO CreatePost(BlogContext context, string sessionToken, string postText)
        {
            var user = _authenticationService.GetUserBySessionToken(context, sessionToken);
            var post = new Post
                {
                    User = user,
                    Text = postText,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = null,
                    Comments = new List<Comment>()
                };

            post = context.Posts.Add(post);
            context.SaveChanges();

            return _postToPostDtoMapper.Map(post);
        }
    }
}