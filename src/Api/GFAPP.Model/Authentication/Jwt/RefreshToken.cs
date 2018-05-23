using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFAPP.Model.Authentication.Jwt
{
    [Table("APP.RefreshToken")]
    public class RefreshToken
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }

        [StringLength(50)]
        public string ClientId { get; set; }

        public string Token { get; set; }

        public string UserName { get; set; }
    }
}
