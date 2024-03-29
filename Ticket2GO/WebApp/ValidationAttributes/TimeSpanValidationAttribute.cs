﻿using System.ComponentModel.DataAnnotations;

public class TimeSpanAttribute : ValidationAttribute
{
    public TimeSpanAttribute()
    {
        ErrorMessage = "The {0} field must be a valid time between {1} and {2}.";
    }

    public override bool IsValid(object? value)
    {
        if (value is not TimeSpan timeSpan)
        {
            return false;
        }

        return timeSpan > TimeSpan.Zero && timeSpan <= TimeSpan.FromDays(1);
    }

    public override string FormatErrorMessage(string name)
    {
        string defaultMessage = "{0} must be a valid time span between {1} and {2}.";
        string message = ErrorMessage != null ? ErrorMessage : defaultMessage;
        return string.Format(message, name, "00:00:01", "24:00:00");
    }
}
