using System;
using System.Security.Cryptography;

namespace ProjectUnknown.AspNetCore.Authentication.BasicAuthentication.Extensions
{
    public static class HashAlgorithmExtensions
    {
        public static void AddToDigest(this HashAlgorithm algorithm, byte[] buffer)
        {
            AddToDigest(algorithm, buffer, 0, buffer.Length);
        }

        public static void AddToDigest(this HashAlgorithm algorithm, byte[] buffer, int offset, int count)
        {
            algorithm.TransformBlock(buffer, offset, count, null, 0);
        }

        public static void FinishDigest(this HashAlgorithm algorithm)
        {
            algorithm.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
        }

    }
}
