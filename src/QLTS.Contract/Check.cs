using System.Diagnostics;

namespace QLTS.Contract;

public sealed class Check
{
    private static bool useAssertions;

    public static bool UseAssertions
    {
        get
        {
            return useAssertions;
        }
        set
        {
            useAssertions = value;
        }
    }

    private static bool UseExceptions => !useAssertions;

    public static void Require(bool assertion, string message)
    {
        if (UseExceptions)
        {
            if (!assertion)
            {
                throw new Exception(message);
            }
        }
        else
        {
            Trace.Assert(assertion, "Precondition: " + message);
        }
    }

    public static void Require(bool assertion, string message, Exception inner)
    {
        if (UseExceptions)
        {
            if (!assertion)
            {
                throw new Exception(message, inner);
            }
        }
        else
        {
            Trace.Assert(assertion, "Precondition: " + message);
        }
    }

    public static void Require(bool assertion)
    {
        if (UseExceptions)
        {
            if (!assertion)
            {
                throw new Exception("Precondition failed.");
            }
        }
        else
        {
            Trace.Assert(assertion, "Precondition failed.");
        }
    }

    public static void Ensure(bool assertion, string message)
    {
        if (UseExceptions)
        {
            if (!assertion)
            {
                throw new Exception(message);
            }
        }
        else
        {
            Trace.Assert(assertion, "Postcondition: " + message);
        }
    }

    public static void Ensure(bool assertion, string message, Exception inner)
    {
        if (UseExceptions)
        {
            if (!assertion)
            {
                throw new Exception(message, inner);
            }
        }
        else
        {
            Trace.Assert(assertion, "Postcondition: " + message);
        }
    }

    public static void Ensure(bool assertion)
    {
        if (UseExceptions)
        {
            if (!assertion)
            {
                throw new Exception("Postcondition failed.");
            }
        }
        else
        {
            Trace.Assert(assertion, "Postcondition failed.");
        }
    }

    public static void Invariant(bool assertion, string message)
    {
        if (UseExceptions)
        {
            if (!assertion)
            {
                throw new Exception(message);
            }
        }
        else
        {
            Trace.Assert(assertion, "Invariant: " + message);
        }
    }

    public static void Invariant(bool assertion, string message, Exception inner)
    {
        if (UseExceptions)
        {
            if (!assertion)
            {
                throw new Exception(message, inner);
            }
        }
        else
        {
            Trace.Assert(assertion, "Invariant: " + message);
        }
    }

    public static void Invariant(bool assertion)
    {
        if (UseExceptions)
        {
            if (!assertion)
            {
                throw new Exception("Invariant failed.");
            }
        }
        else
        {
            Trace.Assert(assertion, "Invariant failed.");
        }
    }

    public static void Assert(bool assertion, string message)
    {
        if (UseExceptions)
        {
            if (!assertion)
            {
                throw new Exception(message);
            }
        }
        else
        {
            Trace.Assert(assertion, "Assertion: " + message);
        }
    }

    public static void Assert(bool assertion, string message, Exception inner)
    {
        if (UseExceptions)
        {
            if (!assertion)
            {
                throw new Exception(message, inner);
            }
        }
        else
        {
            Trace.Assert(assertion, "Assertion: " + message);
        }
    }

    public static void Assert(bool assertion)
    {
        if (UseExceptions)
        {
            if (!assertion)
            {
                throw new Exception("Assertion failed.");
            }
        }
        else
        {
            Trace.Assert(assertion, "Assertion failed.");
        }
    }

    private Check()
    {
    }
}

