namespace ApiProject.Helpers
{
    // class representing the user input for pagination.
    public class UserParams{
        private const int MaxPageSize = 50;
        public int PageNumber {get;set;} = 1;
        private int _pageSize = 10 ;
        public int PageSize {
            get => _pageSize;
            set => _pageSize = (value>MaxPageSize)?MaxPageSize:value;
        }

        public string CurrentUsername {get;set;}
        public string Gender {get;set;}
    }
}