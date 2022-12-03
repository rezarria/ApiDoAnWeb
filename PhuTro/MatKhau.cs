using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Api.PhuTro;

public static class MatKhau
{
    public static byte[] MaHoaMatKhau(string matKhau)
    {
        using var rng = RandomNumberGenerator.Create();
        return MaHoaMatKhau(matKhau, rng);
    }

    public static byte[] MaHoaMatKhau(string matKhau,
        RandomNumberGenerator rng,
        KeyDerivationPrf prf = KeyDerivationPrf.HMACSHA256,
        int saltSize = 128 / 8,
        int iterationCount = 1000,
        int keySize = 256 / 8)
    {
        var salt = new byte[saltSize];

        rng.GetBytes(salt);
        var key = KeyDerivation.Pbkdf2(matKhau, salt, prf, iterationCount, keySize);

        var output = new byte[13 + salt.Length + key.Length];
        output[0] = 0x01;

        GhiByte(output, 1, (uint)prf);
        GhiByte(output, 5, (uint)iterationCount);
        GhiByte(output, 9, (uint)saltSize);

        Buffer.BlockCopy(salt, 0, output, 13, salt.Length);
        Buffer.BlockCopy(key, 0, output, 13 + salt.Length, key.Length);
        return output;
    }

    public static bool XacThucMatKhau(byte[] matKhauMaHoa, string matKhau)
    {
        try
        {
            var prf = (KeyDerivationPrf)DocByte(matKhauMaHoa, 1);
            var iterationCount = (int)DocByte(matKhauMaHoa, 5);
            var saltSize = (int)DocByte(matKhauMaHoa, 9);
            var keySize = matKhauMaHoa.Length - 13 - saltSize;

            var salt = new byte[saltSize];
            var key = new byte[keySize];

            Buffer.BlockCopy(matKhauMaHoa, 13, salt, 0, salt.Length);
            Buffer.BlockCopy(matKhauMaHoa, 13 + salt.Length, key, 0, key.Length);

            var key2 = KeyDerivation.Pbkdf2(matKhau, salt, prf, iterationCount, keySize);

            return key.SequenceEqual(key2);
        }
        catch (Exception)
        {
            return false;
        }
    }


    private static void GhiByte(byte[] mang, int viTri, uint giaTri)
    {
        mang[viTri] = (byte)(giaTri >> 24);
        mang[viTri + 1] = (byte)(giaTri >> 16);
        mang[viTri + 2] = (byte)(giaTri >> 8);
        mang[viTri + 3] = (byte)giaTri;
    }

    private static uint DocByte(byte[] mang, int viTri)
    {
        return (uint)(mang[viTri] << 24)
               | (uint)(mang[viTri + 1] << 16)
               | (uint)(mang[viTri + 2] << 8)
               | mang[viTri + 3];
    }
}