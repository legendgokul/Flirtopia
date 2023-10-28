using System.Text.Json;
using ApiProject.Helpers;

namespace ApiProject.Extensions
{
    /// <summary>
    /// Extention class which extends HttpResponse and adds new method calls AddPagination.
    /// </summary>
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header)
        {
            var jsonOptions = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            response.Headers.Add("Pagination",JsonSerializer.Serialize(header,jsonOptions));
            /* inform the client (typically a web browser) that it's allowed to access the "Pagination" header in the response. 
             * This step is crucial in the context of Cross-Origin Resource Sharing (CORS) to avoid CORS issues. */
            response.Headers.Add("Access-Control-Expose-Headers","Pagination");
        }
    }
}