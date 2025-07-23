using System.Globalization;
using System.Resources;
using NaturalTimeFormatter.Interfaces;

namespace NaturalTimeFormatter
{
    public class LocalizedNaturalTimeFormatter : INaturalTimeFormatter
    {
        private readonly ResourceManager _resources;
        private readonly CultureInfo _culture;

        public LocalizedNaturalTimeFormatter(CultureInfo culture)
        {
            _culture = culture;
            _resources = new ResourceManager("NaturalTimeFormatter.Resources.NaturalTimeFormatter", typeof(LocalizedNaturalTimeFormatter).Assembly);
        }

        private string _(string key) => _resources.GetString(key, _culture) ?? $"[{key}]";

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
                return string.Format(
                    _(isFuture ? "InMinutes" : "MinutesAgo"), minutes);
            }

            if (absDelta.TotalHours < 24)
            {
                var hours = (int)absDelta.TotalHours;
                if (hours == 1)
                    return _(isFuture ? "InHour" : "HourAgo");
                else if (hours < 5)
                    return string.Format(_(isFuture ? "InHours" : "HoursAgo"), hours);
                else
                    return string.Format(_(isFuture ? "InManyHours" : "ManyHoursAgo"), hours);
            }

            var inputDate = inputTime.Date;
            var referenceDate = referenceTime.Value.Date;

            if (inputDate == referenceDate)
            {
                return string.Format(_("TodayAt"), inputTime.ToString("HH:mm", _culture));
            }

            if (inputDate == referenceDate.AddDays(-1))
            {
                return string.Format(_("YesterdayAt"), inputTime.ToString("HH:mm", _culture));
            }

            if (inputDate == referenceDate.AddDays(1))
            {
                return string.Format(_("TomorrowAt"), inputTime.ToString("HH:mm", _culture));
            }

            if (inputDate == referenceDate.AddDays(2))
            {
                return string.Format(_("DayAfterTomorrowAt"), inputTime.ToString("HH:mm", _culture));
            }

            if (absDelta.TotalDays < 30)
            {
                var days = (int)absDelta.TotalDays;
                return string.Format(_(isFuture ? "InDays" : "DaysAgo"), days);
            }

            if (absDelta.TotalDays < 365)
            {
                var months = (int)(absDelta.TotalDays / 30);
                if (months == 1)
                    return _(isFuture ? "InMonth" : "MonthAgo");
                return string.Format(_(isFuture ? "InMonths" : "MonthsAgo"), months);
            }

            var years = (int)(absDelta.TotalDays / 365);
            if (years == 1)
                return _(isFuture ? "InYear" : "YearAgo");
            return string.Format(_(isFuture ? "InYears" : "YearsAgo"), years);
        }
    }
}