using Microsoft.AspNetCore.Http;
using solder.Models;
namespace solder.ViewModels
{
    public class SolderViewModel : Solder
    {
        public IFormFile Avatar {get;set;}
    }
}