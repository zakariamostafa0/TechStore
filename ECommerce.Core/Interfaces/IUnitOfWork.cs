namespace ECommerce.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IPhotoRepository PhotoRepository { get; }
        //Task<int> CompleteAsync();
        //Task DisposeAsync();
    }
}
