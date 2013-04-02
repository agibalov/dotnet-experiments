using System.Collections.Generic;
using System.Linq;
using EntityFrameworkExperiment.DAL.Entities;
using EntityFrameworkExperiment.DTO;

namespace EntityFrameworkExperiment.Mappers
{
    public class PostToBriefPostDTOMapper
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;

        public PostToBriefPostDTOMapper(UserToUserDTOMapper userToUserDtoMapper)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
        }

        public BriefPostDTO Map(Post post)
        {
            return new BriefPostDTO
                {
                    PostId = post.PostId,
                    PostText = post.Text,
                    CreatedAt = post.CreatedAt,
                    ModifiedAt = post.ModifiedAt,
                    Author = _userToUserDtoMapper.Map(post.User),
                    CommentCount = post.Comments.Count()
                };
        }

        public IList<BriefPostDTO> Map(IEnumerable<Post> posts)
        {
            return posts.Select(Map).ToList();
        }
    }
}