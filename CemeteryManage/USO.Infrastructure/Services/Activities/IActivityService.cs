
namespace USO.Infrastructure.Activities
{
    using System.Collections.Generic;
    using USO.Core;
    using USO.Domain;
    using USO.Dto;
    using USO.Dto.Activities;

    public interface IActivityService : IDependency
    {
        ServiceResult<ActivityDTO> CreateActivity(ActivityCreateParams activity);

        ServiceResult<ActivityDTO> UpdateActivity(ActivityEditParams activity);

        void DeleteActivity(int id);

        ServiceResult<ActivityDTO> GetActivity(int id);

        Activity GetActivityEntity(int id);

        IEnumerable<ActivityDTO> GetReplies(int parentId);

        PagedResult<ActivityDTO> Find(ActivityQuery query);

        void AddActivityFile(int activityId, ActivityFileDTO activityFile);

        void DeleteActivityFile(int fileId);

        ActivityFileDTO GetActivityFile(int fileId);

        ActivityFile GetActivityFileEntity(int fileId);

        IEnumerable<ActivityDTO> GetPastActivities(int userId);

        IEnumerable<ActivityDTO> GetNowActivities(int userId);

        IEnumerable<ActivityDTO> GetFutureActivities(int userId);

        ServiceResult<ActivityDTO> Hidden(int activityId, int userId);

        ServiceResult<ActivityDTO> Read(int activityId, int userId);
    }
}
