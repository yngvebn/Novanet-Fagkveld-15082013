using ExternalApi.Contracts.Orders;
using Rikstoto.ExternalApi.Contracts;

namespace ExternalApi.API.Rest.Providers
{
    public class DummyProvider: IProvideDataFor<Contracts.Orders.CustomerWithOrders>
    {
        public void Fill(CustomerWithOrders model)
        {
            if (model.Name == "Yngve") model.Name += " Nilsen";
        }
    }

    public class FirstProvider: IProvideDataFor<Contracts.Orders.CustomerWithOrders>, IMustProvideDataFirst
    {
        public void Fill(CustomerWithOrders model)
        {
            model.Name = "Yngve";
        }
    }
}