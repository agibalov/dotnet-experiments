using System.Collections.Generic;
using System.Linq;
using EntityFrameworkExperiment.DAL.Entities;
using EntityFrameworkExperiment.DTO;

namespace EntityFrameworkExperiment.Mappers
{
    public class PostToPostDTOMapper
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;

        public PostToPostDTOMapper(UserToUserDTOMapper userToUserDtoMapper)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
        }

        public PostDTO Map(Post post)
        {
            return new PostDTO
                {
                    PostId = post.PostId,
                    PostText = post.Text,
                    Author = _userToUserDtoMapper.Map(post.User)
                };
        }

        public IList<PostDTO> Map(IEnumerable<Post> posts)
        {
            return posts.Select(Map).ToList();
        }
    }
}