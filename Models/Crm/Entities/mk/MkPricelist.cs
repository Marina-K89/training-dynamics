using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace AwaraIT.Training.Domain.Models.Crm.Entities.mk
{
    [EntityLogicalName(EntityLogicalName)]
    public class MkPricelist : BaseEntity
    {
        public MkPricelist() : base(EntityLogicalName) { }

        public static class Metadata
        {
            public const string Name = "mk_name";            
        }

        public const string EntityLogicalName = "mk_pricelist";

        public string Name
        {
            get { return GetAttributeValue<string>(Metadata.Name); }
            set { Attributes[Metadata.Name] = value; }
        }        

    }
}