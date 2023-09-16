using Bogus;

namespace SelfDevelopmentProj.BogusSetup
{
    public static class FakeDataA
    {
        public static List<Post> posts = new();
        public static List<Blog> blogs = new();

        public static void Init(int countToGenerate)
        {
            var postId = 1;
            var postFaker = new Faker<Post>()
                .RuleFor(p => p.PostId, _ => postId++)
                .RuleFor(p => p.Title, f => f.Hacker.Phrase())
                .RuleFor(p => p.Content, f => f.Lorem.Sentence());

            var blogId = 1;
            var blogFaker = new Faker<Blog>()
                .RuleFor(b => b.BlogId, _ => blogId++)
                .RuleFor(b => b.Url, f => f.Internet.Url())
                .RuleFor(b => b.Posts, (f, b) =>
                {
                    postFaker.RuleFor(p => p.BlogId, _ => b.BlogId);
                    var posts = postFaker.GenerateBetween(3, 5);

                    FakeDataA.posts.AddRange(posts);
                    
                    return null;
                });

            var blogs = blogFaker.Generate(countToGenerate);

            FakeDataA.blogs.AddRange(blogs);
        }
    }
}
