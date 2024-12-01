using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace AwaraIT.Training.Domain.Models.Crm.Entities.mk
{
    [EntityLogicalName(EntityLogicalName)]
    public class MkProductBasket : BaseEntity
    {
        public MkProductBasket() : base(EntityLogicalName) { }

        public static class Metadata
        {
            public const string Name = "mk_name";
            public const string Product = "mk_productid";
            public const string Discount = "mk_discount";
            public const string BasePrice = "mk_base_price";
            public const string PriceAfterDiscount = "mk_price_after_discount";
            public const string Deal = "mk_dealid";
        }

        public const string EntityLogicalName = "mk_product_basket";

        public string Name
        {
            get { return GetAttributeValue<string>(Metadata.Name); }
            set { Attributes[Metadata.Name] = value; }
        }

        public EntityReference Product
        {
            get { return GetAttributeValue<EntityReference>(Metadata.Product); }
            set { Attributes[Metadata.Product] = value; }
        }

        public decimal Discount
        {
            get { return GetAttributeValue<decimal>(Metadata.Discount); }
            set { Attributes[Metadata.Discount] = value; }
        }

        public decimal BasePrice
        {
            get { return GetAttributeValue<decimal>(Metadata.BasePrice); }
            set { Attributes[Metadata.BasePrice] = value; }
        }

        public decimal PriceAfterDiscount
        {
            get { return GetAttributeValue<decimal>(Metadata.PriceAfterDiscount); }
            set { Attributes[Metadata.PriceAfterDiscount] = value; }
        }

        public EntityReference Deal
        {
            get { return GetAttributeValue<EntityReference>(Metadata.Deal); }
            set { Attributes[Metadata.Deal] = value; }
        }      

    }
}
