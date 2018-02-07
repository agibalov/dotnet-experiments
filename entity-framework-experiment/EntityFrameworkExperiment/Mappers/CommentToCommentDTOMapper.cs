using System.Collections.Generic;
using System.Linq;
using EntityFrameworkExperiment.DAL.Entities;
using EntityFrameworkExperiment.DTO;

namespace EntityFrameworkExperiment.Mappers
{
    public class CommentToCommentDTOMapper
    {
        private readonly UserToUserDTOMapper _userToUserDtoMapper;

        public CommentToCommentDTOMapper(UserToUserDTOMapper userToUserDtoMapper)
        {
            _userToUserDtoMapper = userToUserDtoMapper;
        }

        public CommentDTO Map(Comment comment)
        {
            return new CommentDTO
                {
                    CommentId = comment.CommentId,
                    CommentText = comment.Text,
                    CreatedAt = comment.CreatedAt,
                    ModifiedAt = comment.ModifiedAt,
                    Author = _userToUserDtoMapper.Map(comment.User)
                };
        }

        public IList<CommentDTO> Map(IList<Comment> comments)
        {
            return comments.Select(Map).ToList();
        }
    }
}