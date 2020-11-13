using System.Linq;

public class CheckRule
{
    const int _maxIDLen = 14;
    const int _minIDLen = 6;

    const int _maxPwLen = 10;
    const int _minPwLen = 6;

    const int _maxNickNameLen = 15;
    const int _minNickNameLen = 1;

    static bool IsLetter(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    }

    static bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    static bool IsSymbol(char c)
    {
        return c > 32 && c < 127 && !IsDigit(c) && !IsLetter(c);
    }

    public static bool IsValidID(string id)
    {
        return
            id.Length >= _minIDLen &&
            id.Length <= _maxIDLen &&
            id.Any(c => IsLetter(c)) &&
            id.Any(c => IsDigit(c)) &&
            !id.Any(c => IsSymbol(c));
    }

    public static bool IsValidPassword(string password)
    {
        return
            password.Length >= _minPwLen &&
            password.Length <= _maxPwLen &&
            password.Any(c => IsLetter(c)) &&
            password.Any(c => IsDigit(c)) &&
            password.Any(c => IsSymbol(c));
    }

    public static bool IsValidNickName(string nickname)
    {
        return
            nickname.Length >= _minNickNameLen &&
            nickname.Length <= _maxNickNameLen &&
            nickname.Any(c => IsLetter(c)) ||
            nickname.Any(c => IsDigit(c)) &&
            !nickname.Any(c => IsSymbol(c));
    }
}
