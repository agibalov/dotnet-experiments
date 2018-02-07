using System;
using System.Linq;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DTO;
using EntityFrameworkExperiment.Exceptions;
using EntityFrameworkExperiment.Mappers;

namespace EntityFrameworkExperiment.TransactionScripts
{
    public class UpdateCommentTransactionScript
    {
        private readonly AuthenticationService _authenticationService;
        private readonly CommentToCommentDTOMapper _commentToCommentDtoMapper;

        public UpdateCommentTransactionScript(
            AuthenticationService authenticationService,
            CommentToCommentDTOMapper commentToCommentDtoMapper)
        {
            _authenticationService = authenticationService;
            _commentToCommentDtoMapper = commentToCommentDtoMapper;
        }

        public CommentDTO UpdateComment(BlogContext context, string sessionToken, int commentId, string commentText)
        {
            var user = _authenticationService.GetUserBySessionToken(context, sessionToken);

            var comment = context.Comments.SingleOrDefault(c => c.CommentId == commentId);
            if (comment == null)
            {
                throw new NoSuchCommentException();
            }

            if (comment.UserId != user.UserId)
            {
                throw new NoPermissionsException();
            }

            comment.Text = commentText;
            comment.ModifiedAt = DateTime.UtcNow;

            context.SaveChanges();

            return _commentToCommentDtoMapper.Map(comment);
        }
    }
}