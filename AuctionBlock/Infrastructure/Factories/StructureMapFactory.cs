using StructureMap;

namespace AuctionBlock.Infrastructure.Factories
{
    public class StructureMapFactory<TProduct> : IFactory<TProduct>
    {
        public TProduct Create()
        {
            return ObjectFactory.GetInstance<TProduct>();
        }
    }
}