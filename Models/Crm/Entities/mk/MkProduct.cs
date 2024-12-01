using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace AwaraIT.Training.Domain.Models.Crm.Entities.mk
{
    [EntityLogicalName(EntityLogicalName)]
    public class MkProduct : BaseEntity
    {
        public MkProduct() : base(EntityLogicalName) { }

        public static class Metadata
        {
            public const string Name = "mk_name";
            public const string PreparationFormat = "mk_preparation_format";
            public const string LessonFormat = "mk_lesson_format";
            public const string Subject = "mk_subject";
        }

        public const string EntityLogicalName = "mk_product";

        public string Name
        {
            get { return GetAttributeValue<string>(Metadata.Name); }
            set { Attributes[Metadata.Name] = value; }
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
       
    }
}

