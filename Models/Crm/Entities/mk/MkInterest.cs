using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using static AwaraIT.Training.Domain.Models.Crm.Entities.mk.MkInterest.Metadata;

namespace AwaraIT.Training.Domain.Models.Crm.Entities.mk
{
    [EntityLogicalName(EntityLogicalName)]
    public class MkInterest : BaseEntity
    {
        public MkInterest() : base(EntityLogicalName) { }

        public static class Metadata
        {
            public const string Name = "mk_name";
            public const string FirstName = "mk_firstname";
            public const string LastName = "mk_lastname";
            public const string SurName = "mk_surname";
            public const string Phone = "mk_phone";
            public const string Email = "mk_email";
            public const string Territory = "mk_territoryid";
            public const string Contact = "mk_contactid";
            public const string Status = "statuscode";
            public const string Owner = "ownerid";

            public enum InterestStatusOptions
            {
                Новый = 1,
                В_работе = 124620001,
                Согласие = 124620002,
                Отказ = 2 
            }
        }

        public const string EntityLogicalName = "mk_interest";

        public string Name
        {
            get { return GetAttributeValue<string>(Metadata.Name); }
            set { Attributes[Metadata.Name] = value; }
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

        public EntityReference Contact
        {
            get { return GetAttributeValue<EntityReference>(Metadata.Contact); }
            set { Attributes[Metadata.Contact] = value; }
        }

        public EntityReference Owner
        {
            get { return GetAttributeValue<EntityReference>(Metadata.Owner); }
            set { Attributes[Metadata.Owner] = value; }
        }

        public InterestStatusOptions? Status
        {
            get { return (InterestStatusOptions?)GetAttributeValue<OptionSetValue>(Metadata.Status)?.Value; }
            set { Attributes[Metadata.Status] = value != null ? new OptionSetValue((int)value.Value) : null; }
        }

    }
}