using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using GitBlogEngine.Interface;
using GitBlogEngine.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace GitBlogEngine.Services
{
    public class BlogService : IBlogService
    {
        private AppSettings _settings { get; set; }
        private readonly string _username;
        private readonly string _password;
        public BlogService(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
            _username = _settings.GitHubUsername;
            _password = _settings.GitHubPassword;
        }

        /* The following method uses the github api to get the contents of a repository.
         * This returns a blob with a lost of the folders inside the repo.
        */
        public List<BlogContents> GetListOfBlogPosts()
        {
            RestClient _client = new RestClient()
            {
                BaseUrl = new System.Uri(_settings.GitBaseUrl),
                Authenticator = new HttpBasicAuthenticator(_username, _password)
            };

            RestRequest request = new RestRequest(method: Method.GET);
            request.Resource = $"{_settings.GitBaseUrl}repos/{_settings.GitHubUsername}/{_settings.GitRepoName}/contents";
            List<BlogContents> post = JsonConvert.DeserializeObject<List<BlogContents>>(_client.Execute(request).Content);
            return post;
        }

        public BlogPost GetBlogPost(string post)
        {
            RestClient _client = new RestClient()
            {
                BaseUrl = new System.Uri(_settings.GitBaseUrl),
                Authenticator = new HttpBasicAuthenticator(_username, _password)
            };
            List<BlogContents> blogContent = GetBlogContents(post);
            BlogPost blog = new BlogPost();
            foreach (var blob in blogContent)
            {
                var file = blob.name.Split('.')[1];
                switch (file)
                {
                    case "md":
                        blog.HTML = GetHTMLFromBlob(blob.sha);
                        break;
                }
            }
            return blog;
        }

        /*This methods retrieves the content from inside the selected folder. */
        private List<BlogContents> GetBlogContents(string name)
        {
            RestClient _client = new RestClient()
            {
                BaseUrl = new System.Uri(_settings.GitBaseUrl),
                Authenticator = new HttpBasicAuthenticator(_username, _password)
            };

            RestRequest request = new RestRequest(method: Method.GET);
            request.Resource = $"{_settings.GitBaseUrl}repos/{_settings.GitHubUsername}/{_settings.GitRepoName}/contents/{name}?ref=master";
            return JsonConvert.DeserializeObject<List<BlogContents>>(_client.Execute(request).Content);
        }

        private string GetHTMLFromBlob(string sha)
        {
            // Console.WriteLine(sha);
            RestClient _client = new RestClient()
            {
                BaseUrl = new System.Uri(_settings.GitBaseUrl),
                Authenticator = new HttpBasicAuthenticator(_username, _password)
            };
            RestRequest request = new RestRequest();
            request.Resource = $"{_settings.GitBaseUrl}repos/{_settings.GitHubUsername}/{_settings.GitRepoName}/git/blobs/{sha}";
            string base64 = JsonConvert.DeserializeObject<EncryptedContent>(_client.Execute(request).Content).content;
            string markdown = ConvertFromBase64(base64);
            return ConvertMDToHtml(markdown);
        }

        private string ConvertFromBase64(string base64)
        {
            //Console.WriteLine(base64);
            string temp = base64.Replace(System.Environment.NewLine, "");
            byte[] data = Convert.FromBase64String(temp);
            return Encoding.UTF8.GetString(data);
        }

        private string ConvertMDToHtml(string markdown)
        {
            RestClient _client = new RestClient()
            {
                BaseUrl = new System.Uri(_settings.GitBaseUrl),
                Authenticator = new HttpBasicAuthenticator(_username, _password)
            };
            RestRequest request = new RestRequest();
            request.Resource = $"{_settings.GitBaseUrl}markdown/raw";
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "text/plain");
            Console.WriteLine(markdown);
            request.AddParameter("text/plain", markdown, ParameterType.RequestBody);
            return _client.Execute(request).Content;
        }
    }

}