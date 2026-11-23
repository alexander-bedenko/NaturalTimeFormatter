using System.Globalization;
using System.Resources;
using NaturalTimeFormatter.Interfaces;

namespace NaturalTimeFormatter
{
    public class LocalizedNaturalTimeFormatter : INaturalTimeFormatter
    {
        private readonly ResourceManager _resources;
        private readonly CultureInfo _culture;
        private readonly Func<int, string?> _pluralForm;

        public LocalizedNaturalTimeFormatter(CultureInfo culture)
        {
            _culture = culture;
            _resources = new ResourceManager("NaturalTimeFormatter.Resources.NaturalTimeFormatter", typeof(LocalizedNaturalTimeFormatter).Assembly);
            _pluralForm = GetPluralRule(culture);
        }

        private string _(string key) => _resources.GetString(key, _culture) ?? $"[{key}]";

        /// <summary>
        /// Picks the grammatically correct plural variant ("Key_One", "Key_Few"),
        /// falling back to the base key (the "many" form) when no variant exists.
        /// </summary>
        private string Plural(string key, int count)
        {
            var form = _pluralForm(count);
            var template = form is null ? null : _resources.GetString($"{key}_{form}", _culture);
            return string.Format(template ?? _(key), count);
        }

        private static Func<int, string?> GetPluralRule(CultureInfo culture)
        {
            var language = culture.Name.Split('-')[0].ToLowerInvariant();
            switch (language)
            {
                case "ru":
                case "be":
                case "uk":
                    return n =>
                    {
                        var mod10 = n % 10;
                        var mod100 = n % 100;
                        if (mod10 == 1 && mod100 != 11) return "One";
                        if (mod10 is >= 2 and <= 4 && mod100 is < 12 or > 14) return "Few";
                        return null;
                    };
                case "pl":
                    return n =>
                    {
                        if (n == 1) return "One";
                        var mod10 = n % 10;
                        var mod100 = n % 100;
                        if (mod10 is >= 2 and <= 4 && mod100 is < 12 or > 14) return "Few";
                        return null;
                    };
                default:
                    return _ => null;
            }
        }

        public string Format(DateTimeOffset inputTime, DateTimeOffset? referenceTime = null)
        {
            referenceTime ??= DateTimeOffset.Now;
            var delta = inputTime - referenceTime.Value;
            var isFuture = delta.TotalSeconds > 0;
            var absDelta = delta.Duration();

            if (absDelta.TotalSeconds < 60)
            {
                return _("JustNow");
            }

            if (absDelta.TotalMinutes < 60)
            {
                var minutes = (int)absDelta.TotalMinutes;
                if (minutes == 1)
                    return _(isFuture ? "InMinute" : "MinuteAgo");
                return Plural(isFuture ? "InMinutes" : "MinutesAgo", minutes);
            }

            if (absDelta.TotalHours < 24)
            {
                var hours = (int)absDelta.TotalHours;
                if (hours == 1)
                    return _(isFuture ? "InHour" : "HourAgo");
                return Plural(isFuture ? "InHours" : "HoursAgo", hours);
            }

            var inputDate = inputTime.Date;
            var referenceDate = referenceTime.Value.Date;
            var time = inputTime.ToString("HH:mm", _culture);

            if (inputDate == referenceDate)
                return string.Format(_("TodayAt"), time);

            if (inputDate == referenceDate.AddDays(-1))
                return string.Format(_("YesterdayAt"), time);

            if (inputDate == referenceDate.AddDays(-2))
                return string.Format(_("DayBeforeYesterdayAt"), time);

            if (inputDate == referenceDate.AddDays(1))
                return string.Format(_("TomorrowAt"), time);

            if (inputDate == referenceDate.AddDays(2))
                return string.Format(_("DayAfterTomorrowAt"), time);

            if (absDelta.TotalDays < 7)
            {
                var days = (int)absDelta.TotalDays;
                return Plural(isFuture ? "InDays" : "DaysAgo", days);
            }

            if (absDelta.TotalDays < 30)
            {
                var weeks = (int)(absDelta.TotalDays / 7);
                if (weeks == 1)
                    return _(isFuture ? "InWeek" : "WeekAgo");
                return Plural(isFuture ? "InWeeks" : "WeeksAgo", weeks);
            }

            if (absDelta.TotalDays < 365)
            {
                var months = (int)(absDelta.TotalDays / 30);
                if (months == 1)
                    return _(isFuture ? "InMonth" : "MonthAgo");
                return Plural(isFuture ? "InMonths" : "MonthsAgo", months);
            }

            var years = (int)(absDelta.TotalDays / 365);
            if (years == 1)
                return _(isFuture ? "InYear" : "YearAgo");
            return Plural(isFuture ? "InYears" : "YearsAgo", years);
        }
    }
}
