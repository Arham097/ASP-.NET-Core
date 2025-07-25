using System.Text;
using System;
namespace Model_Validation.Results
{
    public class HtmlResult : IResult
    {
        private readonly string html;

        public HtmlResult(string html)
        {
            this.html = html;

        }
        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            context.Response.ContentLength = Encoding.UTF8.GetByteCount(html);

            await context.Response.WriteAsync(html);
        }
    }
}