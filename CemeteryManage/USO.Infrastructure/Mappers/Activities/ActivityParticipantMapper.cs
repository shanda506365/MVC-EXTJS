
namespace USO.Infrastructure.Mappers
{
    using System.Linq;
    using USO.Core;
    using USO.Domain;
    using USO.Dto.Activities;
    using USO.Infrastructure.Security;
    
    public interface IActivityParticipantMapper : IDependency {
        ActivityParticipantDTO Map(ActivityParticipant entity);
    }
    
    public class ActivityParticipantMapper : IActivityParticipantMapper
    {
        private readonly IMembershipService _membershipService;
        private readonly IOrganizationService _orgService;

        public ActivityParticipantMapper(
            IMembershipService membershipService,
            IOrganizationService orgService
            )
        {
            this._membershipService = membershipService;
            this._orgService = orgService;
        }

        public ActivityParticipantDTO Map(ActivityParticipant entity)
        {
            var dto = new ActivityParticipantDTO();

    		dto.Id = entity.Id;      
    		dto.ActivityId = entity.ActivityId;      
    		dto.ParticipantId = entity.ParticipantId;      
    		dto.OperateType = entity.OperateType;      
            //dto.IsHidden = entity.IsHidden;

            if (entity.ParticipantId > 0)
            {
                if (dto.OperateType == ActivityParticipantOperateType.Organization)
                {
                    var organization = _orgService.All(true).FirstOrDefault(o => o.Id == entity.ParticipantId);
                    if (organization != null)
                        dto.ParticipantName = organization.Name;
                }
                else
                {
                    var user = _membershipService.GetUserEntity(entity.ParticipantId, true);
                    if (user != null)
                        dto.ParticipantName = user.Name;
                }
            }

            return dto;
    	 }
    }
}
