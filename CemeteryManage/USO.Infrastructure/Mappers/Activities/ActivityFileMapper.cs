
namespace USO.Infrastructure.Mappers
{
    using USO.Core;
    using USO.Domain;
    using USO.Dto.Activities;
    
    public interface IActivityFileMapper : IDependency 
    {
        ActivityFileDTO Map(ActivityFile entity);
    }
    
    public class ActivityFileMapper : IActivityFileMapper
    {
        public ActivityFileMapper()
        {
        }

        public ActivityFileDTO Map(ActivityFile entity)
        {
            return new ActivityFileDTO
                          {
                                Id = entity.Id,      
    		                    ActivityId = entity.ActivityId,      
    		                    PathUrl = entity.PathUrl,      
    		                    FileSize = entity.FileSize,      
    		                    FileName = entity.FileName,      
    		                    MimeType = entity.MimeType,      
    		                    Content = entity.Content,      
    		                    CreatedOn = entity.CreatedOn
                          };
    	 }
    }
}
