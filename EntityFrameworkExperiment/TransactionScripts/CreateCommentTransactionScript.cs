using System;
using System.Linq;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DAL.Entities;
using EntityFrameworkExperiment.Exceptions;

namespace EntityFrameworkExperiment.TransactionScripts
{
    public class CreateCommentTransactionScript
    {
        private readonly AuthenticationService _authenticationService;

        public CreateCommentTransactionScript(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public void CreateComment(BlogContext context, string sessionToken, int postId, string commentText)
        {
            var user = _authenticationService.GetUserBySessionToken(context, sessionToken);

            var post = context.Posts.SingleOrDefault(p => p.PostId == postId);
            if (post == null)
            {
                throw new NoSuchPostException();
            }

            var comment = new Comment
                {
                    Text = commentText,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = null,
                    Post = post,
                    User = user
                };

            context.Comments.Add(comment);
            context.SaveChanges();
        }
    }
}