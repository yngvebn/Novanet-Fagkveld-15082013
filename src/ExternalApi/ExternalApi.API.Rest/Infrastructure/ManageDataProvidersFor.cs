using System.Collections.Generic;
using System.Linq;
using Rikstoto.ExternalApi.Contracts;

namespace ExternalApi.API.Rest.Infrastructure
{
    public class ManageDataProvidersFor<T> : IManageDataProvidersFor<T>
    {
        private readonly IEnumerable<IProvideDataFor<T>> _dataProviders;

        public ManageDataProvidersFor(IEnumerable<IProvideDataFor<T>> dataProviders)
        {
            _dataProviders = dataProviders;
        }

        public void FillModelFromProviders(T model)
        {
            List<IProvideDataFor<T>> sortedProviders = new List<IProvideDataFor<T>>();
            var firstProvider =
                _dataProviders.FirstOrDefault(p => typeof (IMustProvideDataFirst).IsInstanceOfType(p));
            if (firstProvider != null)
                sortedProviders.Add(firstProvider);

            sortedProviders.AddRange(_dataProviders.Where(p => !typeof (IMustProvideDataFirst).IsInstanceOfType(p)));

            sortedProviders.ForEach(provider => provider.Fill(model));
        }
    }



}