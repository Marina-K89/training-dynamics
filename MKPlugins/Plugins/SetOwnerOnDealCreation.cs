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
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace MKPlugins.Plugins
{
    public class SetOwnerOnDealCreation: PluginBase
    {
        public SetOwnerOnDealCreation()
        {
            Subscribe
                .ToMessage(CrmMessage.Create)
                .ForEntity(MkDeal.EntityLogicalName)                
                .When(PluginStage.PreOperation)
                .Execute(Execute);
        }

        private void Execute(IContextWrapper wrapper)
        {
            var logger = new Logger(wrapper.Service);

            try
            {
                //logger.INFO("SetOwnerOnDealCreation", "Start", wrapper.TargetEntity.LogicalName, wrapper.TargetEntity.Id);

                var dealEntity = wrapper.TargetEntity.ToEntity<MkDeal>();
                var territory = dealEntity.Territory;

                Guid workingGroupId = Guid.Empty;

                switch (territory.Id.ToString())
                {
                    case "a1fd740a-71a7-ef11-8a6a-000d3a5c09a6": // Армения 
                        workingGroupId = Guid.Parse("2e1c2966-72aa-ef11-b8e9-000d3a5c09a6");
                        break;
                    case "1c6d3004-71a7-ef11-8a6a-000d3a5c09a6": // Грузия
                        workingGroupId = Guid.Parse("2e81118c-77aa-ef11-b8e9-000d3a5c09a6");
                        break;
                    case "21d791f7-70a7-ef11-8a6a-000d3a5c09a6": // Казахстан 
                        workingGroupId = Guid.Parse("4eeaf2cb-72aa-ef11-b8e9-000d3a5c09a6");
                        break;
                    case "c454c3fd-70a7-ef11-8a6a-000d3a5c09a6": // Россия 
                        workingGroupId = Guid.Parse("a0961d03-73aa-ef11-b8e9-000d3a5c09a6");
                        break;
                    default: //Армения
                        workingGroupId = Guid.Parse("2e1c2966-72aa-ef11-b8e9-000d3a5c09a6");
                        break;
                }

                //logger.INFO("SetOwnerOnDealCreation - workingGroupId", workingGroupId.ToString(), wrapper.TargetEntity.LogicalName, wrapper.TargetEntity.Id);

                Guid userid = FindUserWithMinWorkload(wrapper.Service, workingGroupId);

                //logger.INFO("SetOwnerOnDealCreation - userid", userid.ToString(), wrapper.TargetEntity.LogicalName, wrapper.TargetEntity.Id);

                dealEntity.OwnerId = new EntityReference("systemuser", userid);


            }
            catch (Exception ex)
            {
                logger.ERROR("MKPlugins SetOwnerOnDealCreation",
                              ex.ToString(), wrapper.TargetEntity.LogicalName, wrapper.TargetEntity.Id);
            }

        }

        public Guid FindUserWithMinWorkload(IOrganizationService service, Guid workingGroupId)
        {
            // Get workingGroupId group users
            var groupSystemUsers = new QueryExpression("systemuser")
            {
                ColumnSet = new ColumnSet("systemuserid", "fullname")
            };
            var teamMembershipLink = groupSystemUsers.AddLink("teammembership", "systemuserid", "systemuserid");
            var teamLink = teamMembershipLink.AddLink("team", "teamid", "teamid");
            teamLink.LinkCriteria.AddCondition("teamid", ConditionOperator.Equal, workingGroupId);

            var groupUsers = service.RetrieveMultiple(groupSystemUsers).Entities.ToList<Entity>();

            Dictionary<Guid, int> callCentreGroupdict = new Dictionary<Guid, int>();

            foreach (var el in groupUsers)
            {
                callCentreGroupdict.Add(el.Id, 0);
            }

            var dealsInWork = new QueryExpression(MkDeal.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(MkDeal.Metadata.Owner)
            };
            dealsInWork.Criteria = new FilterExpression(LogicalOperator.And);
            dealsInWork.Criteria.AddCondition(MkDeal.Metadata.Status, ConditionOperator.Equal, (int)MkDeal.Metadata.DealStatusOptions.В_работе);

            var interests = service.RetrieveMultiple(dealsInWork).Entities.ToList<Entity>();

            foreach (var el in interests)
            {
                var ownerId = el.GetAttributeValue<EntityReference>(MkDeal.Metadata.Owner).Id;
                if (callCentreGroupdict.ContainsKey(ownerId))
                {
                    callCentreGroupdict[ownerId] += 1;
                }
            }

            var minInterestCount = callCentreGroupdict.Min(x => x.Value);

            var userId = callCentreGroupdict.Where(x => x.Value == minInterestCount).FirstOrDefault().Key;

            return userId;
            
        }

    }
}
