using System;
using System.IO;

namespace Ardalis.GuardClauses;

public static class GuardExtension
{
    public static void InvalidPath(this IGuardClause guardClause, string path, string parameterName)
    {
        if (!File.Exists(path))
            throw new ArgumentException("File does not exist", parameterName);
    }
}