using Microsoft.Extensions.Configuration;

namespace ConfigSync.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var source = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string> { { "Key", "Value1" } })
            .Build();
            var target = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { { "Key", "Value2" } })
                .Build();

            var sync = new ConfigSynchronizer(source) { _targetConfig = target };
            var diffs = sync.GetDifferences();

            Assert.Contains("Key", diffs.Keys);
            Assert.Equal("From Value2 to Value1", diffs["Key"]);
        }
    }
}