using System.Collections.Generic;
using System.Linq;
using CustomerService.Domain;

namespace CustomerService
{
    public interface ICustomerRepository
    {
        Customer Find(int customerId);
        IList<Customer> All();
    }

    public class CustomerRepository : ICustomerRepository
    {
        public Customer Find(int customerId)
        {
            return All().SingleOrDefault(customer => customer.CustomerId == customerId);
        }

        public IList<Customer> All()
        {
            return new List<Customer>()
                {
                    new Customer(){ CustomerId = 1, Name = "Yngve"},
                    new Customer(){ CustomerId = 2, Name = "Christian"},
                    new Customer(){ CustomerId = 3, Name = "Jan"},
                    new Customer(){ CustomerId = 4, Name = "Olav"},
                    new Customer(){ CustomerId = 5, Name = "Thomas"}
                };
        }
    }
}