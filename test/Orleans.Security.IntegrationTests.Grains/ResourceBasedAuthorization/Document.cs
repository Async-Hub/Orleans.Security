using System;

namespace Orleans.Security.IntegrationTests.Grains.ResourceBasedAuthorization
{
    [Serializable]
    public class Document
    {
        public string Name { get; set; }

        public string Author { get; set; }

        public string Content { get; set; }
    }
}