namespace NaturalTimeFormatter.Interfaces
{
    /// <summary>
    /// Interface for formatting DateTimeOffset values into a natural language representation.
    /// </summary>
    public interface INaturalTimeFormatter
    {
        string Format(DateTimeOffset inputTime, DateTimeOffset? referenceTime = null);
    }
}
