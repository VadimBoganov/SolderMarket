namespace solder.Models
{
    public class SolderEntity
    {
        public int Id {get; set;}
        public string Name {get;set;}
    }
    public class SolderType : SolderEntity
    {

    }

    public class Solder
    {
        public int Id {get;set;}
        public string Name {get;set;}
        public int SolderTypeId {get;set;}
        public SolderType SolderType {get;set;}
        public int ProductId {get;set;}
        public SolderProduct Product {get;set;}
        public int Price {get;set;}
        public byte[] Picture {get;set;}
    }

    public class SolderProduct : SolderEntity
    {
        
    }

    public enum SortState
    {
        NameAsc,
        NameDesc,
        SolderTypeAsc,
        SolderTypeDesc,
        ProductAsc,
        ProductDesc,
        PriceAsc,
        PriceDesc
    }
}