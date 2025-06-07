using System.Collections.Generic;

public class ValidationException : System.Exception
{
    public IDictionary<string, List<string>> Failures { get; }

    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Failures = new Dictionary<string, List<string>>();
    }

    public ValidationException(IDictionary<string, List<string>> failures)
    {
        this.Failures = failures;
    }

    public ValidationException(string title, List<string> failures)
    {
        this.Failures = new Dictionary<string, List<string>> { { title, failures } };
    }
}