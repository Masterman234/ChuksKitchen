using System.Text.RegularExpressions;

namespace ChuksKitchen.Application.Common;

public static class PhoneValidator
{
    private static readonly Regex NigerianPhoneRegex =
        new(@"^(?:\+234|234|0)[789][01]\d{8}$",
            RegexOptions.Compiled);

    public static bool IsValidNigerianPhone(string phone) =>
        NigerianPhoneRegex.IsMatch(phone);
}