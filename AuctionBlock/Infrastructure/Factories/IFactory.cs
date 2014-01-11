namespace AuctionBlock.Infrastructure.Factories
{
    public interface IFactory<TProduct>
    {
        TProduct Create();
    }
}
