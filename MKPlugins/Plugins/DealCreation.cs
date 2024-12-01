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

namespace MKPlugins.Deal
{
    public class DealCreation : PluginBase
    {
        public DealCreation()
        {
            Subscribe
                .ToMessage(CrmMessage.Update)
                .ForEntity(MkInterest.EntityLogicalName)
                .WithAnyField(MkInterest.Metadata.Status)
                .When(PluginStage.PostOperation)
                .Execute(Execute);
        }

        private void Execute(IContextWrapper wrapper)
        {
            var logger = new Logger(wrapper.Service);

            try
            {
                var interestTarget = wrapper.TargetEntity.ToEntity<MkInterest>();
                var statuscode = interestTarget.Status;

                //logger.INFO("DealCreation", statuscode, wrapper.TargetEntity.LogicalName, wrapper.TargetEntity.Id);

                if (statuscode.Value != MkInterest.Metadata.InterestStatusOptions.Согласие)
                {
                    return;
                }

                var interestEntity = wrapper.Service.Retrieve(interestTarget.LogicalName, interestTarget.Id, new ColumnSet(MkInterest.Metadata.Contact, MkInterest.Metadata.Territory)).ToEntity<MkInterest>();

                wrapper.Service.Create(new MkDeal
                {
                    Contact = interestEntity.Contact,
                    Territory = interestEntity.Territory
                }.ToEntity<Entity>());


            }
            catch (Exception ex)
            {
                logger.ERROR("MKPlugins DealCreation",
                              ex.ToString(), wrapper.TargetEntity.LogicalName, wrapper.TargetEntity.Id);
            }
        }
    }
}
