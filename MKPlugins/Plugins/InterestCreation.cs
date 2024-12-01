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

namespace MKPlugins.Interest
{
    public class InterestCreation : PluginBase
    {
        public InterestCreation()
        {
            Subscribe
                .ToMessage(CrmMessage.Create)
                .ForEntity(MkInterest.EntityLogicalName)
                .When(PluginStage.PreOperation)
                .Execute(Execute);
        }

        private void Execute(IContextWrapper wrapper)
        {
            var logger = new Logger(wrapper.Service);

            try
            {
                //logger.INFO("InterestCreation", "test", wrapper.TargetEntity.LogicalName, wrapper.TargetEntity.Id);

                var interestEntity = wrapper.TargetEntity.ToEntity<MkInterest>();
                var interestPhone = interestEntity.Phone;
                var interestEmail = interestEntity.Email;


                var contact = FindContactByPhoneAndEmail(wrapper.Service, interestPhone, interestEmail);
                Guid contactid;

                if (contact != null)
                {
                    contactid = contact.Id;
                }
                else
                {
                    contactid = wrapper.Service.Create(new MkContact
                    {
                        FirstName = interestEntity.FirstName,
                        LastName = interestEntity.LastName,
                        SurName = interestEntity.SurName,
                        Phone = interestEntity.Phone,
                        Email = interestEntity.Email,
                        Territory = interestEntity.Territory
                    }.ToEntity<Entity>());
                }

                interestEntity.Contact = new EntityReference(MkContact.EntityLogicalName, contactid);

                var userid = FindUserWithMinWorkload(wrapper.Service);
                //logger.INFO("InterestCreation", userid.ToString(), wrapper.TargetEntity.LogicalName, wrapper.TargetEntity.Id);
                interestEntity.OwnerId = new EntityReference("systemuser", userid);

            }
            catch (Exception ex)
            {
                logger.ERROR("MKPlugins InterestCreation",
                              ex.ToString(), wrapper.TargetEntity.LogicalName, wrapper.TargetEntity.Id);
            }
        }

        private Entity FindContactByPhoneAndEmail(IOrganizationService service, string interestPhone, string interestEmail)
        {
            var query = new QueryExpression(MkContact.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(false)
            };
            query.Criteria = new FilterExpression(LogicalOperator.And);
            query.Criteria.AddCondition(MkContact.Metadata.Phone, ConditionOperator.Equal, interestPhone);
            query.Criteria.AddCondition(MkContact.Metadata.Email, ConditionOperator.Equal, interestEmail);

            return service.RetrieveMultiple(query).Entities.FirstOrDefault();
        }

        public Guid FindUserWithMinWorkload(IOrganizationService service)
        {
            // Get Call Centre group users
            var groupSystemUsers = new QueryExpression("systemuser")
            {
                ColumnSet = new ColumnSet("systemuserid", "fullname")
            };
            var teamMembershipLink = groupSystemUsers.AddLink("teammembership", "systemuserid", "systemuserid");
            var teamLink = teamMembershipLink.AddLink("team", "teamid", "teamid");
            teamLink.LinkCriteria.AddCondition("name", ConditionOperator.Equal, "Колл-центр_мк");

            var groupUsers = service.RetrieveMultiple(groupSystemUsers).Entities.ToList<Entity>();
            
            Dictionary<Guid, int> callCentreGroupdict = new Dictionary<Guid, int>();

            foreach (var el in groupUsers)
            {
                callCentreGroupdict.Add(el.Id, 0);
            }

            var interestsInWork = new QueryExpression(MkInterest.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(MkInterest.Metadata.Owner)
            };
            interestsInWork.Criteria = new FilterExpression(LogicalOperator.And);
            interestsInWork.Criteria.AddCondition(MkInterest.Metadata.Status, ConditionOperator.Equal, (int)MkInterest.Metadata.InterestStatusOptions.В_работе);                      

            var interests = service.RetrieveMultiple(interestsInWork).Entities.ToList<Entity>();

            foreach (var el in interests)
            {
                var ownerId = el.GetAttributeValue<EntityReference>(MkInterest.Metadata.Owner).Id;
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
