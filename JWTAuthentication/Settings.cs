namespace JWTAuthentication
{
    public class Settings
    {
        internal static string SecretKey { get; set;} = "";
        internal static string JWTIssuer { get; set; } = "";
        internal static Type UserDataObjectType { get; set; } = null;
    }
}
