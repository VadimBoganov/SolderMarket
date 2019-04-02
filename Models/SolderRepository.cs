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
        public IEnumerable<T> GetAll<T>()
        {
            Type type = typeof(T);
            
            switch(type.Name)
            {
                case "SolderType":
                    return _db.SolderTypes.ToList().OrderBy(s => s.Id) as IEnumerable<T>;
                case "Solder":
                    return _db.Solders.ToList().OrderBy(s => s.Id) as IEnumerable<T>;    
                default:
                    return _db.Products.ToList().OrderBy(s => s.Id) as IEnumerable<T>;

            }
        }

        public T Get<T>(int? id) where T : class
        {
            Type type = typeof(T);

            switch(type.Name)
            {
                case "SolderType":
                    return _db.SolderTypes.FirstOrDefault(s => s.Id == id) as T;
                case "Solder":
                    return _db.Solders.FirstOrDefault(s => s.Id == id) as T;    
                default:
                    return _db.Products.FirstOrDefault(s => s.Id == id) as T;

            }
        }

        public async Task<T> GetAsync<T>(int? id) where T : class
        {
            Type type = typeof(T);

            switch(type.Name)
            {
                case "SolderType":
                    return await _db.SolderTypes.FirstOrDefaultAsync(s => s.Id == id) as T;
                case "Solder":
                    return await _db.Solders.FirstOrDefaultAsync(s => s.Id == id) as T;
                default:
                    return await _db.Products.FirstOrDefaultAsync(s => s.Id == id) as T;

            }
        }
        
        public void Add<T>(T item)    
        {
            Type type = typeof(T);

            switch(type.Name)
            {
                case "SolderType":
                    _db.SolderTypes.Add(item as SolderType);
                    _db.SaveChanges();
                    break;
                case "Solder":
                    _db.Solders.Add(item as Solder);
                    _db.SaveChanges();
                    break;
                default:
                     _db.Products.Add(item as Product);
                    _db.SaveChanges();
                    break;
            }
            
        }

        public async Task AddAsync<T>(T item)
        {
            await Task.Run(() => Add<T>(item));
        }

        public void Update<T>(T item)
        {
            Type type = typeof(T);

            switch(type.Name)
            {
                case "SolderType":
                    _db.SolderTypes.Update(item as SolderType);
                    _db.SaveChanges();
                    break;
                case "Solder":
                    _db.Solders.Update(item as Solder);
                    _db.SaveChanges();
                    break;
                 default:
                    _db.Products.Update(item as Product);
                    _db.SaveChanges();
                    break;
            }
        }

        public async Task UpdateAsync<T>(T item)
        {
            await Task.Run(() => Update<T>(item));
        }
        
        public async Task DeleteAsync<T>(T item)
        {
            await Task.Run(() => Delete<T>(item));
        }

        public void Delete<T>(T item)
        {
            Type type = typeof(T);

            switch(type.Name)
            {
                case "SolderType":
                    _db.SolderTypes.Remove(item as SolderType);
                    _db.SaveChanges();
                    break;
                case "Solder": 
                    _db.Solders.Remove(item as Solder);
                    _db.SaveChanges();
                    break;
                default:
                    _db.Products.Remove(item as Product);
                    _db.SaveChanges();
                    break;    
            }
        }
    }
}