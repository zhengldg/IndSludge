using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFAPP.Web.Core
{
    public class ZhcnIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError()
        {
            return new IdentityError()
            {
                Code = "DefaultError",
                Description = "服务器内部发生错误"
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError()
            {
                Code = "PasswordMismatch",
                Description = "密码无效"
            };
        }

        public override IdentityError InvalidToken()
        {
            return new IdentityError()
            {
                Code = "InvalidToken",
                Description = "您的token无效或者已过期"
            };
        }


        public override IdentityError InvalidUserName(string userName)
        {
            IdentityError identityError = new IdentityError();
            identityError.Code = "InvalidUserName";
            string str = $"用户名无效";
            identityError.Description = str;
            return identityError;
        }

        public override IdentityError InvalidEmail(string email)
        {
            IdentityError identityError = new IdentityError();
            identityError.Code = "InvalidEmail";
            string str = $"邮箱无效";
            identityError.Description = str;
            return identityError;
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            IdentityError identityError = new IdentityError();
            identityError.Code = "DuplicateUserName";
            string str = $"用户名重复";
            identityError.Description = str;
            return identityError;
        }

        public override IdentityError DuplicateEmail(string email)
        {
            IdentityError identityError = new IdentityError();
            identityError.Code = "DuplicateEmail";
            string str = $"邮箱重复";
            identityError.Description = str;
            return identityError;
        }

        public override IdentityError InvalidRoleName(string role)
        {
            IdentityError identityError = new IdentityError();
            identityError.Code = "InvalidRoleName";
            string str = $"无效的角色名称";
            identityError.Description = str;
            return identityError;
        }

        public override IdentityError DuplicateRoleName(string role)
        {
            IdentityError identityError = new IdentityError();
            identityError.Code = "DuplicateRoleName";
            string str = $"角色名称重复";
            identityError.Description = str;
            return identityError;
        }

        public virtual IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError()
            {
                Code = "UserAlreadyHasPassword",
                Description = "用户已经设置密码"
            };
        }


        public override IdentityError UserAlreadyInRole(string role)
        {
            IdentityError identityError = new IdentityError();
            identityError.Code = "UserAlreadyInRole";
            string str = $"用户已经属于该角色 {role}";
            identityError.Description = str;
            return identityError;
        }

        public override IdentityError UserNotInRole(string role)
        {
            IdentityError identityError = new IdentityError();
            identityError.Code = "UserNotInRole";
            string str = $"用户不属于该角色： {role}";
            identityError.Description = str;
            return identityError;
        }

        public override IdentityError PasswordTooShort(int length)
        {
            IdentityError identityError = new IdentityError();
            identityError.Code = "PasswordTooShort";
            string str = $"密码太短";
            identityError.Description = str;
            return identityError;
        }
    }
}
