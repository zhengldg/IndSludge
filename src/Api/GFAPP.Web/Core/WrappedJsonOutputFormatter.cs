using GFAPP.Model;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System.Buffers;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GFAPP.Web.Core
{
    public class WrappedJsonOutputFormatter : JsonOutputFormatter
    {
        public WrappedJsonOutputFormatter(JsonSerializerSettings serializerSettings, ArrayPool<char> charPool)
            : base(serializerSettings, charPool)
        { }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.OK)
            {
                var @object = new ApiResult(context.Object) ;

                var newContext = new OutputFormatterWriteContext(context.HttpContext, context.WriterFactory, typeof(ApiResult), @object);
                newContext.ContentType = context.ContentType;
                newContext.ContentTypeIsServerDefined = context.ContentTypeIsServerDefined;

                return base.WriteResponseBodyAsync(newContext, selectedEncoding);
            }

            return base.WriteResponseBodyAsync(context, selectedEncoding);
        }
    }
}
