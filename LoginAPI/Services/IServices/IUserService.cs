using LoginAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginAPI.Services.IServices
{
    public interface IUserService
    {
        public BaseResponse CreateUser(User user);
        public BaseResponse loginUser(Login credentials);
        public BaseResponse ForgetPassword(ForgetPassword forgetPassword);
        public BaseResponse ResetPassword(ResetPassword resetPassword);
    }
}
