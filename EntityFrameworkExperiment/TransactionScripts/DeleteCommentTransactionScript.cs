using System.Linq;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.Exceptions;

namespace EntityFrameworkExperiment.TransactionScripts
{
    public class DeleteCommentTransactionScript
    {
        private readonly AuthenticationService _authenticationService;

        public DeleteCommentTransactionScript(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public void DeleteComment(BlogContext context, string sessionToken, int commentId)
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

            context.Comments.Remove(comment);
            context.SaveChanges();
        }
    }
}