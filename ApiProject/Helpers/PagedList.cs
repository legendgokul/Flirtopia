using Microsoft.EntityFrameworkCore;

namespace ApiProject.Helpers{

    // T because this needs to work for any class.
    // Similar to List , we create a Enumerable class called PageList and give custom attributes.
    public class PageList<T> : List<T>{

        public PageList(IEnumerable<T> items , int count, int pageNumber, int pageSize){

            CurrentPage = pageNumber;
            TotalPages = (int) Math.Ceiling(count/ (double) pageSize);
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items);
        }

        public int CurrentPage {get;set;} // represent current page index.
        public int TotalPages {get;set;}  // Totalcount / size 
        public int PageSize {get;set;}  // no of item per page.
        public int TotalCount {get;set;} // total item count

        public static async Task<PageList<T>> CreateAsync(IQueryable<T> source,
        int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber-1) * pageSize).Take(pageSize).ToListAsync();

            return new PageList<T>(items,count,pageNumber,pageSize);
        }

    }

}