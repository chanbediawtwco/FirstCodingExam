namespace FirstCodingExam
{
    public class Constants
    {
        // SQL database connection string
        public const string DefaultConnectionString = "DefaultConnection";

        // JWT token issuer settings
        public const string JwtIssuer = "Jwt:Issuer";
        public const string JwtAudience = "Jwt:Audience";
        public const string JwtKey = "Jwt:Key";
        public const string NameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        // Cors policy origin name
        public const string CorsOrigin = "FirstCodingExamOrigins";

        // Http context accessor
        public const string Authorization = "Authorization";
        public const string Bearer = "Bearer ";
    }
}
