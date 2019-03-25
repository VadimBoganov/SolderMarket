using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using solder.ViewModels;
using System;
using Microsoft.AspNetCore.Mvc;

namespace solder.Models
{
    public class SolderRepository : IRepository
    {
        SolderContext _db;
        
        public SolderRepository(SolderContext ctx)
        {
            _db = ctx;
        }
        public IEnumerable<Solder> GetAll()
        {
            return _db.Solders.ToList().OrderBy(s => s.Id);
        }

        public Solder Get(int? id)
        {
            return _db.Solders.FirstOrDefault(s => s.Id == id);
        }

        public async Task<Solder> GetAsync(int? id)
        {
            Solder solder = await _db.Solders.FirstOrDefaultAsync(s => s.Id == id);    
            return solder;
        }
        
        public void Add(Solder solder)    
        {
            _db.Solders.Add(solder);
            _db.SaveChanges();
        }

        public async Task AddAsync(Solder solder)
        {
            await Task.Run(() => Add(solder));
        }

        public void Update(Solder solder)
        {
            _db.Solders.Update(solder);
            _db.SaveChanges();
        }

        public async Task UpdateAsync(Solder solder)
        {
            await Task.Run(() => Update(solder));
        }
        
        public async Task DeleteAsync(Solder solder)
        {
            await Task.Run(() => Delete(solder));
        }

        public void Delete(Solder solder)
        {
            _db.Solders.Remove(solder);
            _db.SaveChanges();
        }
    }
}