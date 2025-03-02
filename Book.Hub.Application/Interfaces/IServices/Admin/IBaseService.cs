using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Interfaces.IServices.Admin
{
    public interface IBaseService
    {
        void GenericEditMethod<TDto, TEntity>
            (TDto dto, TEntity entity, string? dateOnlyPropertyName = null)
            where TDto : class
            where TEntity : class;

        Task<byte[]> HandleImageFiles(IFormFile file);
    }
}
