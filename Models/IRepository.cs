using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using solder.ViewModels;

namespace solder.Models
{
    public interface IRepository
    {
        IEnumerable<Solder> GetAll();
        Solder Get(int? id);
        Task<Solder> GetAsync(int? id);
        void Add(Solder solder);
        Task AddAsync(Solder solder);
        void Update(Solder solder);
        Task UpdateAsync(Solder solder);
        void Delete(Solder solder);
        Task DeleteAsync(Solder solder);

    }
}