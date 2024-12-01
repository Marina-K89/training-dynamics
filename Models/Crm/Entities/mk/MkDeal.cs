using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using static AwaraIT.Training.Domain.Models.Crm.Entities.mk.MkDeal.Metadata;

namespace AwaraIT.Training.Domain.Models.Crm.Entities.mk
{
    [EntityLogicalName(EntityLogicalName)]
    public class MkDeal : BaseEntity
    {
        public MkDeal() : base(EntityLogicalName) { }

        public static class Metadata
        {
            public const string Name = "mk_name";
            public const string Contact = "mk_contactid";
            public const string Territory = "mk_territoryid";
            public const string Status = "statuscode";
            public const string BasePriceSum = "mk_base_price_sum";
            public const string DiscountSum = "mk_discount_sum";
            public const string PriceAfterDiscountSum = "mk_price_after_discount_sum";
            public const string Owner = "ownerid";

            public enum DealStatusOptions
            {
                Открыто = 1,
                В_работе = 124620001,
                Выиграна = 124620002,
                Неактивные = 2
            }
        }

        public const string EntityLogicalName = "mk_deal";

        public string Name
        {
            get { return GetAttributeValue<string>(Metadata.Name); }
            set { Attributes[Metadata.Name] = value; }
        }

        public EntityReference Contact
        {
            get { return GetAttributeValue<EntityReference>(Metadata.Contact); }
            set { Attributes[Metadata.Contact] = value; }
        }

        public EntityReference Territory
        {
            get { return GetAttributeValue<EntityReference>(Metadata.Territory); }
            set { Attributes[Metadata.Territory] = value; }
        }

        public DealStatusOptions? Status
        {
            get { return (DealStatusOptions?)GetAttributeValue<OptionSetValue>(Metadata.Status)?.Value; }
            set { Attributes[Metadata.Status] = value != null ? new OptionSetValue((int)value.Value) : null; }
        }       

        public decimal BasePriceSum
        {
            get { return GetAttributeValue<decimal>(Metadata.BasePriceSum); }
            set { Attributes[Metadata.BasePriceSum] = value; }
        }

        public decimal DiscountSum
        {
            get { return GetAttributeValue<decimal>(Metadata.DiscountSum); }
            set { Attributes[Metadata.DiscountSum] = value; }
        }

        public decimal PriceAfterDiscountSum
        {
            get { return GetAttributeValue<decimal>(Metadata.PriceAfterDiscountSum); }
            set { Attributes[Metadata.PriceAfterDiscountSum] = value; }
        }

        public EntityReference Owner
        {
            get { return GetAttributeValue<EntityReference>(Metadata.Owner); }
            set { Attributes[Metadata.Owner] = value; }
        }

    }
}
