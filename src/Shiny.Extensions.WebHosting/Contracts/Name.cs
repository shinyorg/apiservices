namespace Shiny.Contracts;

public record Name(string First, string Last)
{
    public string Full => $"{First} {Last}".Trim();
};