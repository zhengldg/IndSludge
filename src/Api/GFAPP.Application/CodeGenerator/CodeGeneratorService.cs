using GFAPP.EntityFramework;
using GFAPP.Model.CodeGenerator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GFAPP.Application.CodeGenerator
{
    public class CodeGeneratorService : ICodeGeneratorService
    {
        private readonly ApplicationDbContext context;

        public CodeGeneratorService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<string> Generate(CodeGeneratorType type, string code)
        {
            var entity = await context.CodeGenerators.FirstOrDefaultAsync(x => x.Code == code && x.Type == type);
            if (entity == null)
            {
                throw new InvalidOperationException("code值未注册");
            }
            if (string.IsNullOrEmpty(entity.Format))
            {
                throw new InvalidOperationException("Format值不能为空");
            }
            var tempValue = entity.Format;
            if (entity.Format.Contains("#CODE#"))
            {
                tempValue = tempValue.Replace("#CODE#", code);
            }
            if (entity.Format.Contains("#YEAR#"))
            {
                tempValue = tempValue.Replace("#YEAR#", DateTime.Now.Year.ToString());
            }
            if (entity.Format.Contains("#MONTH#"))
            {
                tempValue = tempValue.Replace("#MONTH#", DateTime.Now.Month.ToString().PadLeft(2,'0'));
            }
            if (entity.Format.Contains("#DAY#"))
            {
                tempValue = tempValue.Replace("#DAY#", DateTime.Now.Day.ToString().PadLeft(2, '0'));
            }
            if (entity.Format.Contains("#NUMBER#"))
            {
                tempValue = tempValue.Replace("#NUMBER#", (++entity.CurrentValue).ToString().PadLeft(entity.NumberLength, '0'));
            }

            context.CodeGenerators.Update(entity);
            await context.SaveChangesAsync();

            return tempValue;
        }
    }
}
