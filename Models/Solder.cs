namespace solder.Models
{
    public class Solder : Product
    {
        public int Id {get;set;}
        public byte[] Picture {get;set;}
    }

    public enum SolderType
    {
        LeadTin, //Оловянно-свинцовые
        SpecialAndFusible, //Специальные и легкоплавкие
        Babbit
    }

}