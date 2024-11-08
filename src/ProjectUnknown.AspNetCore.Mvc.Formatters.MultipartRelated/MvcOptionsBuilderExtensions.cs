using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using ProjectUnknown.Common;

namespace ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated
{
    public static class MvcOptionsBuilderExtensions
    {
        public static void AddMultipartRelatedInputFormatter(this MvcOptions options)
        {
            Ensure.IsNotNull(options, nameof(options));

            options.InputFormatters.RemoveType<MultipartInputFormatter>();

            var formatters = options.InputFormatters.Where(x => x is InputFormatter).Cast<InputFormatter>().ToList();
            var formatter = new MultipartInputFormatter(formatters);

            options.InputFormatters.Add(formatter);
        }
    }
}
