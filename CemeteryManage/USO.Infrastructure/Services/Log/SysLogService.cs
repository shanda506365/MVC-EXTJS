using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using USO.Domain;
using USO.Dto;
using USO.Infrastructure.Mappers;

namespace USO.Infrastructure.Services
{
    public class SysLogService : ISysLogService
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly SysLogMapper _sysLogMapper;
        private readonly TombstoneMapper _tombstoneMapper;

        public SysLogService(IDatabaseContext databaseContext, SysLogMapper sysLogMapper,
            TombstoneMapper tombstoneMapper)
        {
            _databaseContext = databaseContext;
            _sysLogMapper = sysLogMapper;
            _tombstoneMapper = tombstoneMapper;
        }


        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="sysLogQuery"></param>
        /// <returns></returns>
        public PagedResult<SysLogDTO> Find(SysLogQuery sysLogQuery)
        {
            Check.Argument.IsNotNull(sysLogQuery, "sysLogQuery");

            //Apply filtering    
            var query = _databaseContext.SysLogs.Where(sysLogQuery);
            if (sysLogQuery.filter.User != null)
            {
                query = query.Join(_databaseContext.Users.Where(a => a.Name.Contains(sysLogQuery.filter.User.Name))
                    , log => log.UserId, user => user.Id, (log, user) => log);
            }

            var total = query.Count();
            query = SortMemeberHelper.SortingAndPaging<SysLog>(query, sysLogQuery.sort, sysLogQuery.dir
                , sysLogQuery.page, sysLogQuery.limit);


            var resultSet = query.AsNoTracking().ToList().Select(r => _sysLogMapper.Map(r)).ToList();

            return new PagedResult<SysLogDTO>(resultSet, total);
        }
        /// <summary>
        /// 新建日志
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public DataControlResult<SysLogDTO> Create(SysLogDTO dto)
        {
            var result = new DataControlResult<SysLogDTO>();
            try
            {
                #region 赋值
                var log = new SysLog
                {
                    Type = dto.Type,
                    ControlName = dto.ControlName,
                    Content = dto.Content,
                    UserId = dto.UserId,
                    Date = DateTime.Now
                };
                #region 业务日志写入
                if (!string.IsNullOrEmpty(dto.Applicanter))
                {
                    log.Applicanter = dto.Applicanter;
                }
                if (!string.IsNullOrEmpty(dto.BuryMan))
                {
                    log.BuryMan = dto.BuryMan;
                }
                if (dto.BuryDate.HasValue)
                {
                    log.BuryDate = dto.BuryDate;
                }
                if (!string.IsNullOrEmpty(dto.Telephone))
                {
                    log.Telephone = dto.Telephone;
                }
                if (!string.IsNullOrEmpty(dto.IDNumber))
                {
                    log.IDNumber = dto.IDNumber;
                }
                if (dto.Money.HasValue)
                {
                    log.Money = dto.Money;
                }
                if (dto.ControllTid.HasValue && dto.ControllTid.Value > 0)
                {
                    log.ControllTid = (int)dto.ControllTid;
                }
                if (!string.IsNullOrEmpty(dto.ControllIds))
                {
                    log.ControllIds = dto.ControllIds;
                }
                if (!string.IsNullOrEmpty(dto.Remark))
                {
                    log.Remark = dto.Remark;
                }
                if (!string.IsNullOrEmpty(dto.Remark2))
                {
                    log.Remark2 = dto.Remark2;
                }
                #endregion
                #endregion

                _databaseContext.SysLogs.Add(log);
                _databaseContext.SaveChanges();
                result.ResultOutDto = _sysLogMapper.Map(log);
                result.code = MyErrorCode.ResOK;
                result.msg = string.Empty;
                result.success = true;
            }
            catch (Exception ex)
            {
                result.code = MyErrorCode.ResDBError;
                result.msg = ex.Message;
                result.success = false;
                result.ResultOutDto = null;
                return result;
            }
            return result;
        }

        /// <summary>
        /// 更新日志信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public DataControlResult<SysLogDTO> Update(SysLogDTO dto)
        {
            var result = new DataControlResult<SysLogDTO>();
            try
            {
                var log = _databaseContext.SysLogs.SingleOrDefault(n => n.Id == dto.Id);
                if (log == null)
                {
                    result.success = false;
                    result.msg = "该日志不存在";
                    result.code = MyErrorCode.ResParamError;
                    return result;
                }
                #region 赋值

                if (dto.Type > 0)
                {
                    log.Type = dto.Type;
                }
                if (!string.IsNullOrEmpty(dto.ControlName))
                {
                    log.ControlName = dto.ControlName;
                }
                if (!string.IsNullOrEmpty(dto.Content))
                {
                    log.Content = dto.Content;
                }
                if (dto.UserId > 0)
                {
                    log.UserId = dto.UserId;
                }
                #region 业务日志写入
                if (!string.IsNullOrEmpty(dto.Applicanter))
                {
                    log.Applicanter = dto.Applicanter;
                }
                if (!string.IsNullOrEmpty(dto.BuryMan))
                {
                    log.BuryMan = dto.BuryMan;
                }
                if (!string.IsNullOrEmpty(dto.Telephone))
                {
                    log.Telephone = dto.Telephone;
                }
                if (!string.IsNullOrEmpty(dto.IDNumber))
                {
                    log.IDNumber = dto.IDNumber;
                }
                if (dto.Money.HasValue)
                {
                    log.Money = dto.Money;
                }
                if (dto.ControllTid.HasValue && dto.ControllTid.Value > 0)
                {
                    log.ControllTid = (int)dto.ControllTid;
                }
                if (!string.IsNullOrEmpty(dto.ControllIds))
                {
                    log.ControllIds = dto.ControllIds;
                }
                if (!string.IsNullOrEmpty(dto.BuryMan))
                {
                    log.BuryMan = dto.BuryMan;
                }
                if (dto.BuryDate.HasValue)
                {
                    log.BuryDate = dto.BuryDate;
                }
                if (!string.IsNullOrEmpty(dto.Remark))
                {
                    log.Remark = dto.Remark;
                }
                if (!string.IsNullOrEmpty(dto.Remark2))
                {
                    log.Remark2 = dto.Remark2;
                }
                #endregion
                #endregion
                _databaseContext.SaveChanges();
                result.code = MyErrorCode.ResOK;
                result.msg = string.Empty;
                result.success = true;
            }
            catch (Exception ex)
            {
                result.code = MyErrorCode.ResDBError;
                result.msg = ex.Message;
                result.success = false;
                return result;
            }
            return result;
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="dtoList"></param>
        /// <returns></returns>
        public DataControlResult<SysLogDTO> Delete(List<SysLogDTO> dtoList)
        {
            var result = new DataControlResult<SysLogDTO>();
            var stopFlag = false;
            try
            {
                foreach (var dto in dtoList)
                {
                    var log = _databaseContext.SysLogs.SingleOrDefault(n => n.Id == dto.Id);
                    if (log == null)
                    {
                        stopFlag = true;
                        result.success = false;
                        result.msg = "该日志不存在";
                        result.code = MyErrorCode.ResParamError;
                        break;
                    }
                    _databaseContext.SysLogs.Remove(log);
                }

                if (!stopFlag)
                {
                    result.code = MyErrorCode.ResOK;
                    result.msg = string.Empty;
                    result.success = true;
                    _databaseContext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                result.code = MyErrorCode.ResDBError;
                result.msg = ex.Message;
                result.success = false;
                return result;
            }
            return result;
        }


        /// <summary>
        /// 查询墓碑相关业务信息(不包括续交管理费)
        /// </summary>
        /// <param name="tombstoneId"></param>
        /// <param name="controlName"></param>
        /// <returns></returns>
        public PagedResult<SysLogDTO> GetJobInfoTombstone(int tombstoneId, string controlName)
        {
            var query = _databaseContext.SysLogs.Where(a => a.Type == LogType.JobManage && a.ControllTid == tombstoneId && a.ControlName != "续交管理费")
                .Join(_databaseContext.Tombstones, sys => sys.ControllTid, tomb => tomb.Id, (sys, tomb) => new SysLogDTO
                    {
                        Id = sys.Id,
                        Type = sys.Type,
                        ControlName = sys.ControlName,
                        Content = sys.Content,
                        UserId = sys.UserId,
                        Date = sys.Date,
                        Applicanter = sys.Applicanter,
                        Telephone = sys.Telephone,
                        IDNumber = sys.IDNumber,
                        Money = sys.Money,
                        ControllTid = sys.ControllTid,
                        ControllIds = sys.ControllIds,
                        ManageLimit = tomb.ManageLimit,
                        SupperManage = tomb.SupperManage,
                        LastPaymentDate = tomb.LastPaymentDate,
                        BuryMan = sys.BuryMan,
                        Remark = sys.Remark,
                        Remark2 = sys.Remark2
                    });
            if (controlName != "")
            {
                if (controlName.IndexOf('!') != -1)
                {
                    query = query.Where(a => !a.ControlName.Equals(controlName.Substring(1, controlName.Length)));
                }
                else
                {
                    query = query.Where(a => a.ControlName.Equals(controlName));
                }

            }

            var total = query.Count();
            query = query.OrderByDescending(a => a.Id);

            var resultSet = query.AsNoTracking().ToList();

            foreach (var sysLogDto in resultSet)
            {
                var tombstone = _databaseContext.Tombstones.First(a => a.Id == sysLogDto.ControllTid);
                sysLogDto.TombstoneEntity = _tombstoneMapper.Map(tombstone, false);
                if (!string.IsNullOrEmpty(sysLogDto.BuryMan))
                {
                    var maxManageDate = tombstone.ExpiryDate.ToString("yyyy-MM-dd");
                    var starDate = tombstone.ExpiryDate.AddYears(-(int)tombstone.ManageLimit).ToString("yyyy-MM-dd");
                    sysLogDto.ManageDate = starDate + "至" + maxManageDate;
                }

            }

            return new PagedResult<SysLogDTO>(resultSet, total);
        }


        /// <summary>
        /// 查询交管理费续交记录
        /// </summary>
        /// <param name="tombstoneId"></param>
        /// <param name="controlName"></param>
        /// <returns></returns>
        public PagedResult<SysLogDTO> GetRenewManageTombstone(int tombstoneId)
        {
            var query = _databaseContext.SysLogs.Where(a => a.Type == LogType.JobManage && a.ControllTid == tombstoneId && a.ControlName == "续交管理费")
                .Join(_databaseContext.Tombstones, sys => sys.ControllTid, tomb => tomb.Id, (sys, tomb) => new SysLogDTO
                {
                    Id = sys.Id,
                    Type = sys.Type,
                    ControlName = sys.ControlName,
                    Content = sys.Content,
                    UserId = sys.UserId,
                    Date = sys.Date,
                    Applicanter = sys.Applicanter,
                    Telephone = sys.Telephone,
                    IDNumber = sys.IDNumber,
                    Money = sys.Money,
                    ControllTid = sys.ControllTid,
                    ControllIds = sys.ControllIds,
                    ManageLimit = tomb.ManageLimit,
                    SupperManage = tomb.SupperManage,
                    LastPaymentDate = tomb.LastPaymentDate,
                    BuryMan = sys.BuryMan,
                    Remark = sys.Remark,
                    Remark2 = sys.Remark2
                });

            var total = query.Count();
            query = query.OrderByDescending(a => a.Id);

            var resultSet = query.AsNoTracking().ToList();
            var tombstone = _databaseContext.Tombstones.First(a => a.Id == tombstoneId);
            var tombstoneDto = _tombstoneMapper.Map(tombstone, false);
            var maxManageDate = tombstone.ExpiryDate.ToString("yyyy-MM-dd");
            var starDate = tombstone.ExpiryDate.AddYears(-(int)tombstone.ManageLimit).ToString("yyyy-MM-dd");
            //var starDate1 = tombstone.ExpiryDate.AddYears(-(int) tombstone.ManageLimit);
            int renewLimitTotal = 0;
            foreach (var sysLogDto in resultSet)
            {
                // var tombstone = _databaseContext.Tombstones.First(a => a.Id == sysLogDto.ControllTid);
                //sysLogDto.TombstoneEntity = _tombstoneMapper.Map(tombstone, false);
                sysLogDto.TombstoneEntity = tombstoneDto;
                if (!string.IsNullOrEmpty(sysLogDto.BuryMan))
                {
                    sysLogDto.ManageDate = starDate + "至" + maxManageDate;
                }
                int limit;
                try
                {
                     limit = int.Parse(sysLogDto.Content.Split(';')[1].Split('：')[1]);
                }
                catch
                {
                     limit = 0;
                }

                renewLimitTotal += limit;
            }
            //添加首次埋葬的时间记录 即是 当前到期时间-所有年限 = 首次落葬时间（管理费起始时间）
            // 当前墓碑管理年限-所有缴费记录年限 = 首次落葬设置的管理期限（管理费首次缴纳年限）
            int fistLimit = (int)tombstone.ManageLimit - renewLimitTotal;
            var fistDate = tombstone.ExpiryDate.AddYears((-(int)tombstone.ManageLimit+fistLimit)).ToString("yyyy-MM-dd");
            resultSet.Add(new SysLogDTO()
                {
                    Id = 0,
                    Date = DateTime.Parse(starDate),
                    Content = "首次缴纳管理费,续交管理费墓碑编号:" + tombstone.Id + ";续交年限：" + fistLimit + ";本次续交后到期日期：" + fistDate,
                    Remark = "首次缴纳管理费"
                });
            return new PagedResult<SysLogDTO>(resultSet, total);
        }
    }
}
