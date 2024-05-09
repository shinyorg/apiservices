namespace Shiny.Contracts;

public record Address(
    string Country,
    string StateProvince,
    string City,
    string PostalCode,
    string Value1,
    string Value2
);