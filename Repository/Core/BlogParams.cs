namespace Repository.Core
{
    public class BlogParams : PagingParams
    {
        public DateTime StartDate { get; set; } = new DateTime(2023, 08, 01);
    }
}
