using System.Collections.Generic;
using System;

public class ApplicationValidationException : Exception
{
    public List<ApplicationErrorModel> Errors { get; set; } = new List<ApplicationErrorModel>();
    public ApplicationValidationException(List<ApplicationErrorModel> errors)
    {
        this.Errors = errors;
    }
    public ApplicationValidationException(ApplicationErrorModel error)
    {
        this.Errors.Add(error);
    }
}