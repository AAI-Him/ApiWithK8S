namespace ApiWithK8S.Extensions
{
    public static class StringExtensions
    {
        public static string CapitalizeFirstCharacter(this string value) =>
            value switch
            {
                null => throw new ArgumentNullException(nameof(value)),
                "" => throw new ArgumentException($"{nameof(value)} cannot be empty"),
                _ => string.Concat(value[0].ToString().ToUpper(), value.AsSpan(1))
            };
    }
}
