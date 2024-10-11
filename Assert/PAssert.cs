using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PAssert;

public static class PAssert
{
    public static void Ensure(
            [DoesNotReturnIf(false)] bool condition,
            [CallerArgumentExpression(nameof(condition))] string? message = null
        )
    {
        if (!condition)
        {
            Fail(message);
        }
    }

    [DoesNotReturn]
    public static void Fail(string? message)
    {
        string stackTrace;
        try
        {
            stackTrace = new StackTrace(0, true).ToString();
        }
        catch
        {
            stackTrace = "";
        }

        var ex = new AssertException(message, stackTrace);
        Environment.FailFast(ex.Message, ex);
    }

    private sealed class AssertException : Exception
    {
        internal AssertException(string? message, string? stackTrace)
            : base(Terminate(message) + stackTrace) { }

        private static string? Terminate(string? s)
        {
            if (s == null)
            {
                return s;
            }

            s = s.Trim();
            if (s.Length > 0)
            {
                s += Environment.NewLine;
            }

            return s;
        }
    }
}