namespace solder.Models
{
    public class SolderType
    {
        public int Id {get;set;}
        public string Name {get;set;}
    }

    public class Solder
    {
        public int Id {get;set;}
        public string Name {get;set;}
        public SolderType SolderType {get;set;}
    }

    public class Product
    {
        public int Id {get;set;}
        public string Name {get;set;}
        public int Price {get;set;}
        public byte[] Picture {get;set;}
        public Solder ProductType {get;set;}

    }

    // public enum SolderType
    // {
    //     LeadTin, //Оловянно-свинцовые
    //     SpecialAndFusible, //Специальные и легкоплавкие
    //     Babbit
    // }

}