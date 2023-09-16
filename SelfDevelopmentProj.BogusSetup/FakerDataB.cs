using Bogus;
using System.Diagnostics.Metrics;

namespace SelfDevelopmentProj.BogusSetup
{
    public static class FakerDataB
    {
        private static int blogId = 1;
        private static int postId = 1;
        private static List<Blog> blogs = new();
        private static List<Post> posts = new();

        private static Faker _faker;

        public static void Init(int counts)
        {
            _faker = new Faker();

            GenerateBlogs(counts);
        }


        private static void GenerateBlogs(int counts)
        {
            for (int i = 0; i < counts; i++, blogId++)
            {
                var blog = new Blog
                {
                    BlogId = blogId,
                    Url = _faker.Internet.Url()
                };

                var postsCount = _faker.Random.Number(3, 5);
                GeneratePosts(blog, postsCount);

                blogs.Add(blog);
            }
        }

        private static void GeneratePosts(Blog blog, int counts)
        {
            for (int i = 0; i < counts; i++, postId++)
            {
                var post = new Post
                {
                    PostId = postId,
                    Content = _faker.Lorem.Sentence(),
                    Title = _faker.Hacker.Phrase(),
                    BlogId = blog.BlogId,

                };

                posts.Add(post);
            }
        }
    }
}
