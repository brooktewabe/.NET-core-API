using LoginAPI.EnumTypes;
using LoginAPI.Models;

namespace LoginAPI.Services
{
    public class BaseResponseService
    {
        public BaseResponse GetSuccessResponse(object data) => new BaseResponse() { Status = (byte)Status.Success, Token = data, Message = "Process Succeded"};
        public BaseResponse GetSuccessResponse(object data, string message) => new BaseResponse() { Status = (byte)Status.Success, Token = data, Message = message };
        public BaseResponse GetErrorResponse(Exception ex) => new BaseResponse() { Status = (byte)Status.Fail, Token = ex, Message = ex.Message};
    }
}
