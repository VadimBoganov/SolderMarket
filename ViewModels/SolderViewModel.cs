using Microsoft.AspNetCore.Http;
using solder.Models;

namespace solder.ViewModels
{
    public class SolderViewModel
    {
        public string Name { get; set; }
        public Solder Type {get;set;}
        public int Price {get;set;}
        public IFormFile Avatar { get; set; }
    }
}