using System;
using System.Linq;
using EntityFrameworkExperiment.DAL;
using EntityFrameworkExperiment.DAL.Entities;
using EntityFrameworkExperiment.DTO;
using EntityFrameworkExperiment.Exceptions;
using EntityFrameworkExperiment.Mappers;

namespace EntityFrameworkExperiment.TransactionScripts
{
    public class CreateCommentTransactionScript
    {
        private readonly AuthenticationService _authenticationService;
        private readonly CommentToCommentDTOMapper _commentToCommentDtoMapper;

        public CreateCommentTransactionScript(
            AuthenticationService authenticationService,
            CommentToCommentDTOMapper commentToCommentDtoMapper)
        {
            _authenticationService = authenticationService;
            _commentToCommentDtoMapper = commentToCommentDtoMapper;
        }

        public CommentDTO CreateComment(BlogContext context, string sessionToken, int postId, string commentText)
        {
            var user = _authenticationService.GetUserBySessionToken(context, sessionToken);

            var post = context.Posts.SingleOrDefault(p => p.PostId == postId);
            if (post == null)
            {
                throw new NoSuchPostException();
            }

            var comment = new Comment
                {
                    Text = commentText,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = null,
                    Post = post,
                    User = user
                };

            context.Comments.Add(comment);
            context.SaveChanges();

            return _commentToCommentDtoMapper.Map(comment);
        }
    }
}