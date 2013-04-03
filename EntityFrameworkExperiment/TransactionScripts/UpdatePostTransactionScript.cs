using System;
using System.Linq;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DTO;
using EntityFrameworkExperiment.Exceptions;
using EntityFrameworkExperiment.Mappers;

namespace EntityFrameworkExperiment.TransactionScripts
{
    public class UpdatePostTransactionScript
    {
        private readonly AuthenticationService _authenticationService;
        private readonly PostToPostDTOMapper _postToPostDtoMapper;

        public UpdatePostTransactionScript(
            AuthenticationService authenticationService,
            PostToPostDTOMapper postToPostDtoMapper)
        {
            _authenticationService = authenticationService;
            _postToPostDtoMapper = postToPostDtoMapper;
        }

        public PostDTO UpdatePost(BlogContext context, string sessionToken, int postId, string postText)
        {
            var user = _authenticationService.GetUserBySessionToken(context, sessionToken);
            var post = context.Posts
                .Include("Comments")
                .SingleOrDefault(p => p.PostId == postId);
            if (post == null)
            {
                throw new NoSuchPostException();
            }

            if (post.UserId != user.UserId)
            {
                throw new NoPermissionsException();
            }

            post.Text = postText;
            post.ModifiedAt = DateTime.UtcNow;

            context.SaveChanges();

            return _postToPostDtoMapper.Map(post);
        }
    }
}