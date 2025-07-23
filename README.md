# NaturalTimeFormatter

**NaturalTimeFormatter** is a lightweight and extensible .NET library for displaying human-friendly time expressions like:

- `5 minutes ago`
- `in 2 days`
- `yesterday at 14:20`

Fully localized and suitable for use in:
- 🟢 Telegram bots
- 🟢 Web APIs
- 🟢 Blazor
- 🟢 WPF / WinForms
- 🟢 ASP.NET

---

## 🌟 Features

- ✅ Easy-to-read natural time formatting
- 🌍 Localization support (currently: `ru`, `en`, `de`, `pl`, `fr`, `es`, `it`)
- 🧩 Extensible formatter interface
- 💡 Suitable for both frontend and backend projects
- 📦 Lightweight — no external dependencies

---

## 🔧 Example Usage

```csharp
var formatter = NaturalTime.CreateFormatter("ru");

var fiveMinutesAgo = DateTimeOffset.Now.AddMinutes(-5);
Console.WriteLine(formatter.Format(fiveMinutesAgo)); // "5 минут назад"

var tomorrow = DateTimeOffset.Now.AddDays(1).AddHours(3);
Console.WriteLine(formatter.Format(tomorrow)); // "завтра в 03:00"
