namespace ApiProject.Entities{

    //many to many relationship in EF
    public class UserLike
    {
        public AppUser SourceUser {get;set;}
        public int SourceUserId {get;set;}
        public AppUser TargetUser {get;set;}
        public int TargetUserId {get;set;}
    }
}