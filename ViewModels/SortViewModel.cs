using solder.Models;

namespace solder.ViewModels
{
    public class SortViewModel
    {
        public SortState NameSort {get;set;}
        public SortState SolderTypeSort {get;set;}
        public SortState SolderProductSort {get;set;}
        public SortState PriceSort {get;set;}
        public SortState Current {get;set;}
        public bool Up {get;set;}

        public SortViewModel(SortState sortSolders)
        {
            NameSort = SortState.NameAsc;
            SolderTypeSort = SortState.SolderTypeAsc;
            SolderProductSort = SortState.SolderProductAsc;
            PriceSort = SortState.PriceAsc;
            Up = true;

            if(sortSolders == SortState.NameDesc || sortSolders == SortState.PriceDesc 
                || sortSolders == SortState.SolderProductDesc || sortSolders == SortState.SolderTypeDesc)
            {
                Up = false;
            }

            switch(sortSolders)
            {
                 case SortState.NameDesc:
                    Current = NameSort = SortState.NameAsc;
                    break;
                case SortState.PriceAsc:
                    Current = PriceSort = SortState.PriceDesc;
                    break;
                case SortState.PriceDesc:
                    Current = PriceSort = SortState.PriceAsc;
                    break;
                case SortState.SolderTypeAsc:
                    Current = SolderTypeSort = SortState.SolderTypeDesc;
                    break;
                case SortState.SolderTypeDesc:
                    Current = SolderTypeSort = SortState.SolderTypeAsc;
                    break;
                case SortState.SolderProductDesc:
                    Current = SolderProductSort = SortState.SolderProductAsc;
                    break;
                case SortState.SolderProductAsc:
                    Current = SolderProductSort = SortState.SolderProductDesc;
                    break;        
                default:
                    Current = NameSort = SortState.NameDesc;
                    break;
            }
        }
    }
}