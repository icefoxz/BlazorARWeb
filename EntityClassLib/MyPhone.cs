namespace EntityClassLib;

public static class MyPhone
{
    public static int MinLength { get; set; } = 10;
    public static int MaxLength { get; set; } = 12;

    public static bool VerifyMobilePhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber))
        {
            return false;
        }

        var numberWithout60 = ResolveNumberWithout60(phoneNumber);

        // Check if the phone number has the correct length and starts with the prefix "01"
        return IsInLength(numberWithout60) && numberWithout60.StartsWith("01");
    }

    private static bool IsInLength(string numberWithout60) => (numberWithout60.Length >= MinLength && numberWithout60.Length <= MaxLength);

    private static string ResolveNumberWithout60(string phoneNumber)
    {
        var digitsOnly = DigitOnly(phoneNumber);

        // Handle the "+60" prefix for Malaysian phone numbers
        if (digitsOnly.StartsWith("60") && (digitsOnly.Length == MinLength + 1 || digitsOnly.Length == MaxLength + 1))
        {
            digitsOnly = "0" + digitsOnly[2..];
        }

        return digitsOnly;

    }

    private static string DigitOnly(string phoneNumber)
    {
        // Remove any non-digit characters
        return new string(phoneNumber.Where(char.IsDigit).ToArray());
    }

    public static bool MyPhoneNumber(string phoneNumber)
    {
        var numberWithout60 = ResolveNumberWithout60(phoneNumber);
        return IsInLength(numberWithout60) && numberWithout60.StartsWith("0");
    }

    public static string PhoneNumberWithMyPrefix(string phoneNumber)
    {
        var numberWithout60 = ResolveNumberWithout60(phoneNumber);
        return $"6{numberWithout60}";
    }
    /// <summary>
    /// eg. 60123456789
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <param name="withCountryCode"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string NormalizePhoneNumber(string phoneNumber,bool withCountryCode = true)
    {
        if (!VerifyMobilePhoneNumber(phoneNumber))
        {
            throw new ArgumentException("Invalid Malaysian phone number format.");
        }
        // Remove any non-digit characters
        var numberWithout60 = ResolveNumberWithout60(phoneNumber);
        return withCountryCode ? $"6{numberWithout60}" : numberWithout60;
    }

    public static string GenerateCode(int digit)
    {
        if (digit < 1)
        {
            throw new ArgumentException("The number of digits must be at least 1.");
        }

        var random = new Random();
        var code = string.Empty;

        for (var i = 0; i < digit; i++)
        {
            code += random.Next(0, 10);
        }

        return code;
    }
}