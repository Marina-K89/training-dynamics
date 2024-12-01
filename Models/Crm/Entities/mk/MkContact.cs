using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace AwaraIT.Training.Domain.Models.Crm.Entities
{
    [EntityLogicalName(EntityLogicalName)]
    public class MkContact : BaseEntity
    {
        public MkContact() : base(EntityLogicalName) { }

        public static class Metadata
        {
            public const string FullName = "mk_name";
            public const string FirstName = "mk_firstname";
            public const string LastName = "mk_lastname";
            public const string SurName = "mk_surname";
            public const string Phone = "mk_phone";
            public const string Email = "mk_email";
            public const string Territory = "mk_territoryid";
        }

        public const string EntityLogicalName = "mk_contact";

        public string FullName
        {
            get { return GetAttributeValue<string>(Metadata.FullName); }
            set { Attributes[Metadata.FullName] = value; }
        }

        public string FirstName
        {
            get { return GetAttributeValue<string>(Metadata.FirstName); }
            set { Attributes[Metadata.FirstName] = value; }
        }

        public string LastName
        {
            get { return GetAttributeValue<string>(Metadata.LastName); }
            set { Attributes[Metadata.LastName] = value; }
        }

        public string SurName
        {
            get { return GetAttributeValue<string>(Metadata.SurName); }
            set { Attributes[Metadata.SurName] = value; }
        }

        public string Phone
        {
            get { return GetAttributeValue<string>(Metadata.Phone); }
            set { Attributes[Metadata.Phone] = value; }
        }

        public string Email
        {
            get { return GetAttributeValue<string>(Metadata.Email); }
            set { Attributes[Metadata.Email] = value; }
        }

        public EntityReference Territory
        {
            get { return GetAttributeValue<EntityReference>(Metadata.Territory); }
            set { Attributes[Metadata.Territory] = value; }
        }
    }
}
