using System.Linq;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DTO;
using EntityFrameworkExperiment.Exceptions;
using EntityFrameworkExperiment.Mappers;

namespace EntityFrameworkExperiment.TransactionScripts
{
    public class GetPostTransactionScript
    {
        private readonly AuthenticationService _authenticationService;
        private readonly PostToPostDTOMapper _postToPostDtoMapper;

        public GetPostTransactionScript(
            AuthenticationService authenticationService,
            PostToPostDTOMapper postToPostDtoMapper)
        {
            _authenticationService = authenticationService;
            _postToPostDtoMapper = postToPostDtoMapper;
        }

        public PostDTO GetPost(BlogContext context, string sessionToken, int postId)
        {
            _authenticationService.MakeSureSessionTokenIsOk(context, sessionToken);

            var post = context.Posts.SingleOrDefault(p => p.PostId == postId);
            if (post == null)
            {
                throw new NoSuchPostException();
            }

            return _postToPostDtoMapper.Map(post);
        }
    }
}