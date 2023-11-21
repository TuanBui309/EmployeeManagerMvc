using X.PagedList;

namespace Entity.Pagination
{
    public class GetPagination
    {
        public static async Task<IEnumerable<T>> GetPage<T>(IQueryable<T> query, int? pageNumber = null, int currentPage = 1, int pageSize = 5) where T : class
        {
            return await query.ToPagedListAsync(pageNumber ?? currentPage, pageSize);
        }
    }
}
