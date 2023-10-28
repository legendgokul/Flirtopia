namespace ApiProject.Helpers{

    /// <summary>
    /// model which gets returns in Response header of this request Based on Request params.
    /// </summary>
    public class PaginationHeader{

        public PaginationHeader(int currentPage , 
        int itemsperPage, int totalItems , int totalPages){
            CurrentPage = currentPage;
            ItemPerPage = itemsperPage;
            TotalItems = totalItems;
            TotalPages = totalPages;
        }

        public int CurrentPage{get; set;}
        public int ItemPerPage{get;set;}
        public int TotalItems {get;set;}
        public int TotalPages {get;set;}
    }
} 