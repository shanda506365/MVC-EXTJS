using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using USO.Core;
using USO.Domain;
using USO.Dto;
using USO.Infrastructure.Mappers;

namespace USO.Infrastructure.Services
{
    public interface ICemeteryAreasService : IDependency
    {
        /// <summary>
        /// 查询墓碑区域
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        PagedResult<CemeteryAreasDTO> Find(CemeteryAreasQuery query);

        /// <summary>
        /// 新建墓碑区域
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        DataControlResult<CemeteryAreasDTO> Create(CemeteryAreasDTO csDto);

        /// <summary>
        /// 更新墓碑区域信息
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        DataControlResult<CemeteryAreasDTO> Update(CemeteryAreasDTO csDto);
        /// <summary>
        /// 删除墓碑区域
        /// </summary>
        /// <param name="csDtoList"></param>
        /// <returns></returns>
        DataControlResult<CemeteryAreasDTO> Delete(List<CemeteryAreasDTO> csDtoList);
    }

    public class CemeteryAreasService : ICemeteryAreasService
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly CemeteryAreasMapper _cemeteryAreasMapper;

        public CemeteryAreasService(IDatabaseContext databaseContext, CemeteryAreasMapper cemeteryAreasMapper)
        {
            _databaseContext = databaseContext;
            _cemeteryAreasMapper = cemeteryAreasMapper;
        }
        /// <summary>
        /// 查询墓碑区域
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public PagedResult<CemeteryAreasDTO> Find(CemeteryAreasQuery cemeteryAreasQuery)
        {
            Check.Argument.IsNotNull(cemeteryAreasQuery, "cemeteryAreasQuery");

            //Apply filtering    
            var query = _databaseContext.CemeteryAreas.Where(cemeteryAreasQuery);
            var total = query.Count();
            query = SortMemeberHelper.SortingAndPaging<CemeteryAreas>(query, cemeteryAreasQuery.sort, cemeteryAreasQuery.dir
                , cemeteryAreasQuery.page, cemeteryAreasQuery.limit);


            var resultSet = query.AsNoTracking().ToList().Select(r => _cemeteryAreasMapper.Map(r)).ToList();

            return new PagedResult<CemeteryAreasDTO>(resultSet, total);
        }
        /// <summary>
        /// 新建墓碑区域
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        public DataControlResult<CemeteryAreasDTO> Create(CemeteryAreasDTO csDto)
        {
            var result = new DataControlResult<CemeteryAreasDTO>();
            //判断是否为重复区域
            var repeat =
                  _databaseContext.CemeteryAreas.FirstOrDefault(a => a.Name == csDto.Name || a.Alias == csDto.Alias);
            if (repeat != null)
            {
                result.code = MyErrorCode.ResDBError;
                result.msg = "重复的区域名或别名编号";
                result.success = false;
                result.ResultOutDto = null;
                return result;
            }
            try
            {
                #region 赋值
                var cemeteryAreas = new CemeteryAreas
                {
                    Alias = csDto.Alias,
                    Name = csDto.Name,
                    Remark = csDto.Remark,
                    RowSort = csDto.RowSort
                };
                #endregion

                _databaseContext.CemeteryAreas.Add(cemeteryAreas);
                _databaseContext.SaveChanges();
                result.ResultOutDto = _cemeteryAreasMapper.Map(cemeteryAreas);
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
        /// 更新墓碑区域信息
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        public DataControlResult<CemeteryAreasDTO> Update(CemeteryAreasDTO csDto)
        {
            var result = new DataControlResult<CemeteryAreasDTO>();
            try
            {
                var cemeteryAreas = _databaseContext.CemeteryAreas.SingleOrDefault(n => n.Id == csDto.Id);
                if (cemeteryAreas == null)
                {
                    result.success = false;
                    result.msg = "该墓碑区域不存在";
                    result.code = MyErrorCode.ResParamError;
                    return result;
                }
                //判断是否为重复区域
                var repeat =
                      _databaseContext.CemeteryAreas.FirstOrDefault(a => a.Name == csDto.Name || a.Alias == csDto.Alias);
                if (repeat != null && repeat.Id != cemeteryAreas.Id)
                {
                    result.code = MyErrorCode.ResDBError;
                    result.msg = "重复的区域名或别名编号";
                    result.success = false;
                    result.ResultOutDto = null;
                    return result;
                }
                #region 赋值
                cemeteryAreas.Name = csDto.Name;
                cemeteryAreas.Alias = csDto.Alias;
                cemeteryAreas.Remark = csDto.Remark;
                cemeteryAreas.RowSort = csDto.RowSort;
                #endregion
                _databaseContext.SaveChanges();
                result.ResultOutDto = _cemeteryAreasMapper.Map(cemeteryAreas); ;
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
        /// 删除墓碑区域
        /// </summary>
        /// <param name="csDtoList"></param>
        /// <returns></returns>
        public DataControlResult<CemeteryAreasDTO> Delete(List<CemeteryAreasDTO> csDtoList)
        {
            var result = new DataControlResult<CemeteryAreasDTO>();
            var stopFlag = false;
            try
            {
                using (TransactionScope tsScope = new TransactionScope())
                {
                    foreach (var cemeteryAreasDto in csDtoList)
                    {

                        var cemeteryAreas =
                            _databaseContext.CemeteryAreas.SingleOrDefault(n => n.Id == cemeteryAreasDto.Id);
                        if (cemeteryAreas == null)
                        {
                            stopFlag = true;
                            result.success = false;
                            result.msg = "该墓碑区域不存在";
                            result.code = MyErrorCode.ResParamError;
                            break;
                        }
                        //首先删除该区域下所有的墓碑
                        var tombstones = _databaseContext.Tombstones.Where(n => n.AreaId == cemeteryAreasDto.Id);
                        foreach (var tombstone in tombstones)
                        {
                            _databaseContext.Tombstones.Remove(tombstone);
                        }
                        _databaseContext.CemeteryAreas.Remove(cemeteryAreas);
                    }

                    if (!stopFlag)
                    {
                        result.code = MyErrorCode.ResOK;
                        result.msg = string.Empty;
                        result.success = true;
                        _databaseContext.SaveChanges();
                        tsScope.Complete();
                    }
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
    }
}
