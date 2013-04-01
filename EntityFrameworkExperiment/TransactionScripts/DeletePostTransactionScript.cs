using System.Linq;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.Exceptions;

namespace EntityFrameworkExperiment.TransactionScripts
{
    public class DeletePostTransactionScript
    {
        private readonly AuthenticationService _authenticationService;

        public DeletePostTransactionScript(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public void DeletePost(BlogContext context, string sessionToken, int postId)
        {
            var user = _authenticationService.GetUserBySessionToken(context, sessionToken);
            var post = context.Posts.SingleOrDefault(p => p.PostId == postId);
            if (post == null)
            {
                throw new NoSuchPostException();
            }

            if (post.UserId != user.UserId)
            {
                throw new NoPermissionsException();
            }

            context.Posts.Remove(post);
            context.SaveChanges();
        }
    }
}