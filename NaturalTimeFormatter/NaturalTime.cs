using System.Globalization;
using NaturalTimeFormatter.Interfaces;

namespace NaturalTimeFormatter
{
    public static class NaturalTime
    {
        public static INaturalTimeFormatter CreateFormatter(CultureInfo culture)
        {
            return new LocalizedNaturalTimeFormatter(culture);
        }
    }
}
