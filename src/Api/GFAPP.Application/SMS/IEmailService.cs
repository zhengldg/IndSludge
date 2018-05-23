using GFAPP.Model;
using GFAPP.Model.SMS.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace GFAPP.Application.SMS
{
    public interface IEmailService
    {
        ApiResult Send(EmailInput input);
    }
}
