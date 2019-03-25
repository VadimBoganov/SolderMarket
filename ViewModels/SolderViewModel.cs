using Microsoft.AspNetCore.Http;
using solder.Models;

namespace solder.ViewModels
{
    public class SolderViewModel : Product
    {
        public IFormFile Avatar { get; set; }
    }
}