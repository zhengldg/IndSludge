using GFAPP.Model;
using GFAPP.Model.Account.Dtos;
using GFAPP.Model.Configuration;
using Microsoft.Extensions.Options;
using Qiniu.Http;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.Util;
using System.IO;

namespace GFAPP.Application.Account
{
    public class AccountService: IAccountService
    {
        private QiniuConfig qiniuconfig;

        public AccountService(IOptions<QiniuConfig> qiniuconfig)
        {
            this.qiniuconfig = qiniuconfig.Value;
        }

        public ApiResult UploadAvatar(UploadAvatarInput input)
        {
            ApiResult result = new ApiResult(true);

            var file = input.avatarFile;
            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独提供了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Qiniu.Common.Config.SetZone(Qiniu.Common.ZoneID.CN_South, false);
            Mac mac = new Mac(qiniuconfig.APP_KEY, qiniuconfig.SECRET_KEY);

            string bucket = "gougouapp";
            string saveKey = file.Name;

            // 上传策略，参见 
            // http://developer.qiniu.com/article/developer/security/put-policy.html
            PutPolicy putPolicy = new PutPolicy();

            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = bucket;

            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);

            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            putPolicy.DeleteAfterDays = 1;

            // 生成上传凭证，参见
            // http://developer.qiniu.com/article/developer/security/upload-token.html            

            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);

            FormUploader fu = new FormUploader();

            // 支持自定义参数
            //var extra = new System.Collections.Generic.Dictionary<string, string>();
            //extra.Add("FileType", "UploadFromLocal");
            //extra.Add("YourKey", "YourValue");
            //UploadFile(...,extra)
            if (file.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    var buffer = stream.ToArray();
                    HttpResult uploadResult = fu.UploadData(buffer, saveKey, token);
                    result.success = uploadResult.Code == 200;
                }
            }
            else
            {
                result.success = false;
                result.message = "请选择需要上传的文件";
            }

            return result;
        }
    }
}
