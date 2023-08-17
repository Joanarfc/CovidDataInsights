namespace CDI.CovidDataManagement.API.Data.Repository
{
    public class Repository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task PersistData()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}