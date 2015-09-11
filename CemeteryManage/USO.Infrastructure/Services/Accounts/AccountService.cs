using USO.Dto.Accounts;
using USO.Infrastructure.Accounts;

namespace USO.Infrastructure.Activities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;
    using USO.Domain;
    using USO.Domain.Extensions;
    using USO.Dto;
    using USO.Dto.Activities;
    using USO.Infrastructure.Mappers;
    using USO.Infrastructure.Security;

    public class AccountService : IAccountService
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly IAccountMapper _accountMapper;
        private readonly IOrganizationsGetter _organizationsGetter;
        private readonly IMembershipService _membershipService;

        public AccountService(
            IDatabaseContext databaseContext,
            IAccountMapper accountMapper,
            IActivityFileMapper activityFileMapper,
            OrganizationsGetter organizationsGetter,
            IMembershipService membershipService
            )
        {
            _databaseContext = databaseContext;
            _accountMapper = accountMapper;
            _organizationsGetter = organizationsGetter;
            _membershipService = membershipService;
        }


        public PagedResult<AccountDTO> Find(AccountQuery accountQuery)
        {
            Check.Argument.IsNotNull(accountQuery, "accountQuery");

            //Apply filtering
            var query = _databaseContext.Accounts
                                        .Where(accountQuery);

            var total = query.Count();

            //Apply sorting
            #region sorting
            //if (!string.IsNullOrWhiteSpace(activityQuery.SortMember))
            //{
            //    if (activityQuery.SortDirection == ListSortDirection.Ascending)
            //    {
            //        switch (activityQuery.SortMember)
            //        {
            //            case "Id":
            //                query = query.OrderBy(u => u.Id);
            //                break;
            //            case "ParentId":
            //                query = query.OrderBy(u => u.ParentId);
            //                break;
            //            case "ActivityTypeString":
            //            case "ActivityType":
            //                query = query.OrderBy(u => u.ActivityType);
            //                break;
            //            case "RegardingObjectId":
            //                query = query.OrderBy(u => u.RegardingObjectId);
            //                break;
            //            case "RegardingObjectType":
            //                query = query.OrderBy(u => u.RegardingObjectType);
            //                break;
            //            case "RegardingObjectUrl":
            //                query = query.OrderBy(u => u.RegardingObjectUrl);
            //                break;
            //            case "PriorityString":
            //            case "Priority":
            //                query = query.OrderBy(u => u.Priority);
            //                break;
            //            case "QuoteOrOrderNumber":
            //                query = query.OrderBy(u => u.QuoteOrOrderNumber);
            //                break;
            //            case "AssignedOn":
            //                query = query.OrderBy(u => u.AssignedOn);
            //                break;
            //            case "DateDue":
            //                query = query.OrderBy(u => u.DateDue);
            //                break;
            //            case "StartedOn":
            //                query = query.OrderBy(u => u.StartedOn);
            //                break;
            //            case "CompletedOn":
            //                query = query.OrderBy(u => u.CompletedOn);
            //                break;
            //            case "EstimatedHours":
            //                query = query.OrderBy(u => u.EstimatedHours);
            //                break;
            //            case "ActualHours":
            //                query = query.OrderBy(u => u.ActualHours);
            //                break;
            //            case "Detail":
            //                query = query.OrderBy(u => u.Detail);
            //                break;
            //            case "Resolution":
            //                query = query.OrderBy(u => u.Resolution);
            //                break;
            //            case "EstimatedProgress":
            //                query = query.OrderBy(u => u.EstimatedProgress);
            //                break;
            //            case "ArticleUrl":
            //                query = query.OrderBy(u => u.ArticleUrl);
            //                break;
            //            case "OwningUser.Name":
            //            case "OwningUserId":
            //                query = query.OrderBy(u => u.OwningUserId);
            //                break;
            //            case "OrganizationId":
            //                query = query.OrderBy(u => u.OrganizationId);
            //                break;
            //            case "State":
            //                query = query.OrderBy(u => u.State);
            //                break;
            //            case "StatusString":
            //            case "Status":
            //                query = query.OrderBy(u => u.Status);
            //                break;
            //            case "PlaceV4":
            //                query = query.OrderBy(u => u.PlaceV4);
            //                break;
            //            case "CreatedByUser.Name":
            //            case "CreatedBy":
            //                query = query.OrderBy(u => u.CreatedBy);
            //                break;
            //            case "ModifiedByUser.Name":
            //            case "ModifiedBy":
            //                query = query.OrderBy(u => u.ModifiedBy);
            //                break;
            //            case "CreatedOn":
            //                query = query.OrderBy(u => u.CreatedOn);
            //                break;
            //            case "ModifiedOn":
            //                query = query.OrderBy(u => u.ModifiedOn);
            //                break;
            //            default:
            //                query = query.OrderBy(u => u.ModifiedOn);
            //                break;
            //        }
            //    }
            //    else
            //    {
            //        switch (activityQuery.SortMember)
            //        {
            //            case "Id":
            //                query = query.OrderByDescending(u => u.Id);
            //                break;
            //            case "ParentId":
            //                query = query.OrderByDescending(u => u.ParentId);
            //                break;
            //            case "ActivityTypeString":
            //            case "ActivityType":
            //                query = query.OrderByDescending(u => u.ActivityType);
            //                break;
            //            case "RegardingObjectId":
            //                query = query.OrderByDescending(u => u.RegardingObjectId);
            //                break;
            //            case "RegardingObjectType":
            //                query = query.OrderByDescending(u => u.RegardingObjectType);
            //                break;
            //            case "RegardingObjectUrl":
            //                query = query.OrderByDescending(u => u.RegardingObjectUrl);
            //                break;
            //            case "PriorityString":
            //            case "Priority":
            //                query = query.OrderByDescending(u => u.Priority);
            //                break;
            //            case "QuoteOrOrderNumber":
            //                query = query.OrderByDescending(u => u.QuoteOrOrderNumber);
            //                break;
            //            case "AssignedOn":
            //                query = query.OrderByDescending(u => u.AssignedOn);
            //                break;
            //            case "DateDue":
            //                query = query.OrderByDescending(u => u.DateDue);
            //                break;
            //            case "StartedOn":
            //                query = query.OrderByDescending(u => u.StartedOn);
            //                break;
            //            case "CompletedOn":
            //                query = query.OrderByDescending(u => u.CompletedOn);
            //                break;
            //            case "EstimatedHours":
            //                query = query.OrderByDescending(u => u.EstimatedHours);
            //                break;
            //            case "ActualHours":
            //                query = query.OrderByDescending(u => u.ActualHours);
            //                break;
            //            case "Detail":
            //                query = query.OrderByDescending(u => u.Detail);
            //                break;
            //            case "Resolution":
            //                query = query.OrderByDescending(u => u.Resolution);
            //                break;
            //            case "EstimatedProgress":
            //                query = query.OrderByDescending(u => u.EstimatedProgress);
            //                break;
            //            case "ArticleUrl":
            //                query = query.OrderByDescending(u => u.ArticleUrl);
            //                break;
            //            case "OwningUser.Name":
            //            case "OwningUserId":
            //                query = query.OrderByDescending(u => u.OwningUserId);
            //                break;
            //            case "OrganizationId":
            //                query = query.OrderByDescending(u => u.OrganizationId);
            //                break;
            //            case "State":
            //                query = query.OrderByDescending(u => u.State);
            //                break;
            //            case "StatusString":
            //            case "Status":
            //                query = query.OrderByDescending(u => u.Status);
            //                break;
            //            case "PlaceV4":
            //                query = query.OrderByDescending(u => u.PlaceV4);
            //                break;
            //            case "CreatedByUser.Name":
            //            case "CreatedBy":
            //                query = query.OrderByDescending(u => u.CreatedBy);
            //                break;
            //            case "ModifiedByUser.Name":
            //            case "ModifiedBy":
            //                query = query.OrderByDescending(u => u.ModifiedBy);
            //                break;
            //            case "CreatedOn":
            //                query = query.OrderByDescending(u => u.CreatedOn);
            //                break;
            //            case "ModifiedOn":
            //                query = query.OrderByDescending(u => u.ModifiedOn);
            //                break;
            //            default:
            //                query = query.OrderByDescending(u => u.ModifiedOn);
            //                break;
            //        }
            //    }
            //}
            //else
            //{
            //    query = query.OrderByDescending(r => r.ModifiedOn);
            //}
            #endregion
            query = query.OrderByDescending(r => r.ModifiedOn);
            //... and paging
            if (accountQuery.PageSize > 0)
            {
                query = query.Skip((accountQuery.Page - 1) * accountQuery.PageSize);
            }

            query = query.Take(accountQuery.PageSize);

            var resultSet = query.AsNoTracking().ToList().Select(r => _accountMapper.Map(r)).ToList();

            return new PagedResult<AccountDTO>(resultSet, total);
        }

       
    }
}