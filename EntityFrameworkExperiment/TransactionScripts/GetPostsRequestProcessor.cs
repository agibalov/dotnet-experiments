using System.Linq;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DTO;
using EntityFrameworkExperiment.Mappers;

namespace EntityFrameworkExperiment.TransactionScripts
{
    public class GetPostsRequestProcessor
    {
        private readonly AuthenticationService _authenticationService;
        private readonly PostToPostDTOMapper _postToPostDtoMapper;

        public GetPostsRequestProcessor(
            AuthenticationService authenticationService,
            PostToPostDTOMapper postToPostDtoMapper)
        {
            _authenticationService = authenticationService;
            _postToPostDtoMapper = postToPostDtoMapper;
        }

        public Page<PostDTO> GetPosts(BlogContext context, string sessionToken, int itemsPerPage, int page)
        {
            _authenticationService.GetUserBySessionToken(context, sessionToken);

            var postCount = context.Posts.Count();
            var posts = context.Posts.OrderBy(post => post.PostId).Skip(page * itemsPerPage).Take(itemsPerPage);

            return new Page<PostDTO>
                {
                    TotalItemCount = postCount,
                    Items = _postToPostDtoMapper.Map(posts)
                };
        }
    }
}