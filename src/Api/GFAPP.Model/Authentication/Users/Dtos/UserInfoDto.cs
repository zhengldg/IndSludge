using GFAPP.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GFAPP.Model.Authentication.Users.Dtos
{
    public class UserInfoDto
    {
        public DateTime? Birthday { get; set; }

        public Sex Sex { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
