namespace solder.Models
{
    public class Solder
    {
        public int Id {get;set;}
        public string Name {get;set;}
        public SolderType Type {get;set;}
        public int Price {get;set;}
        public byte[] Picture {get;set;}
    }

    public enum SolderType
    {
        LeadTin, //Оловянно-свинцовые
        SpecialAndFusible, //Специальные и легкоплавкие
        Babbit
    }

}