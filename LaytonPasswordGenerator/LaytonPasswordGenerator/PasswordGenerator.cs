using System.Text;
using Kryptography.Checksum.Crc;
using Kryptography.Contract.Enums.Checksum.Crc;

namespace LaytonPasswordGenerator;

static class PasswordGenerator
{
    public static string Generate(byte[] macAddress, PasswordType type)
    {
        switch (type)
        {
            case PasswordType.CuriousVillage:
                return GenerateFirstTrilogy(macAddress, 0x4E59);

            case PasswordType.DiabolicalBoxLeft:
                return GenerateFirstTrilogy(macAddress, 0x4E4B);

            case PasswordType.DiabolicalBoxRight:
                return GenerateFirstTrilogy(macAddress, 0x4B55);

            case PasswordType.UnwoundFuture:
                return GenerateFirstTrilogy(macAddress, 0x6F79);

            case PasswordType.SpectresCall:
                return GenerateSecondTrilogy(macAddress, "LT5TOLT4");

            case PasswordType.MiracleMaskLeft:
                return GenerateSecondTrilogy(macAddress, "LT4TOLT5");

            case PasswordType.MiracleMaskRight:
                return GenerateSecondTrilogy(macAddress, "LT6TOLT5");

            default:
                throw new InvalidOperationException("Invalid password type.");
        }
    }

    private static string GenerateFirstTrilogy(byte[] macAddress, int seed)
    {
        byte m0 = macAddress[0];
        byte m1 = macAddress[1];
        byte m2 = macAddress[2];
        byte m3 = macAddress[3];
        byte m4 = macAddress[4];
        byte m5 = macAddress[5];

        var scrambled = new byte[8];

        scrambled[0] = (byte)((m0 + m3) * seed);
        scrambled[1] = (byte)((m1 + m3 - m4 + seed));
        scrambled[2] = (byte)(((m2 + m3 + m4 + m5) * seed));
        scrambled[3] = (byte)((m3 - seed));
        scrambled[4] = (byte)((m4 * seed));
        scrambled[5] = (byte)((m5 + seed));
        scrambled[6] = (byte)(((m0 + m3 + m1 + m3 - m4 + m2 + m3 + m4 + m5) * seed));
        scrambled[7] = (byte)((m3 - m4 - m5 - seed));

        var result = "";
        result += (char)('0' + scrambled[0] % 9);
        result += (char)('A' + scrambled[1] % 7);
        result += (char)('0' + scrambled[2] % 9);
        result += (char)('A' + scrambled[3] % 7);
        result += (char)('0' + scrambled[4] % 9);
        result += (char)('A' + scrambled[5] % 7);
        result += (char)('0' + scrambled[6] % 9);
        result += (char)('A' + scrambled[7] % 7);

        return result;
    }

    private static string GenerateSecondTrilogy(byte[] macAddress, string seed)
    {
        const string initializationValue = "8B9SXDM4E7QUFH2CJL6NPZKA3RWT5VYG";

        var hash = 0xFFFFFFFFu;

        var crc32 = Crc32.Create(Crc32Formula.Reflected, Crc32.DefaultReflectedPolynomial);

        crc32.ComputeBlock(Encoding.ASCII.GetBytes(seed), ref hash);
        crc32.ComputeBlock(macAddress, ref hash);

        hash = ~hash;

        var password = "";

        for (var i = 0; i < 8; i++)
            password += initializationValue[(int)(i * 0xb + (hash >> ((i & 0x3f) << 2) & 0xf) & 0x1f)];

        return password;
    }
}