using DapperExperiment.MultipleTablesTests.DAL;
using NUnit.Framework;

namespace DapperExperiment.MultipleTablesTests
{
    public class PostTests : AbstractBlogServiceTest
    {
        [Test]
        public void CanCreatePost()
        {
            var user = BlogService.CreateUser("loki2302");
            var post = BlogService.CreatePost(user.UserId, "test post");
            Assert.AreNotEqual(0, post.PostId);
            Assert.AreEqual("test post", post.PostText);
            AssertEqualUsers(user, post.User);
        }

        [Test]
        public void CanUpdatePost()
        {
            var user = BlogService.CreateUser("loki2302");
            var post = BlogService.CreatePost(user.UserId, "test post");
            Assert.AreEqual("test post", post.PostText);

            var updatedPost = BlogService.UpdatePost(post.PostId, "new text");
            Assert.AreEqual(post.PostId, updatedPost.PostId);
            Assert.AreEqual("new text", updatedPost.PostText);
            AssertEqualUsers(post.User, updatedPost.User);
        }

        [Test]
        [ExpectedException(typeof(NoSuchPostException))]
        public void CantUpdatePostThatDoesNotExist()
        {
            BlogService.UpdatePost(123, "new post text");
        }

        [Test]
        public void CanGetPost()
        {
            var user = BlogService.CreateUser("loki2302");
            var createdPost = BlogService.CreatePost(user.UserId, "test post");
            var fetchedPost = BlogService.GetPost(createdPost.PostId);
            AssertEqualPosts(createdPost, fetchedPost);
        }

        [Test]
        [ExpectedException(typeof(NoSuchPostException))]
        public void CantGetPostThatDoesNotExist()
        {
            BlogService.GetPost(123);
        }

        [Test]
        public void CanGetMultiplePosts()
        {
            var user = BlogService.CreateUser("loki2302");
            var post1 = BlogService.CreatePost(user.UserId, "test post #1");
            BlogService.CreatePost(user.UserId, "test post #2");
            var post3 = BlogService.CreatePost(user.UserId, "test post #3");

            var posts = BlogService.GetPosts(new[] {post1.PostId, post3.PostId});
            AssertEqualPosts(post1, posts[0]);
            AssertEqualPosts(post3, posts[1]);
        }

        [Test]
        [ExpectedException(typeof(NoSuchPostException))]
        public void CantGetPostsIfAtLeastOneDoesNotExist()
        {
            var user = BlogService.CreateUser("loki2302");

            var post1 = BlogService.CreatePost(user.UserId, "test post #1");
            Assert.AreNotEqual(123, post1.PostId);

            var post2 = BlogService.CreatePost(user.UserId, "test post #1");
            Assert.AreNotEqual(123, post2.PostId);

            BlogService.GetPosts(new[] { post1.PostId, post2.PostId, 123 });
        }

        [Test]
        public void CanGetAllPosts()
        {
            var user = BlogService.CreateUser("loki2302");
            var post1 = BlogService.CreatePost(user.UserId, "test post #1");
            var post2 = BlogService.CreatePost(user.UserId, "test post #2");
            var post3 = BlogService.CreatePost(user.UserId, "test post #3");

            var posts = BlogService.GetAllPosts();
            AssertEqualPosts(post1, posts[0]);
            AssertEqualPosts(post2, posts[1]);
            AssertEqualPosts(post3, posts[2]);
        }

        [Test]
        public void CanGetPostCount()
        {
            var postCount = BlogService.GetPostCount();
            Assert.AreEqual(0, postCount);
        }

        [Test]
        public void CanDeletePost()
        {
            var user = BlogService.CreateUser("loki2302");
            var post1 = BlogService.CreatePost(user.UserId, "test post #1");
            var post2 = BlogService.CreatePost(user.UserId, "test post #2");

            var postCountBeforeDelete = BlogService.GetPostCount();
            Assert.AreEqual(2, postCountBeforeDelete);

            BlogService.DeletePost(post1.PostId);

            var postCountAfterDelete = BlogService.GetPostCount();
            Assert.AreEqual(1, postCountAfterDelete);

            var remainingPost = BlogService.GetPost(post2.PostId);
            AssertEqualPosts(post2, remainingPost);
        }

        [Test]
        [ExpectedException(typeof(NoSuchPostException))]
        public void CantDeletePostThatDoesNotExist()
        {
            BlogService.DeletePost(123);
        }

        [Test]
        public void CanDeleteMultiplePosts()
        {
            var user = BlogService.CreateUser("loki2302");
            var post1 = BlogService.CreatePost(user.UserId, "test post #1");
            var post2 = BlogService.CreatePost(user.UserId, "test post #2");
            var post3 = BlogService.CreatePost(user.UserId, "test post #3");

            var postCountBeforeDelete = BlogService.GetPostCount();
            Assert.AreEqual(3, postCountBeforeDelete);

            BlogService.DeletePosts(new[] { post1.PostId, post3.PostId });

            var postCountAfterDelete = BlogService.GetPostCount();
            Assert.AreEqual(1, postCountAfterDelete);

            var remainingPost = BlogService.GetPost(post2.PostId);
            AssertEqualPosts(post2, remainingPost);
        }

        [Test]
        [ExpectedException(typeof(NoSuchPostException))]
        public void CantDeleteMultiplePostsIfAtLeastOneDoesNotExist()
        {
            var user = BlogService.CreateUser("loki2302");

            var post1 = BlogService.CreatePost(user.UserId, "test post #1");
            Assert.AreNotEqual(123, post1.PostId);

            var post2 = BlogService.CreatePost(user.UserId, "test post #2");
            Assert.AreNotEqual(123, post2.PostId);

            BlogService.DeletePosts(new[] { post1.PostId, post2.PostId, 123 });
        }
    }
}