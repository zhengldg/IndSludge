using GFAPP.Model;
using GFAPP.Model.SMS.Dtos;

namespace GFAPP.Application.SMS
{
    public interface ISMSService
    {
        ApiResult Send(SMSSendInput input);
    }
}
