using System.Text;
using FluentAssertions;
using Xunit;

namespace ProjectUnknown.BaseEncoding.Tests
{
    public class BaseEncodingTests
    {
        [Fact]
        public void Smoke()
        {
            var buffer = Encoding.UTF8.GetBytes("Test string 123!@#%");
            var hash = "VGVzdCBzdHJpbmcgMTIzIUAjJQ==";
            Base64Encoding.Default.Encode(buffer).Should().BeEquivalentTo(hash);
            Base64Encoding.Default.Decode(hash).Should().BeEquivalentTo(buffer);
        }
    }
}
