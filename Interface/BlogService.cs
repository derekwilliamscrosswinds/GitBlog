using System.Collections.Generic;
using GitBlogEngine.Models;

namespace GitBlogEngine.Interface {
    interface IBlogService {
        List<BlogContents> GetListOfBlogPosts();

        BlogPost GetBlogPost(string post);
    }
}