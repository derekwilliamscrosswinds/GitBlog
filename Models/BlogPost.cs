namespace GitBlogEngine.Models
{
    public class BlogPost
    {
        public string HTML { get; set; }
        public string imageUri { get; set; }
    }

    public class EncryptedContent
    {
        public string sha { get; set; }
        public string node_id { get; set; }
        public int size { get; set; }
        public string url { get; set; }
        public string content { get; set; }
        public string encoding { get; set; }
    }
}