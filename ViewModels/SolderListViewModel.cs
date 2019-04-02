using System.Collections.Generic;
using solder.Models;

namespace solder.ViewModels
{
    public class SolderListViewModel
    {
        public IEnumerable<Solder> Solders {get;set;}
        public string Name {get;set;}
        
    }
}