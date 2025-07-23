using System.Globalization;

namespace NaturalTimeFormatter.UnitTests
{
    public class NaturalTimeUnitTest
    {
        [Fact]
        public void ShouldFormatMinutesAgo()
        {
            var formatter = NaturalTime.CreateFormatter(new CultureInfo("ru"));
            var input = DateTimeOffset.Now.AddMinutes(-10);
            var output = formatter.Format(input);
            Assert.Equal("10 минут назад", output);
        }
    }
}