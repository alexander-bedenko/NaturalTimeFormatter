# NaturalTimeFormatter

**NaturalTimeFormatter** is a lightweight and extensible .NET library for displaying human-friendly time expressions like:

- `5 minutes ago`
- `in 2 days`
- `yesterday at 14:20`
- `the day before yesterday at 09:15`
- `2 weeks ago` / `in a minute`

Fully localized and suitable for use in:
- 🟢 Telegram bots
- 🟢 Web APIs
- 🟢 Blazor
- 🟢 WPF / WinForms
- 🟢 ASP.NET

---

## 🌟 Features

- ✅ Easy-to-read natural time formatting
- 🔢 Correct plural forms for Slavic languages (ru, uk, be, pl, cs) (`1 минуту`, `2 минуты`, `5 минут`)
- 🌍 Localization support (currently: `ru`, `en`, `de`, `pl`, `fr`, `es`, `it`, `nl`, `be`, `uk`, `cs`, `pt`, `sv`, `fi`, `tr`, `ko`, `ja`, `zh`; unknown cultures fall back to English)
- 🧩 Extensible formatter interface
- 💡 Suitable for both frontend and backend projects
- 📦 Lightweight — no external dependencies

---

## 🔧 Example Usage

```csharp
var formatter = NaturalTime.CreateFormatter(culture);

var fiveMinutesAgo = DateTimeOffset.Now.AddMinutes(-5);
Console.WriteLine(formatter.Format(fiveMinutesAgo)); // "5 минут назад" / "5 minutes ago"

var tomorrow = DateTimeOffset.Now.AddDays(1).AddHours(3);
Console.WriteLine(formatter.Format(tomorrow)); // "завтра в 03:00" / "tomorrow at 03:00"
