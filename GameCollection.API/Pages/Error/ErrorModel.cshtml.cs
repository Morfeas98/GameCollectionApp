using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace GameCollection.API.Pages.Error
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? OriginalPath { get; set; }

        public void OnGet(int? statusCode = null, string? message = null)
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            StatusCode = statusCode ?? HttpContext.Response.StatusCode;
            ErrorMessage = message;
            OriginalPath = HttpContext.Features.Get<Microsoft.AspNetCore.Http.Features.IHttpRequestFeature>()?.RawTarget;
        }

        public void OnPost()
        {
            OnGet();
        }
    }
}
