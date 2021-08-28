namespace Sandbox.Application
{
    public class SetCompanyNipDto
    {
        public SetCompanyNipDto(string name, string nip)
        {
            Name = name;
            Nip = nip;
        }

        public string Name { get; set; }
        
        public string Nip { get; set; }
    }
}