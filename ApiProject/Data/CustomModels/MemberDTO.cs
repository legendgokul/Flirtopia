namespace ApiProject.Data.CustomModels
{
    public class MemberDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string photoUrl { get; set; }
        public int Age { get; set; }
        public string knownAs { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastActive { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string lookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<PhotoDTO> Photos { get; set; }
    }


}