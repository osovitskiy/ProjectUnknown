using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated.Internal;

namespace ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated
{
    public class MultipartInputFormatter : InputFormatter
    {
        private struct FormatterInfo
        {
            public FormatterInfo(InputFormatter formatter)
                : this(formatter, formatter.SupportedMediaTypes.Select(x => MediaTypeHeaderValue.Parse(x)))
            {
            }

            public FormatterInfo(InputFormatter formatter, IEnumerable<MediaTypeHeaderValue> supportedMediaTypes)
            {
                Formatter = formatter;
                SupportedMediaTypes = supportedMediaTypes.ToList();
            }

            public InputFormatter Formatter;
            public IList<MediaTypeHeaderValue> SupportedMediaTypes;
        }

        private readonly IList<FormatterInfo> _formatters;

        public MultipartInputFormatter(IEnumerable<InputFormatter> formatters)
        {
            _formatters = formatters.Select(formatter => new FormatterInfo(formatter)).ToList();

            SupportedMediaTypes.Add(MediaTypeHeaderValues.MultipartRelated);
        }

        public override bool CanRead(InputFormatterContext context)
        {
            if (base.CanRead(context))
            {
                MediaTypeHeaderValue mediaType;
                MediaTypeHeaderValue.TryParse(context.HttpContext.Request.ContentType, out mediaType);

                if (mediaType == null)
                {
                    return false;
                }

                var type = mediaType.GetInnerType();

                if (type == null)
                {
                    return false;
                }

                MediaTypeHeaderValue innerType;
                MediaTypeHeaderValue.TryParse(type, out innerType);

                if (innerType == null)
                {
                    return false;
                }

                return _formatters.Any(formatter => formatter.SupportedMediaTypes.Any(fomatterType => innerType.IsSubsetOf(fomatterType)));
            }

            return false;
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var multipart = await context.HttpContext.Request.ReadMultipartAsync();
            var root = multipart.Root;

            if (root == null)
            {
                return InputFormatterResult.Failure();
            }

            var logger = context.HttpContext.RequestServices.GetService<ILogger<MultipartInputFormatter>>();

            using (var body = root.OpenReadStream())
            {
                var wrapper = new HttpContextWrapper(context.HttpContext, root, body);

                var formatterContext = new InputFormatterContext(
                    wrapper,
                    context.ModelName,
                    context.ModelState,
                    context.Metadata,
                    context.ReaderFactory);

                IInputFormatter formatter = null;

                for (var i = 0; i < _formatters.Count; i++)
                {
                    var info = _formatters[i];

                    if (info.Formatter.CanRead(formatterContext))
                    {
                        formatter = info.Formatter;
                        logger?.InputFormatterSelected(formatter, formatterContext);
                        break;
                    }
                    else
                    {
                        logger?.InputFormatterRejected(info.Formatter, formatterContext);
                    }
                }

                if (formatter == null)
                {
                    logger?.NoInputFormatterSelected(formatterContext);
                    return InputFormatterResult.Failure();
                }

                return await formatter.ReadAsync(formatterContext);
            }
        }
    }
}
