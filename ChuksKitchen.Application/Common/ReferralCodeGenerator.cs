namespace ChuksKitchen.Application.Common;

public static class ReferralCodeGenerator
{
    private static readonly Random _random = new Random();

    public static string Generate(int length = 8)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Range(0, length)
                                    .Select(_ => chars[_random.Next(chars.Length)])
                                    .ToArray());
    }
}
