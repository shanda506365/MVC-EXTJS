using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USO.Core;
using USO.Domain;
using USO.Dto;

namespace USO.Infrastructure.Mappers
{
    public interface ISysLogMapper : IDependency
    {
        SysLogDTO Map(SysLog entity);
    }

    public class SysLogMapper : ISysLogMapper
    {
        private readonly IDatabaseContext _databaseContext;

        public SysLogMapper(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public SysLogDTO Map(SysLog entity)
        {
            var dto = LoadEntityData(entity);
            return dto;
        }

        public SysLogDTO Map(SysLog entity,int manageLimit,int supperManage)
        {
            var dto = LoadEntityData(entity);
            dto.ManageLimit = manageLimit;
            dto.SupperManage = supperManage;
            return dto;
        }

        private SysLogDTO LoadEntityData(SysLog entity)
        {

            var myDto = new SysLogDTO()
            {
                Id = entity.Id,
                Type = entity.Type,
                ControlName = entity.ControlName,
                Content = entity.Content,
                UserId = entity.UserId,
                Date = entity.Date,
                Applicanter = entity.Applicanter,
                Telephone = entity.Telephone,
                IDNumber = entity.IDNumber,
                Money = entity.Money,
                ControllTid = entity.ControllTid,
                ControllIds = entity.ControllIds,
                BuryMan = entity.BuryMan,
                BuryDate = entity.BuryDate,
                Remark = entity.Remark,
                Remark2 = entity.Remark2
            };

            if (entity.UserId > 0)
            {
                var user = _databaseContext.Users.AsNoTracking().FirstOrDefault(a => a.Id == entity.UserId);
                UserDTO userDto = null;
                if (user != null)
                {
                    userDto = new UserDTO
                    {
                        Id = user.Id,
                        Name = user.Name
                    };
                }
                myDto.UserEntity = userDto;
            }
            if (!string.IsNullOrEmpty(entity.BuryMan))
            {
                if (entity.ControllTid > 0)
                {
                    var tombstone = _databaseContext.Tombstones.AsNoTracking().FirstOrDefault(a => a.Id == entity.ControllTid);
                    if (tombstone != null)
                    {
                        myDto.ManageLimit = tombstone.ManageLimit;
                        myDto.SupperManage = tombstone.SupperManage;
                        var maxManageDate = tombstone.ExpiryDate.ToString("yyyy-MM-dd");
                        var starDate = tombstone.ExpiryDate.AddYears(-(int)tombstone.ManageLimit).ToString("yyyy-MM-dd");
                        myDto.ManageDate = starDate + "至" + maxManageDate;
                       
                    }
                }
                
            }

            return myDto;
        }
    }
}
