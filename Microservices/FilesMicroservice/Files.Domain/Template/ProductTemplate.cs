using Files.Domain.Attributes;

namespace Files.Domain.Template
{
    public class ProductTemplate
    {
        [ValidateDataType(IsRequired = true, IsUnique = true)]
        public string ProductNumber { get; set; }
        [ValidateDataType(IsRequired = true)]
        public string ProductName { get; set; }

        [ValidateDataType(IsNumber = true, IsRequired = true)]
        public string QtyPerPackage { get; set; }
        public string Notes { get; set; }
    }
}
