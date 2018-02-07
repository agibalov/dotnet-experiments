using System.Collections.Generic;
using System.Linq;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DTO;

namespace EntityFrameworkExperiment.TransactionScripts
{
    public class GetMostActiveUsersTransactionScript
    {
        private readonly AuthenticationService _authenticationService;

        public GetMostActiveUsersTransactionScript(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public IList<ActiveUserDTO> GetMostActiveUsers(BlogContext context, string sessionToken, int maxNumberOfMostActiveUsers)
        {
            _authenticationService.MakeSureSessionTokenIsOk(context, sessionToken);

            var activeUsers =
                (from post in context.Posts
                 group post by new
                     {
                         post.UserId, 
                         post.User.UserName
                     }
                 into g
                 let postCount = g.Count()
                 orderby postCount descending
                 select new ActiveUserDTO
                     {
                         UserId = g.Key.UserId,
                         UserName = g.Key.UserName,
                         NumberOfPosts = postCount
                     }).Take(maxNumberOfMostActiveUsers).ToList();

            return activeUsers;
        }
    }
}