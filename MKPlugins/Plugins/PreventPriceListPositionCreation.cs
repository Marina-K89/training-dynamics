using AwaraIT.Training.Application.Core;
using AwaraIT.Training.Domain.Extensions;
using AwaraIT.Training.Domain.Models.Crm.Entities;
using AwaraIT.Training.Domain.Models.Crm.Entities.mk;
using MKPlugins.PluginExtensions;
using MKPlugins.PluginExtensions.Enums;
using MKPlugins.PluginExtensions.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;
using Microsoft.Xrm.Sdk.Extensions;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;

namespace MKPlugins.Plugins
{
    public class PreventPriceListPositionCreation : PluginBase
    {
        public PreventPriceListPositionCreation()
        {
            Subscribe
                .ToMessage(CrmMessage.Create)
                .ForEntity(MkPricelistPosition.EntityLogicalName)
                .When(PluginStage.PreOperation)
                .Execute(Execute);

            Subscribe
                .ToMessage(CrmMessage.Update)
                .ForEntity(MkPricelistPosition.EntityLogicalName)
                .WithAnyField(MkPricelistPosition.Metadata.PreparationFormat, MkPricelistPosition.Metadata.LessonFormat, MkPricelistPosition.Metadata.Subject, MkPricelistPosition.Metadata.Territory)
                .When(PluginStage.PreOperation)
                .Execute(Execute);
            
        }

        private void Execute(IContextWrapper wrapper)
        {
            var PriceListPositionEntity = wrapper.TargetEntity.ToEntity<MkPricelistPosition>();

            int PreparationFormat = 0;
            int LessonFormat = 0;
            int Subject = 0;
            Guid TerritoryId = Guid.Empty;
                        

            if (wrapper.Context.MessageName == "Create")
            {
                PreparationFormat = PriceListPositionEntity.PreparationFormat.Value;
                LessonFormat = PriceListPositionEntity.LessonFormat.Value;
                Subject = PriceListPositionEntity.Subject.Value;
                TerritoryId = PriceListPositionEntity.Territory.Id;
            }
            else if (wrapper.Context.MessageName == "Update")
            {
                var preImage = wrapper.Context.PreEntityImages["PreImage"];                         

                PreparationFormat = preImage.GetAttributeValue<OptionSetValue>(MkPricelistPosition.Metadata.PreparationFormat).Value;
                LessonFormat = preImage.GetAttributeValue<OptionSetValue>(MkPricelistPosition.Metadata.LessonFormat).Value;
                Subject = preImage.GetAttributeValue<OptionSetValue>(MkPricelistPosition.Metadata.Subject).Value;
                TerritoryId = preImage.GetAttributeValue<EntityReference>(MkPricelistPosition.Metadata.Territory).Id;           

            }


            var query = new QueryExpression(MkPricelistPosition.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(false),
                Criteria = new FilterExpression(LogicalOperator.And)
                {
                    Conditions =
                    {
                        new ConditionExpression(MkPricelistPosition.Metadata.StateCode, ConditionOperator.Equal, (int)MkPricelistPosition.Metadata.StateCodeOptions.Активный),
                        new ConditionExpression(MkPricelistPosition.Metadata.PreparationFormat, ConditionOperator.Equal, PreparationFormat),
                        new ConditionExpression(MkPricelistPosition.Metadata.LessonFormat, ConditionOperator.Equal, LessonFormat),
                        new ConditionExpression(MkPricelistPosition.Metadata.Subject, ConditionOperator.Equal, Subject),
                        new ConditionExpression(MkPricelistPosition.Metadata.Territory, ConditionOperator.Equal, TerritoryId)
                    }
                }
            };

            var activePriceLists = wrapper.Service.RetrieveMultiple(query).Entities.ToList<Entity>();

            if (activePriceLists.Any())
            {
                throw new InvalidPluginExecutionException($"Внимание дубль! Такая прайс-лист позиция существует.");
            }
        }

    }
}
