using System.Collections.Generic;
using System.Linq;
using Rikstoto.ExternalApi.Contracts;

namespace ExternalApi.API.Rest.Infrastructure
{
    public class ManageDataProviders : IManageDataProviders
    {
        private readonly IEnumerable<IProvideDataFor> _dataProviders;

        public ManageDataProviders(IEnumerable<IProvideDataFor> dataProviders)
        {
            _dataProviders = dataProviders;
        }

        public void FillModelFromProviders<T>(T model)
        {
            new ManageDataProvidersFor<T>(_dataProviders.OfType<IProvideDataFor<T>>()).FillModelFromProviders(model);
        }
    }
}