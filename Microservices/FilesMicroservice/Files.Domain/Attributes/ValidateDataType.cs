using System;

namespace Files.Domain.Attributes
{
    public class ValidateDataType : Attribute
    {
        public bool IsNumber { get; set; }
        public bool IsDecimal { get; set; }
        public bool IsUnique { get; set; }
        public bool IsRequired { get; set; }
        public bool IsDateTime { get; set; }

        public ValidateDataType()
        {
            IsNumber = false;
            IsDateTime = false;
            IsRequired = false;
            IsUnique = false;
            IsDecimal = false;
        }
    }
}
