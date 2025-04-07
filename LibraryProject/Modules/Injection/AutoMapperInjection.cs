using LibraryProject.Transversal.Mapper;

namespace LibraryProject.Modules.Injection
{
    public static class AutoMapperInjection
    {
        public static IServiceCollection AddCustomAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingsProfile));
            return services;
        }
    }
}
