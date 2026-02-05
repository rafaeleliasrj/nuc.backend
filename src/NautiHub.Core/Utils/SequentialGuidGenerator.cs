using System.Security.Cryptography;

namespace NautiHub.Core.Utils;

public static class SequentialGuidGenerator
{
    public static Guid NewSequentialGuid(SequentialGuidType guidType = SequentialGuidType.SequentialAtEnd)
    {
        var randomBytes = new byte[10];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        long timestamp = DateTime.UtcNow.Ticks;
        byte[] timestampBytes = BitConverter.GetBytes(timestamp);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(timestampBytes);

        var guidBytes = new byte[16];

        switch (guidType)
        {
            case SequentialGuidType.SequentialAsString:
            case SequentialGuidType.SequentialAsBinary:
                // Timestamp no início
                Array.Copy(timestampBytes, 2, guidBytes, 0, 6);
                Array.Copy(randomBytes, 0, guidBytes, 6, 10);
                break;

            case SequentialGuidType.SequentialAtEnd:
                // Timestamp no final
                Array.Copy(randomBytes, 0, guidBytes, 0, 10);
                Array.Copy(timestampBytes, 2, guidBytes, 10, 6);
                break;
        }

        return new Guid(guidBytes);
    }
}

public enum SequentialGuidType
{
    SequentialAsString,  // Para PostgreSQL, MySQL
    SequentialAsBinary,  // Para Oracle
    SequentialAtEnd      // Para SQL Server
}
