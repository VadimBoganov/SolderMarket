namespace solder.Models
{
    public abstract class SolderEntity
    {
        public int Id {get; set;}
        public string Name {get;set;}
    }
    public class SolderType : SolderEntity
    {

    }

    public class Solder : SolderEntity
    {
        public int SolderTypeId {get;set;}
        public SolderType SolderType {get;set;}
        public int ProductId {get;set;}
        public SolderProduct SolderProduct {get;set;}
        public int Price {get;set;}
        public string PictureName {get;set;}
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
        SolderProductAsc,
        SolderProductDesc,
        PriceAsc,
        PriceDesc
    }
}