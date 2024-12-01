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
using System.Activities.Expressions;
using System.Collections;
using System.Collections.Generic;

namespace MKPlugins.Plugins
{
    public class BasketCreationPlugin : PluginBase
    {
        public BasketCreationPlugin()
        {
            Subscribe
                .ToMessage(CrmMessage.Create)
                .ForEntity(MkProductBasket.EntityLogicalName)                
                .When(PluginStage.PostOperation)
                .Execute(Execute);
        }

        private void Execute(IContextWrapper wrapper)
        {
            var logger = new Logger(wrapper.Service);

            try
            {
                //logger.INFO("BasketCreationPlugin", "test", wrapper.TargetEntity.LogicalName, wrapper.TargetEntity.Id);

                var basketEntity = wrapper.TargetEntity.ToEntity<MkProductBasket>();

                var deal = basketEntity.Deal;

                if (deal != null)
                {
                    RecalculatePrice(wrapper.Service, deal.Id);
                }               

            }
            catch (Exception ex)
            {
                logger.ERROR("MKPlugins BasketCreationPlugin",
                              ex.ToString(), wrapper.TargetEntity.LogicalName, wrapper.TargetEntity.Id);
            }
        }

        private void RecalculatePrice(IOrganizationService service, Guid dealId)
        {
            var productBaskets = GetProductBasketsByDeal(service, dealId);

            var discount = productBaskets.Sum(x => x.GetAttributeValue<decimal>("mk_discount"));
            var basePrice = productBaskets.Sum(x => x.GetAttributeValue<decimal>("mk_base_price"));
            var priceAfterDiscount = productBaskets.Sum(x => x.GetAttributeValue<decimal>("mk_price_after_discount"));

            service.Update(new MkDeal
            { 
                Id = dealId,
                DiscountSum = discount,
                BasePriceSum = basePrice,                
                PriceAfterDiscountSum = priceAfterDiscount,
            }.ToEntity<Entity>());
        }

        private IEnumerable<Entity> GetProductBasketsByDeal(IOrganizationService service, Guid dealId)
        {
            var query = new QueryExpression("mk_product_basket")
            {
                NoLock = true,
                ColumnSet = new ColumnSet("mk_discount", "mk_base_price", "mk_price_after_discount"),
                Criteria = new FilterExpression(LogicalOperator.And)
                {
                    Conditions =
                    {
                        new ConditionExpression("mk_dealid", ConditionOperator.Equal, dealId),
                        new ConditionExpression("statecode", ConditionOperator.Equal, 0)
                    }
                }
            };

            return service.RetrieveMultiple(query).Entities.ToList<Entity>();
        }
    }
}
