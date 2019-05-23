using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using solder.ViewModels;

namespace solder.Models
{
    public interface IRepository
    {
        IEnumerable<T> GetAll<T>();
        T Get<T>(int? id) where T : class;
        Task<T> GetAsync<T>(int? id) where T : class;
        void Add<T>(T item);
        Task AddAsync<T>(T itme);
        void Update<T>(T item);
        Task UpdateAsync<T>(T item);
        void Delete<T>(T solder);
        Task DeleteAsync<T>(T solder);

    }
}