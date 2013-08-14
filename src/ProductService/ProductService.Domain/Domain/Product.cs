namespace ProductService.Domain
{
    public class Product
    {
        public int ProductSku { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Manufacturer { get; set; }
        public decimal ListPrice { get; set; }
    }
}