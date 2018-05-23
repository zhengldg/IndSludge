using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace GFAPP.Model.Account.Dtos
{
    /// <summary>
    /// 上传头像
    /// </summary>
    public class UploadAvatarInput
    {
        /// <summary>
        /// 头像文件
        /// </summary>
        public IFormFile avatarFile { get; set; }

        /// <summary>
        /// 图床保存路径
        /// </summary>
        public string folder { get; set; }
    }
}
