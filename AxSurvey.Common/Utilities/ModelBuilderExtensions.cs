using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace AxSurvey.Common.Utilities
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Dynamically register all Entities that inherit from specific BaseType
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="assemblies">Assemblies contains Entities</param>
        public static void RegisterAllEntities<TBaseType>(this ModelBuilder modelBuilder, params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(a => a.GetExportedTypes()).Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(TBaseType).IsAssignableFrom(c));
            var list = types.ToList();
            foreach (var type in list)
            {
                modelBuilder.Entity(type);
                modelBuilder.Entity(type).Property<byte[]>("RowVersion").IsRowVersion();
            }
        }

    }
}
