using Books.Hub.Application.Interfaces.IServices.Admin;
using Books.Hub.Application.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Services.Admin
{
    public class BaseService : IBaseService
    {
        public void GenericEditMethod<TDto, TEntity>
            (TDto dto, TEntity entity, string? dateOnlyPropertyName = null)
            where TDto : class
            where TEntity : class
        {
            var dtoProperties = typeof(TDto).GetProperties();
            var entityProperties = typeof(TEntity).GetProperties().ToDictionary(p => p.Name, p => p);

            foreach (var dtoProperty in dtoProperties)
            {
                if (dtoProperty.Name == "Id") // Skip Id updates
                    continue;

                var newValue = dtoProperty.GetValue(dto);
                if (newValue == null)
                    continue; // Skip null values

                if (entityProperties.TryGetValue(dtoProperty.Name, out var entityProperty))
                {
                    // Handle DateOnly conversion (if applicable)
                    if (!string.IsNullOrEmpty(dateOnlyPropertyName) &&
                        dtoProperty.Name == dateOnlyPropertyName &&
                        entityProperty.PropertyType == typeof(DateOnly))
                    {
                        if (DateOnly.TryParse(newValue.ToString(), out var parsedDate))
                        {
                            newValue = parsedDate;
                        }
                        else
                        {
                            continue; // Skip if invalid date format
                        }
                    }

                    entityProperty.SetValue(entity, newValue);
                }
            }
        }

        public async Task<byte[]> HandleImageFiles(IFormFile file) 
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            return stream.ToArray();
        }
    }
}
