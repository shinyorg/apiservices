namespace Shiny.Contracts;

public record ContactMethod(
    ContactMethodType Type,
    string Value
);

public enum ContactMethodType
{
    Email,
    Phone,
    Sms
}