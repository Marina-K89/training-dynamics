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
    public class PreventPriceListCreation : PluginBase
    {
        public PreventPriceListCreation()
        {
            Subscribe
                .ToMessage(CrmMessage.Create)
                .ForEntity(MkPricelist.EntityLogicalName)
                .When(PluginStage.PreOperation)
                .Execute(Execute);
        }

        private void Execute(IContextWrapper wrapper)
        {
            
            var query = new QueryExpression(MkPricelist.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(false),
                Criteria = new FilterExpression(LogicalOperator.And)
                {
                    Conditions =
                    {
                        new ConditionExpression(MkPricelist.Metadata.StateCode, ConditionOperator.Equal, (int)MkPricelist.Metadata.StateCodeOptions.Активный)
                    }
                }
            };

            var activePriceLists = wrapper.Service.RetrieveMultiple(query).Entities.ToList<Entity>();

            if(activePriceLists.Any())
            {
                throw new InvalidPluginExecutionException($"Внимание! Один активный прайс-лист существует.");
            }
        }
    }
}
