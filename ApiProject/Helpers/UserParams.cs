namespace ApiProject.Helpers
{
    // class representing the user input for pagination.
    // since it inherits PaginationParams class , this class will contain 
    // access to all the parameters present in PaginationParams class. 
    public class UserParams : PaginationParams{

        public string CurrentUsername {get;set;}
        public string Gender {get;set;}

        public int MinAge {get;set;}
        public int MaxAge {get;set;}
        public string orderBy {get;set;} = "lastActive";
             
    }
}