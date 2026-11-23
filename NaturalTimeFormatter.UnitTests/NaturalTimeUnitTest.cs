using System.Globalization;

namespace NaturalTimeFormatter.UnitTests
{
    public class NaturalTimeUnitTest
    {
        private static readonly DateTimeOffset Now = new(2025, 6, 15, 12, 0, 0, TimeSpan.Zero);

        private static string Format(string culture, TimeSpan offset) =>
            NaturalTime.CreateFormatter(new CultureInfo(culture)).Format(Now + offset, Now);

        [Fact]
        public void ShouldFormatMinutesAgo()
        {
            var formatter = NaturalTime.CreateFormatter(new CultureInfo("ru"));
            var input = DateTimeOffset.Now.AddMinutes(-10);
            var output = formatter.Format(input);
            Assert.Equal("10 минут назад", output);
        }

        [Theory]
        [InlineData(-30, "только что")]
        [InlineData(-60, "минуту назад")]
        [InlineData(60, "через минуту")]
        [InlineData(-2 * 60, "2 минуты назад")]
        [InlineData(-5 * 60, "5 минут назад")]
        [InlineData(-11 * 60, "11 минут назад")]
        [InlineData(-21 * 60, "21 минуту назад")]
        [InlineData(22 * 60, "через 22 минуты")]
        [InlineData(-3600, "час назад")]
        [InlineData(-2 * 3600, "2 часа назад")]
        [InlineData(-5 * 3600, "5 часов назад")]
        [InlineData(-21 * 3600, "21 час назад")]
        [InlineData(3 * 3600, "через 3 часа")]
        public void ShouldUseRussianPluralForms(int seconds, string expected)
        {
            Assert.Equal(expected, Format("ru", TimeSpan.FromSeconds(seconds)));
        }

        [Theory]
        [InlineData(-1, "вчера в 12:00")]
        [InlineData(-2, "позавчера в 12:00")]
        [InlineData(1, "завтра в 12:00")]
        [InlineData(2, "послезавтра в 12:00")]
        [InlineData(-3, "3 дня назад")]
        [InlineData(5, "через 5 дней")]
        [InlineData(-7, "неделю назад")]
        [InlineData(14, "через 2 недели")]
        [InlineData(-35, "месяц назад")]
        [InlineData(-65, "2 месяца назад")]
        [InlineData(-200, "6 месяцев назад")]
        [InlineData(-400, "год назад")]
        [InlineData(-800, "2 года назад")]
        [InlineData(5 * 365 + 10, "через 5 лет")]
        [InlineData(-21 * 365 - 10, "21 год назад")]
        public void ShouldFormatRussianDays(int days, string expected)
        {
            Assert.Equal(expected, Format("ru", TimeSpan.FromDays(days)));
        }

        [Theory]
        [InlineData("en", -1, "a minute ago")]
        [InlineData("en", -2 * 24 * 60, "the day before yesterday at 12:00")]
        [InlineData("en", -7 * 24 * 60, "a week ago")]
        [InlineData("en", 21 * 24 * 60, "in 3 weeks")]
        [InlineData("de", -2 * 24 * 60, "vorgestern um 12:00")]
        [InlineData("pl", -2 * 60, "2 godziny temu")]
        [InlineData("pl", -22, "22 minuty temu")]
        [InlineData("pl", -25, "25 minut temu")]
        [InlineData("pl", -14 * 24 * 60, "2 tygodnie temu")]
        [InlineData("be", -2 * 60, "2 гадзіны таму")]
        [InlineData("be", -21 * 24 * 60, "3 тыдні таму")]
        [InlineData("be-BY", -21, "21 хвіліну таму")]
        [InlineData("nl", -5, "5 minuten geleden")]
        [InlineData("zh", -5, "5 分钟前")]
        [InlineData("zh-CN", -2 * 24 * 60, "前天 12:00")]
        [InlineData("ja", -5, "5分前")]
        [InlineData("ko", -5, "5분 전")]
        [InlineData("ru-RU", -2 * 60, "2 часа назад")]
        [InlineData("uk", -1, "хвилину тому")]
        [InlineData("uk", -3, "3 хвилини тому")]
        [InlineData("uk", -21 * 60, "21 годину тому")]
        [InlineData("uk", 5 * 24 * 60, "через 5 днів")]
        [InlineData("uk-UA", -2 * 24 * 60, "позавчора о 12:00")]
        [InlineData("pt", -5, "há 5 minutos")]
        [InlineData("pt-BR", 14 * 24 * 60, "em 2 semanas")]
        [InlineData("tr", -5, "5 dakika önce")]
        [InlineData("tr", -1 * 24 * 60, "dün 12:00")]
        [InlineData("cs", -3, "před 3 minutami")]
        [InlineData("cs", 3, "za 3 minuty")]
        [InlineData("cs", 5, "za 5 minut")]
        [InlineData("cs", 14 * 24 * 60, "za 2 týdny")]
        [InlineData("sv", -5, "för 5 minuter sedan")]
        [InlineData("sv", 2 * 24 * 60, "i övermorgon kl. 12:00")]
        [InlineData("fi", -5, "5 minuuttia sitten")]
        [InlineData("fi", 3 * 60, "3 tunnin päästä")]
        [InlineData("hu", -5, "5 minutes ago")]
        [InlineData("", -5, "5 minutes ago")]
        public void ShouldFormatOtherLanguages(string culture, int minutes, string expected)
        {
            Assert.Equal(expected, Format(culture, TimeSpan.FromMinutes(minutes)));
        }
    }
}
