using System.Diagnostics.CodeAnalysis;
using LaytonPasswordGenerator;

if (!TryReadMacAddress(out byte[]? mac))
    return;

//return [0x00, 0x09, 0xBF, 0x12, 0x34, 0x56];

Console.WriteLine($"Curious Village:      {PasswordGenerator.Generate(mac, PasswordType.CuriousVillage)}");
Console.WriteLine($"Diabolical Box Left:  {PasswordGenerator.Generate(mac, PasswordType.DiabolicalBoxLeft)}");
Console.WriteLine($"Diabolical Box Right: {PasswordGenerator.Generate(mac, PasswordType.DiabolicalBoxRight)}");
Console.WriteLine($"Unwound Future:       {PasswordGenerator.Generate(mac, PasswordType.UnwoundFuture)}");
Console.WriteLine($"Spectre's Call:       {PasswordGenerator.Generate(mac, PasswordType.SpectresCall)}");
Console.WriteLine($"Miracle Mask Left:    {PasswordGenerator.Generate(mac, PasswordType.MiracleMaskLeft)}");
Console.WriteLine($"Miracle Mask Right:   {PasswordGenerator.Generate(mac, PasswordType.MiracleMaskRight)}");

return;

static bool TryReadMacAddress([NotNullWhen(true)] out byte[]? mac)
{
    mac = null;

    Console.Write("Enter your NDS or 3DS MAC address (ex. E0-09-BF-12-B4-9D): ");
    string? input = Console.ReadLine();

    if (string.IsNullOrEmpty(input))
    {
        Console.WriteLine("No MAC address entered.");
        return false;
    }

    if (input.Length < 12)
    {
        Console.WriteLine("Entered MAC address is too short.");
        return false;
    }

    var macBuffer = new byte[6];
    var macPosition = 0;

    foreach (char character in input)
    {
        int value;
        switch (character)
        {
            case >= '0' and <= '9':
                value = character - '0';
                break;

            case >= 'A' and <= 'F':
                value = character - '7';
                break;

            case >= 'a' and <= 'f':
                value = character - 'W';
                break;

            default:
                continue;
        }

        int bufferIndex = macPosition / 2;
        if (bufferIndex >= 6)
        {
            macPosition++;
            continue;
        }

        int shiftValue = (macPosition++ & 1) is 0 ? 4 : 0;
        macBuffer[bufferIndex] |= (byte)(value << shiftValue);
    }

    switch (macPosition)
    {
        case < 12:
            Console.WriteLine("Entered MAC address is too short.");
            return false;

        case > 12:
            Console.WriteLine("Entered MAC address is too long.");
            return false;

        default:
            mac = macBuffer;
            return true;
    }
}