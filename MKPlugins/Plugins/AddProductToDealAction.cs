using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using AwaraIT.Training.Application.Core;
using System.Runtime.Remoting.Contexts;
using AwaraIT.Training.Domain.Models.Crm.Entities.mk;
using Microsoft.Xrm.Sdk.Query;

namespace MKPlugins.Plugins
{
    public class AddProductToDealAction : CodeActivity
    {
        [RequiredArgument]
        [Input("Deal")]
        [ReferenceTarget(MkDeal.EntityLogicalName)]
        public InArgument<EntityReference> Deal { get; set; }

        [RequiredArgument]
        [Input("Product")]
        [ReferenceTarget(MkProduct.EntityLogicalName)]
        public InArgument<EntityReference> Product { get; set; }

        [RequiredArgument]
        [Input("Discount")]
        public InArgument<decimal> Discount { get; set; }

        [Output("BasePrice")]
        public OutArgument<decimal> BasePrice { get; set; }

        [Output("PriceAfterDiscount")]
        public OutArgument<decimal> PriceAfterDiscount { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var workflowContext = context.GetExtension<IWorkflowContext>();
            var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(workflowContext.UserId);

            var dealId = Deal.Get(context).Id;
            var productId = Product.Get(context).Id;
            var discount = Discount.Get(context);

            var logger = new Logger(service);
            logger.INFO("AddProductToDealAction", dealId.ToString() + " " + productId.ToString() + " " + discount);

            var dealEntity = service.Retrieve(MkDeal.EntityLogicalName, dealId, new ColumnSet(MkDeal.Metadata.Territory)).ToEntity<MkDeal>();
            var productEntity = service.Retrieve(MkProduct.EntityLogicalName, productId, new ColumnSet(MkProduct.Metadata.PreparationFormat, MkProduct.Metadata.LessonFormat, MkProduct.Metadata.Subject)).ToEntity<MkProduct>();

            
            var query = new QueryExpression(MkPricelistPosition.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(MkPricelistPosition.Metadata.Price)
            };
            query.Criteria = new FilterExpression(LogicalOperator.And);
            query.Criteria.AddCondition(MkPricelistPosition.Metadata.PreparationFormat, ConditionOperator.Equal, productEntity.PreparationFormat.Value);
            query.Criteria.AddCondition(MkPricelistPosition.Metadata.LessonFormat, ConditionOperator.Equal, productEntity.LessonFormat.Value);
            query.Criteria.AddCondition(MkPricelistPosition.Metadata.Subject, ConditionOperator.Equal, productEntity.Subject.Value);
            query.Criteria.AddCondition(MkPricelistPosition.Metadata.Territory, ConditionOperator.Equal, dealEntity.Territory.Id);

            var priceListPosition = service.RetrieveMultiple(query).Entities.FirstOrDefault().ToEntity<MkPricelistPosition>();

            var price = priceListPosition.Price;
            var priceAfterDiscount = price - discount;

            logger.INFO("AddProductToDealAction", price.ToString() + " " + priceAfterDiscount.ToString());
            
            //BasePrice.Set(context, price);
            //PriceAfterDiscount.Set(context, priceAfterDiscount);

            BasePrice.Set(context, price);
            PriceAfterDiscount.Set(context, priceAfterDiscount);



        }
    }
}
