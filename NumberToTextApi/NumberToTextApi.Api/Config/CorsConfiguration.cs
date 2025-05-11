namespace NumberToTextApi.Api.Config
{
    public static class CorsConfiguration
    {
        public static void ConfigureCors(this IServiceCollection services, IConfiguration config )
        {
            services.AddCors(options =>
            {
            options.AddPolicy("CorsPolicy",
                builder => builder//.WithOrigins(["http://localhost:5173"])
                .AllowAnyHeader()
                //.AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyOrigin());
            });
        }
    }
}
