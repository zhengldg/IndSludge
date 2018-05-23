using GFAPP.Model;
using GFAPP.Model.Account.Dtos;

namespace GFAPP.Application.Account
{
    public interface IAccountService
    {
        ApiResult UploadAvatar(UploadAvatarInput input);
    }
}
