namespace Shiny.Extensions.Mail.Impl.Helpers
{
    public static class RazorTemplateHelper
    {
        public static string ToMailAddress(string address, string? displayName)
        {
            if (String.IsNullOrEmpty(displayName))
                return address;

            return $"{displayName} <{address}>";
        }


        public static string ToMailGroup(this IEnumerable<string> addresses)
        {
            if (addresses == null)
                return String.Empty;

            var result = String.Join(";", addresses);
            return result;
        }
    }
}
