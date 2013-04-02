using System;
using System.Linq;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.Exceptions;

namespace EntityFrameworkExperiment.TransactionScripts
{
    public class UpdateCommentTransactionScript
    {
        private readonly AuthenticationService _authenticationService;

        public UpdateCommentTransactionScript(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public void UpdateComment(BlogContext context, string sessionToken, int commentId, string commentText)
        {
            var user = _authenticationService.GetUserBySessionToken(context, sessionToken);

            var comment = context.Comments.SingleOrDefault(c => c.CommentId == commentId);
            if (comment == null)
            {
                throw new NoSuchCommentException();
            }

            if (comment.UserId != user.UserId)
            {
                throw new NoPermissionsException();
            }

            comment.Text = commentText;
            comment.ModifiedAt = DateTime.UtcNow;

            context.SaveChanges();
        }
    }
}