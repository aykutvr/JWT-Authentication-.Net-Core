namespace JWTAuthentication.Builders
{
    public class JWTAuthenticationBuilder
    {
        public void SetSecretKey(string value)
            => Settings.SecretKey = value;
        public void SetJWTIssuer(string value)
            => Settings.JWTIssuer = value;
    }
}
