using GFAPP.Model.CodeGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GFAPP.Application.CodeGenerator
{
    public interface ICodeGeneratorService
    {
         Task<string> Generate(CodeGeneratorType type, string code);
    }
}
