
namespace ApiProject.Data.CustomModels.ApiException
{
    public class ApiException
    {
        public ApiException(int code, string message,string detail){
            StatusCode = code;
            Message = message;
            StackTrace = detail;
        }

        public int StatusCode {get;set;}
        public string Message {get;set;}
        public string StackTrace {get;set;}
    }
}