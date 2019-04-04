using Microsoft.AspNetCore.Http;
using solder.Models;

namespace solder.ViewModels
{
    public class SolderViewModel
    {
        public string Name { get; set; }
        public int SolderTypeId {get;set;}
        public int ProductId {get;set;}
        public int Price {get;set;}
        public IFormFile Avatar { get; set; }
    }
}