using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace AwaraIT.Training.Domain.Models.Crm.Entities.mk
{
    [EntityLogicalName(EntityLogicalName)]
    public class MkPricelistPosition : BaseEntity
    {
        public MkPricelistPosition() : base(EntityLogicalName) { }

        public static class Metadata
        {
            public const string Name = "mk_name";
            public const string Territory = "mk_territoryid";
            public const string Pricelist = "mk_pricelistid";
            public const string PreparationFormat = "mk_preparation_format";
            public const string LessonFormat = "mk_lesson_format";
            public const string Subject = "mk_subject";
            public const string Price = "mk_price";
        }

        public const string EntityLogicalName = "mk_pricelist_position";

        public string Name
        {
            get { return GetAttributeValue<string>(Metadata.Name); }
            set { Attributes[Metadata.Name] = value; }
        }

        public EntityReference Territory
        {
            get { return GetAttributeValue<EntityReference>(Metadata.Territory); }
            set { Attributes[Metadata.Territory] = value; }
        }

        public EntityReference Pricelist
        {
            get { return GetAttributeValue<EntityReference>(Metadata.Pricelist); }
            set { Attributes[Metadata.Pricelist] = value; }
        }

        public OptionSetValue PreparationFormat
        {
            get { return GetAttributeValue<OptionSetValue>(Metadata.PreparationFormat); }
            set { Attributes[Metadata.PreparationFormat] = value; }
        }

        public OptionSetValue LessonFormat
        {
            get { return GetAttributeValue<OptionSetValue>(Metadata.LessonFormat); }
            set { Attributes[Metadata.LessonFormat] = value; }
        }

        public OptionSetValue Subject
        {
            get { return GetAttributeValue<OptionSetValue>(Metadata.Subject); }
            set { Attributes[Metadata.Subject] = value; }
        }

        public decimal Price
        {
            get { return GetAttributeValue<decimal>(Metadata.Price); }
            set { Attributes[Metadata.Price] = value; }
        }
    }
}
