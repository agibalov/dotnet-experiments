using System.Linq;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DTO;
using EntityFrameworkExperiment.Exceptions;
using EntityFrameworkExperiment.Mappers;

namespace EntityFrameworkExperiment.TransactionScripts
{
    public class GetUserDetailsTransactionScript
    {
        private readonly AuthenticationService _authenticationService;
        private readonly PostToPostDTOMapper _postToPostDtoMapper;

        public GetUserDetailsTransactionScript(
            AuthenticationService authenticationService,
            PostToPostDTOMapper postToPostDtoMapper)
        {
            _authenticationService = authenticationService;
            _postToPostDtoMapper = postToPostDtoMapper;
        }

        public UserDetailsDTO GetUserDetails(BlogContext context, string sessionToken, int userId, int maxNumberOfRecentPosts)
        {
            _authenticationService.GetUserBySessionToken(context, sessionToken);

            var user = context.Users.SingleOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                throw new NoSuchUserException();
            }

            var numberOfPosts = context.Posts.Count(post => post.UserId == userId);
            var recentPosts = context.Posts
                .Where(post => post.UserId == userId)
                .OrderByDescending(post => post.CreatedAt)
                .Take(maxNumberOfRecentPosts)
                .ToList();

            return new UserDetailsDTO
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    NumberOfPosts = numberOfPosts,
                    RegisteredAt = user.CreatedAt,
                    RecentPosts = _postToPostDtoMapper.Map(recentPosts)
                };
        }
    }
}