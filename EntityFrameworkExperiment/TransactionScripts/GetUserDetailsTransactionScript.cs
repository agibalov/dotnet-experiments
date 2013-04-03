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
        private readonly PostToBriefPostDTOMapper _postToBriefPostDtoMapper;
        private readonly CommentToCommentDTOMapper _commentToCommentDtoMapper;

        public GetUserDetailsTransactionScript(
            AuthenticationService authenticationService,
            PostToBriefPostDTOMapper postToBriefPostDtoMapper,
            CommentToCommentDTOMapper commentToCommentDtoMapper)
        {
            _authenticationService = authenticationService;
            _postToBriefPostDtoMapper = postToBriefPostDtoMapper;
            _commentToCommentDtoMapper = commentToCommentDtoMapper;
        }

        public UserDetailsDTO GetUserDetails(
            BlogContext context, 
            string sessionToken, 
            int userId, 
            int maxNumberOfRecentPosts, 
            int maxNumberOfRecentComments)
        {
            _authenticationService.MakeSureSessionTokenIsOk(context, sessionToken);

            var user = context.Users.SingleOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                throw new NoSuchUserException();
            }

            var numberOfPosts = context.Posts.Count(post => post.UserId == userId);
            var recentPosts = context.Posts
                .Include("Comments")
                .Where(post => post.UserId == userId)
                .OrderByDescending(post => post.CreatedAt)
                .Take(maxNumberOfRecentPosts)
                .ToList();

            var numberOfComments = context.Comments.Count(comment => comment.UserId == userId);
            var recentComments = context.Comments
                .Where(comment => comment.UserId == userId)
                .OrderByDescending(comment => comment.CreatedAt)
                .Take(maxNumberOfRecentComments)
                .ToList();

            return new UserDetailsDTO
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    RegisteredAt = user.CreatedAt,
                    NumberOfPosts = numberOfPosts,
                    RecentPosts = _postToBriefPostDtoMapper.Map(recentPosts),
                    NumberOfComments = numberOfComments,
                    RecentComments = _commentToCommentDtoMapper.Map(recentComments)
                };
        }
    }
}