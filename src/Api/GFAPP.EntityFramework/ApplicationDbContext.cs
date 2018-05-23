using GFAPP.Model.Authentication.Jwt;
using GFAPP.Model.Authentication.Users;
using GFAPP.Model.Company;
using GFAPP.Model.IndSludge;
using GFAPP.Model.Record;
using GFAPP.Model.Signature;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GFAPP.EntityFramework
{
    public class ApplicationDbContext : IdentityDbContext<UserInfo>
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<CompanyInfo> Companys { get; set; }
        public DbSet<RegisterCode> RegisterCodes { get; set; }
        public DbSet<EleDuplicateInfo> EleDuplicates { get; set; }
        public DbSet<RecordInfo> Records { get; set; }
        public DbSet<SignatureInfo> Signatures { get; set; }


        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<EleDuplicateInfo>().Property(x => x.CreatedTime).ValueGeneratedOnAdd();
            builder.Entity<EleDuplicateInfo>().Property(x => x.ModifiedTime).ValueGeneratedOnAddOrUpdate();
            //builder.Entity<EleDuplicateInfo>()
            //    .HasMany(x => x.RecordList)
            //    .WithOne(x => x.EleDuplicate);

            builder.Entity<UserInfo>().Property(x => x.CompanyInfoId).IsRequired();

        }
    }
}
