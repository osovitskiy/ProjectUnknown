﻿using System.IO;
using Microsoft.AspNetCore.WebUtilities;

namespace ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated
{
    public class MultipartOptions
    {
        public const int DefaultPartsCountLimit = 1024;
        public const int DefaultMemoryBufferThreshold = 1024 * 64;
        public const int DefaultBufferBodyLengthLimit = 1024 * 1024 * 128;
        public const int DefaultMultipartBoundaryLengthLimit = 128;
        public const long DefaultMultipartBodyLengthLimit = 1024 * 1024 * 128;

        /// <summary>
        /// Enables full request body buffering. Use this if multiple components need to read the raw stream.
        /// The default value is false.
        /// </summary>
        public bool BufferBody { get; set; } = false;

        /// <summary>
        /// If <see cref="BufferBody"/> is enabled, this many bytes of the body will be buffered in memory.
        /// If this threshold is exceeded then the buffer will be moved to a temp file on disk instead.
        /// This also applies when buffering individual multipart section bodies.
        /// </summary>
        public int MemoryBufferThreshold { get; set; } = DefaultMemoryBufferThreshold;

        /// <summary>
        /// If <see cref="BufferBody"/> is enabled, this is the limit for the total number of bytes that will
        /// be buffered. Forms that exceed this limit will throw an <see cref="InvalidDataException"/> when parsed.
        /// </summary>
        public long BufferBodyLengthLimit { get; set; } = DefaultBufferBodyLengthLimit;

        /// <summary>
        /// A limit for the number of request parts to allow.
        /// Requests that exceed this limit will throw an <see cref="InvalidDataException"/> when parsed.
        /// </summary>
        public int PartsCountLimit { get; set; } = DefaultPartsCountLimit;

        /// <summary>
        /// A limit for the length of the boundary identifier. Forms with boundaries that exceed this
        /// limit will throw an <see cref="InvalidDataException"/> when parsed.
        /// </summary>
        public int MultipartBoundaryLengthLimit { get; set; } = DefaultMultipartBoundaryLengthLimit;

        /// <summary>
        /// A limit for the number of headers to allow in each multipart section. Headers with the same name will
        /// be combined. Form sections that exceed this limit will throw an <see cref="InvalidDataException"/>
        /// when parsed.
        /// </summary>
        public int MultipartHeadersCountLimit { get; set; } = MultipartReader.DefaultHeadersCountLimit;

        /// <summary>
        /// A limit for the total length of the header keys and values in each multipart section.
        /// Form sections that exceed this limit will throw an <see cref="InvalidDataException"/> when parsed.
        /// </summary>
        public int MultipartHeadersLengthLimit { get; set; } = MultipartReader.DefaultHeadersLengthLimit;

        /// <summary>
        /// A limit for the length of each multipart body. Forms sections that exceed this limit will throw an
        /// <see cref="InvalidDataException"/> when parsed.
        /// </summary>
        public long MultipartBodyLengthLimit { get; set; } = DefaultMultipartBodyLengthLimit;
    }
}
