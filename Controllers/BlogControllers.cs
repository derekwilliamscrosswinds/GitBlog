using System.Collections.Generic;
using GitBlogEngine.Models;
using GitBlogEngine.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GitBlogEngine.Controllers {
    [Route("api/blog")]
    public class RepositoryController : Controller {
        private BlogService gd;

        public RepositoryController(IOptions<AppSettings> options){
            gd = new BlogService(options);
        }

        [HttpGet("posts")]
        public List<BlogContents> GetRepos() {
            return gd.GetListOfBlogPosts();
        }
        [HttpGet("posts/{name}")]
        public BlogPost GetBlogPost(string name) {
            return gd.GetBlogPost(name);
        }

        


    }
}