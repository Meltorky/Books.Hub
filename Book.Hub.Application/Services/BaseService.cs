using Books.Hub.Application.Interfaces.IServices;
using Books.Hub.Application.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Services
{
    public class BaseService : IBaseService
    {
        public void GenericMapDtoToEntity<TDto, TEntity>(TDto dto, TEntity entity)
            where TDto : class
            where TEntity : class
        {
            var dtoProperties = typeof(TDto).GetProperties();
            var entityProperties = typeof(TEntity).GetProperties().ToDictionary(p => p.Name, p => p);

            foreach (var dtoProperty in dtoProperties)
            {
                // Skip Id updates
                if (dtoProperty.Name == "Id") continue;

                var newValue = dtoProperty.GetValue(dto);
                
                // Skip null values
                if (newValue == null) continue; 

                // Try to get the matching property in the entity by name
                if (!entityProperties.TryGetValue(dtoProperty.Name, out var entityProp)) 
                    continue;
                
                // set the entity's property to the DTO's non-null value
                entityProp.SetValue(entity, newValue);               
            }
        }

        public async Task<byte[]> HandleImageFiles(IFormFile file) 
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            return stream.ToArray();
        }


        //public void GenericEditMethodv0<TDto, TEntity>(TDto dto, TEntity entity)
        //    where TDto : class
        //    where TEntity : class
        //{
        //    var dtoProperties = typeof(TDto).GetProperties();
        //    var entityProperties = typeof(TEntity).GetProperties().ToDictionary(p => p.Name, p => p);

        //    foreach (var dtoProperty in dtoProperties)
        //    {
        //        if (dtoProperty.Name == "Id") // Skip Id updates
        //            continue;

        //        var newValue = dtoProperty.GetValue(dto);
        //        if (newValue == null)
        //            continue; // Skip null values

        //        // Try to get the matching property in the entity by name
        //        if (!entityProperties.TryGetValue(dtoProperty.Name, out var entityProp)) continue;

        //        // Special handling: If the target entity property is of type DateOnly
        //        if (entityProp.PropertyType == typeof(DateOnly))
        //        {
        //            // If the DTO value is a string, try to parse it to DateOnly
        //            if (newValue is string str && DateOnly.TryParse(str, out var dateOnly))
        //            {
        //                entityProp.SetValue(entity, dateOnly); // Assign parsed DateOnly
        //            }
        //            // If the DTO value is already a DateTime, convert it to DateOnly
        //            else if (newValue is DateTime dt)
        //            {
        //                entityProp.SetValue(entity, DateOnly.FromDateTime(dt));
        //            }

        //            continue; // Skip remaining logic for this property (already set it)
        //        }
        //        // Default case: set the entity's property to the DTO's non-null value
        //        entityProp.SetValue(entity, newValue);

        //    }
        //}
    }
}
