using System.Collections.Generic;
using solder.Models;

namespace solder.ViewModels
{
    public class CommonSolderViewModel
    {
        public IEnumerable<Solder> Solders;
        public IEnumerable<SolderType> SolderTypes;
        public IEnumerable<SolderProduct> Products;
    }
}