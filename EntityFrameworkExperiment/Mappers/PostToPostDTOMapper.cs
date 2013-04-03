using System;
using EntityFrameworkExperiment.DAL.Entities;
using EntityFrameworkExperiment.DTO;

namespace EntityFrameworkExperiment.Mappers
{
    public class PostToPostDTOMapper
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;
        private readonly CommentToCommentDTOMapper _commentToCommentDtoMapper;

        public PostToPostDTOMapper(
            UserToUserDTOMapper userToUserDtoMapper,
            CommentToCommentDTOMapper commentToCommentDtoMapper)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
            _commentToCommentDtoMapper = commentToCommentDtoMapper;
        }

        public PostDTO Map(Post post)
        {
            return new PostDTO
                {
                    PostId = post.PostId,
                    PostText = post.Text,
                    CreatedAt = post.CreatedAt,
                    ModifiedAt = post.ModifiedAt,
                    Author = _userToUserDtoMapper.Map(post.User),
                    Comments = _commentToCommentDtoMapper.Map(post.Comments)
                };
        }
    }
}