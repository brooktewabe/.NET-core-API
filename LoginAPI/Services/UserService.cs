using LoginAPI.Models.Email;
using LoginAPI.Models;
using LoginAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace LoginAPI.Services
{
    public class UserService : IUserService
    {
        private readonly UserDBContext _userDbContext;
        private readonly JwtService _jwtService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly EmailSender _emailSender;

        public UserService(UserDBContext userDbContext, JwtService jwtService, IPasswordHasher<User> passwordHasher, EmailSender emailSender)
        {
            _userDbContext = userDbContext;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _emailSender = emailSender;
        }

        public BaseResponse CreateUser(User user)
        {
            if (_userDbContext.Users.Where(u => u.Email == user.Email).FirstOrDefault() != null)
                return new BaseResponseService().GetErrorResponse(new Exception("User Email Already Exists"));

            user.Password = _passwordHasher.HashPassword(user, user.Password);
            _userDbContext.Add(user);
            _userDbContext.SaveChanges();
            return new BaseResponseService().GetSuccessResponse("User Successfully Registered");
        }

        public BaseResponse loginUser(Login credentials)
        {
            var userAvailable = _userDbContext.Users.Where(u => u.Email == credentials.Email).FirstOrDefault();

            if (userAvailable != null && _passwordHasher.VerifyHashedPassword(userAvailable, userAvailable.Password, credentials.Password) == PasswordVerificationResult.Success)
                return new BaseResponseService().GetSuccessResponse(_jwtService.GenerateToken(userAvailable.Email));
            else
                return new BaseResponseService().GetErrorResponse(new Exception("Invalid Credentials"));
        }

        public BaseResponse ForgetPassword(ForgetPassword forgetPassword)
        {
            var userAvailable = _userDbContext.Users.Where(u => u.Email == forgetPassword.Email).FirstOrDefault();
            if (userAvailable == null)
                return new BaseResponseService().GetErrorResponse(new BadHttpRequestException("Invalid Input"));

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var token = new string(Enumerable.Repeat(chars, 50).Select(s => s[RandomNumberGenerator.GetInt32(s.Length)]).ToArray());

            var URL = $"{forgetPassword.ClientURL}?email={forgetPassword.Email}&token={token}";

            userAvailable.PasswordResetToken = token;
            userAvailable.TokenGeneratedTime = DateTime.UtcNow;
            _userDbContext.Users.Update(userAvailable);
            _userDbContext.SaveChanges();

            var message = new Message(forgetPassword.Email, "Password Reset Request", new HtmlTemplate().GetHtmlBody(userAvailable.Name, URL));
            _emailSender.SendEmail(message);

            return new BaseResponseService().GetSuccessResponse("Password Reset Email Successfully Sent");
        }

        public BaseResponse ResetPassword(ResetPassword resetPassword)
        {
            var userAvailable = _userDbContext.Users.Where(u => u.Email == resetPassword.Email).FirstOrDefault();
            if (userAvailable == null)
                return new BaseResponseService().GetErrorResponse(new BadHttpRequestException("Invalid Request"));
          
            if (userAvailable.PasswordResetToken == null || !userAvailable.PasswordResetToken.Equals(resetPassword.Token) || DateTime.UtcNow.Subtract(userAvailable.TokenGeneratedTime.Value).TotalMinutes >= 60)
                return new BaseResponseService().GetErrorResponse(new BadHttpRequestException("Token Invalid or Expired"));

            userAvailable.Password = _passwordHasher.HashPassword(userAvailable, resetPassword.Password);
            userAvailable.PasswordResetToken = null;
            _userDbContext.Users.Update(userAvailable);
            _userDbContext.SaveChanges();

            return new BaseResponseService().GetSuccessResponse("Password Successfully Updated");
        }
    }
}
