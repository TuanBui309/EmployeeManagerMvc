using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Entity.Constants
{
    public class ResponseEntity : IActionResult
    {
        public int StatusCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        public object? Content { get; set; }

        public DateTime DateTime { get; set; }

        public ResponseEntity(int statusCode, object? content=null , string message = "")
        {
            StatusCode = statusCode;
            Content = content;
            Message = message ;
            DateTime = DateTime.Now;
        }

        public async System.Threading.Tasks.Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCode;
            await new ObjectResult(this).ExecuteResultAsync(context);
        }

        
    }
}
