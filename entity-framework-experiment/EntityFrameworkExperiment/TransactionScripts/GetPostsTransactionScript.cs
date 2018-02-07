using System.Linq;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DTO;
using EntityFrameworkExperiment.Mappers;

namespace EntityFrameworkExperiment.TransactionScripts
{
    public class GetPostsTransactionScript
    {
        private readonly AuthenticationService _authenticationService;
        private readonly PostToBriefPostDTOMapper _postToBriefPostDtoMapper;

        public GetPostsTransactionScript(
            AuthenticationService authenticationService,
            PostToBriefPostDTOMapper postToBriefPostDtoMapper)
        {
            _authenticationService = authenticationService;
            _postToBriefPostDtoMapper = postToBriefPostDtoMapper;
        }

        public Page<BriefPostDTO> GetPosts(BlogContext context, string sessionToken, int itemsPerPage, int page)
        {
            _authenticationService.MakeSureSessionTokenIsOk(context, sessionToken);

            var postCount = context.Posts.Count();
            var posts = context.Posts
                .Include("User")
                .Include("Comments")
                .OrderBy(post => post.PostId)
                .Skip(page * itemsPerPage)
                .Take(itemsPerPage);

            return new Page<BriefPostDTO>
                {
                    TotalItemCount = postCount,
                    Items = _postToBriefPostDtoMapper.Map(posts)
                };
        }
    }
}