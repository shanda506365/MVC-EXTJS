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
    public class TombstoneService : ITombstoneService
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly TombstoneMapper _tombstoneMapper;
        private readonly ITombstoneBuriedPeopleMapMapper _tombstoneBuriedPeopleMapMapper;

        public TombstoneService(IDatabaseContext databaseContext, TombstoneMapper tombstoneMapper
            , ITombstoneBuriedPeopleMapMapper tombstoneBuriedPeopleMapMapper)
        {
            _databaseContext = databaseContext;
            _tombstoneMapper = tombstoneMapper;
            _tombstoneBuriedPeopleMapMapper = tombstoneBuriedPeopleMapMapper;
        }
        /// <summary>
        /// 查询墓碑
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public PagedResult<TombstoneDTO> Find(TombstoneQuery tombstoneQuery)
        {
            Check.Argument.IsNotNull(tombstoneQuery, "tombstoneQuery");

            //Apply filtering    
            var query = _databaseContext.Tombstones.Where(tombstoneQuery);
            if (tombstoneQuery.filter.Area != null)
            {
                query = query.Join(_databaseContext.CemeteryAreas.Where(a => a.Alias == tombstoneQuery.filter.Area.Alias.Substring(0, 3))
                    , tmb => tmb.AreaId, area => area.Id, (tmb, area) => tmb);
            }
            if (tombstoneQuery.filter.Row != null && tombstoneQuery.filter.Column.Alias.Length >= 5)
            {
                query = query.Join(_databaseContext.CemeteryRows.Where(a => a.Alias == tombstoneQuery.filter.Row.Alias.Substring(3, 2))
                    , tmb => tmb.RowId, row => row.Id, (tmb, row) => tmb);
            }
            if (tombstoneQuery.filter.Column != null && tombstoneQuery.filter.Column.Alias.Length == 7)
            {
                query = query.Join(_databaseContext.CemeteryColumns.Where(a => a.Alias == tombstoneQuery.filter.Column.Alias.Substring(5, 2))
                    , tmb => tmb.ColumnId, col => col.Id, (tmb, col) => tmb);
            }


            var total = query.Count();
            query = SortMemeberHelper.SortingAndPaging<Tombstone>(query, tombstoneQuery.sort, tombstoneQuery.dir
                , tombstoneQuery.page, tombstoneQuery.limit);


            var resultSet = query.AsNoTracking().ToList().Select(r => _tombstoneMapper.Map(r, true)).ToList();

            return new PagedResult<TombstoneDTO>(resultSet, total);
        }
        /// <summary>
        /// 新建墓碑
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        public DataControlResult<TombstoneDTO> Create(TombstoneDTO csDto)
        {
            var result = new DataControlResult<TombstoneDTO>();
            //判断该墓碑是否存在
            var repeat =
                  _databaseContext.Tombstones.FirstOrDefault(a => a.Name == csDto.Name);
            if (repeat != null)
            {
                result.code = MyErrorCode.ResDBError;
                result.msg = "该墓碑名称已存在";
                result.success = false;
                result.ResultOutDto = null;
                return result;
            }
            //判断该墓碑是否存在
            var repeatCol =
                  _databaseContext.Tombstones.FirstOrDefault(a => a.AreaId ==csDto.AreaId 
                      && a.RowId == csDto.RowId
                      && a.ColumnId == csDto.ColumnId);
            if (repeatCol != null)
            {
                result.code = MyErrorCode.ResDBError;
                result.msg = "该墓碑行号已存在";
                result.success = false;
                result.ResultOutDto = null;
                return result;
            }
            try
            {
                #region 赋值
                var tombstone = new Tombstone
                {
                    Name = csDto.Name,
                    Alias = csDto.Alias,
                    Remark = csDto.Remark,
                    CustomerName = csDto.CustomerName,
                    StoneText = csDto.StoneText,
                    Width = csDto.Width,
                    Height = csDto.Height,
                    Acreage = csDto.Width * csDto.Height,
                    Image = csDto.Image,
                    SortNum = csDto.SortNum,
                    SupperManage = 0,
                    ManageLimit = 0
                };

                if (csDto.ExpiryDate != null) tombstone.ExpiryDate = (DateTime)csDto.ExpiryDate;
                if (csDto.BuyDate != null) tombstone.BuyDate = (DateTime)csDto.BuyDate;
                if (csDto.LastPaymentDate != null) tombstone.LastPaymentDate = (DateTime)csDto.LastPaymentDate;
                if (csDto.BuryDate != null) tombstone.BuryDate = (DateTime)csDto.BuryDate;

                if (csDto.AreaId.HasValue && csDto.AreaId > 0)
                {
                    tombstone.AreaId = (int)csDto.AreaId;
                }
                if (csDto.RowId.HasValue && csDto.RowId > 0)
                {
                    tombstone.RowId = (int)csDto.RowId;
                }
                if (csDto.ColumnId.HasValue && csDto.ColumnId > 0)
                {
                    tombstone.ColumnId = (int)csDto.ColumnId;
                }
                if (csDto.CustomerId.HasValue && csDto.CustomerId > 0)
                {
                    tombstone.CustomerId = (int)csDto.CustomerId;
                }
                if (csDto.SecurityLevelId.HasValue && csDto.SecurityLevelId > 0)
                {
                    tombstone.SecurityLevelId = (int)csDto.SecurityLevelId;
                }
                if (csDto.ServiceLevelId.HasValue && csDto.ServiceLevelId > 0)
                {
                    tombstone.ServiceLevelId = (int)csDto.ServiceLevelId;
                }
                if (csDto.TypeId.HasValue && csDto.TypeId > 0)
                {
                    tombstone.TypeId = (int)csDto.TypeId;
                }
                if (csDto.PaymentStatusId.HasValue && csDto.PaymentStatusId > 0)
                {
                    tombstone.PaymentStatusId = (int)csDto.PaymentStatusId;
                }
                if (csDto.ParentId.HasValue && csDto.ParentId > 0)
                {
                    tombstone.ParentId = (int)csDto.ParentId;
                }
                #endregion

                _databaseContext.Tombstones.Add(tombstone);
                _databaseContext.SaveChanges();
                result.ResultOutDto = _tombstoneMapper.Map(tombstone, false);
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
        /// 批量添加墓碑
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="rowId"></param>
        /// <param name="typeId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataControlResult<TombstoneDTO> CreateList(int areaId, int rowId, int typeId, int count)
        {
            var result = new DataControlResult<TombstoneDTO>();
            var submitFlag = true;
            //获取当前区域，行所有的墓碑
            var hadTombstones = _databaseContext.Tombstones.Where(a => a.AreaId == areaId && a.RowId == rowId);
            var satrIndex = 1;

            if (hadTombstones.Any())
            {
                var hadTombstone = hadTombstones.Max(a => a.ColumnId);
                satrIndex = hadTombstone+1;
            }
            var endIndex = satrIndex + count;
            if (endIndex > 99)
            {
                endIndex = 99;
            }
            using (TransactionScope tsScope = new TransactionScope())
            {
                for (int i = satrIndex; i < endIndex; i++)
                {
                    var area = _databaseContext.CemeteryAreas.FirstOrDefault(a=>a.Id == areaId);
                    var dto = new TombstoneDTO
                        {
                            Name = area.Name + rowId + "排" + i+"号",
                            Alias = area.Name + rowId + "排" + i + "号",
                            AreaId = areaId,
                            RowId = rowId,
                            ColumnId = i,
                            SortNum = -1,
                            TypeId = typeId,
                            BuryDate = DateTime.Parse("1777-01-01"),
                            BuyDate = DateTime.Parse("1777-01-01"),
                            ExpiryDate = DateTime.Parse("1777-01-01"),
                            LastPaymentDate = DateTime.Parse("1777-01-01"),
                            SecurityLevelId = 1,
                            ServiceLevelId = 1,
                            PaymentStatusId = 1
                        };
                    var resultTemp = Create(dto);
                    result.ResultOutDtos.Add(resultTemp.ResultOutDto);
                    if (!resultTemp.success)
                    {
                        submitFlag = false;
                        result.code = MyErrorCode.ResDBError;
                        result.msg = "墓碑批量添加失败";
                        result.success = false;
                        result.ResultOutDto = null;
                    }
                }
                if (submitFlag)
                {
                    tsScope.Complete();
                    result.code = MyErrorCode.ResOK;
                    result.msg = string.Empty;
                    result.success = true;
                }
                //result.ResultOutDtos.Add(_tombstoneMapper.Map(tombstoneBuriedPeopleMap, false));
            }
            return result;
        }


        /// <summary>
        /// 更新墓碑信息
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        public DataControlResult<TombstoneDTO> Update(TombstoneDTO csDto)
        {
            var result = new DataControlResult<TombstoneDTO>();
            try
            {
                var tombstone = _databaseContext.Tombstones.SingleOrDefault(n => n.Id == csDto.Id);
                if (tombstone == null)
                {
                    result.success = false;
                    result.msg = "该墓碑不存在";
                    result.code = MyErrorCode.ResParamError;
                    return result;
                }
                //判断该墓碑是否存在
                var repeatCol =
                      _databaseContext.Tombstones.FirstOrDefault(a => a.AreaId == csDto.AreaId
                          && a.RowId == csDto.RowId
                          && a.ColumnId == csDto.ColumnId);
                if (repeatCol != null && repeatCol.Id != csDto.Id)
                {
                    result.code = MyErrorCode.ResDBError;
                    result.msg = "该墓碑行号已存在";
                    result.success = false;
                    result.ResultOutDto = null;
                    return result;
                }
                #region 赋值

                if (csDto.Name != null)
                {
                    tombstone.Name = csDto.Name;
                }
                if (csDto.Alias != null)
                {
                    tombstone.Alias = csDto.Alias;
                }
                if (csDto.Remark != null)
                {
                    tombstone.Remark = csDto.Remark;
                }
                if (csDto.CustomerName != null)
                {
                    tombstone.CustomerName = csDto.CustomerName;
                }
                if (csDto.StoneText != null)
                {
                    tombstone.StoneText = csDto.StoneText;
                }
                if (csDto.Image != null)
                {
                    tombstone.Image = csDto.Image;
                }
                tombstone.Width = csDto.Width;
                tombstone.Height = csDto.Height;
                tombstone.Acreage = csDto.Width * csDto.Height;
                tombstone.Image = csDto.Image;
                tombstone.SortNum = csDto.SortNum;

                if (csDto.ExpiryDate != null) tombstone.ExpiryDate = (DateTime)csDto.ExpiryDate;
                if (csDto.BuyDate != null) tombstone.BuyDate = (DateTime)csDto.BuyDate;
                if (csDto.LastPaymentDate != null) tombstone.LastPaymentDate = (DateTime)csDto.LastPaymentDate;
                if (csDto.BuryDate != null) tombstone.BuryDate = (DateTime)csDto.BuryDate;
                if (csDto.AreaId.HasValue && csDto.AreaId > 0)
                {
                    tombstone.AreaId = (int)csDto.AreaId;
                }
                if (csDto.RowId.HasValue && csDto.RowId > 0)
                {
                    tombstone.RowId = (int)csDto.RowId;
                }
                if (csDto.ColumnId.HasValue && csDto.ColumnId > 0)
                {
                    tombstone.ColumnId = (int)csDto.ColumnId;
                }
                if (csDto.CustomerId.HasValue && csDto.CustomerId > 0)
                {
                    tombstone.CustomerId = (int)csDto.CustomerId;
                }
                if (csDto.SecurityLevelId.HasValue && csDto.SecurityLevelId > 0)
                {
                    tombstone.SecurityLevelId = (int)csDto.SecurityLevelId;
                }
                if (csDto.ServiceLevelId.HasValue && csDto.ServiceLevelId > 0)
                {
                    tombstone.ServiceLevelId = (int)csDto.ServiceLevelId;
                }
                if (csDto.TypeId.HasValue && csDto.TypeId > 0)
                {
                    tombstone.TypeId = (int)csDto.TypeId;
                }
                if (csDto.PaymentStatusId.HasValue && csDto.PaymentStatusId > 0)
                {
                    tombstone.PaymentStatusId = (int)csDto.PaymentStatusId;
                }
                if (csDto.ParentId.HasValue && csDto.ParentId > 0)
                {
                    tombstone.ParentId = (int)csDto.ParentId;
                }

                #endregion
                _databaseContext.SaveChanges();
                result.ResultOutDto = _tombstoneMapper.Map(tombstone, false);
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
        /// 删除墓碑
        /// </summary>
        /// <param name="csDtoList"></param>
        /// <returns></returns>
        public DataControlResult<TombstoneDTO> Delete(List<TombstoneDTO> csDtoList)
        {
            var result = new DataControlResult<TombstoneDTO>();
            var stopFlag = false;
            try
            {
                foreach (var tombstoneDto in csDtoList)
                {
                    var tombstone = _databaseContext.Tombstones.SingleOrDefault(n => n.Id == tombstoneDto.Id);
                    if (tombstone == null)
                    {
                        stopFlag = true;
                        result.success = false;
                        result.msg = "该墓碑不存在";
                        result.code = MyErrorCode.ResParamError;
                        break;
                    }
                    _databaseContext.Tombstones.Remove(tombstone);
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
        /// 墓碑排序
        /// </summary>
        /// <param name="csDtoList"></param>
        /// <returns></returns>
        public DataControlResult<TombstoneDTO> SorTombstone(string[] idList)
        {
            var result = new DataControlResult<TombstoneDTO>();
            try
            {
                using (TransactionScope tsScope = new TransactionScope())
                {
                    var submitFlag = true;
                    for (int i = 0; i < idList.Length; i++)
                    {
                        TombstoneDTO dto = new TombstoneDTO
                            {
                                Id = int.Parse(idList[i]),
                                SortNum = i
                            };
                        var tempRl = Update(dto);
                        if (!tempRl.success)
                        {
                            submitFlag = false;
                            result = tempRl;
                        }
                    }
                    if (submitFlag)
                    {
                        tsScope.Complete();
                        result.code = MyErrorCode.ResOK;
                        result.msg = string.Empty;
                        result.success = true;
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


        /// <summary>
        /// 墓碑落葬
        /// </summary>
        /// <param name="tombstoneId"></param>
        /// <param name="customerIds"></param>
        /// <returns></returns>
        public DataControlResult<TombstoneBuriedPeopleMapDTO> BuryPeopleTombstone(int tombstoneId, List<int> customerIds)
        {
            var result = new DataControlResult<TombstoneBuriedPeopleMapDTO>();
            try
            {
                using (TransactionScope tsScope = new TransactionScope())
                {
                    var submitFlag = true;
                    foreach (var customerId in customerIds)
                    {
                        var tombstoneBuriedPeopleMap = new TombstoneBuriedPeopleMap
                        {
                            TombstoneId = tombstoneId,
                            BuriedCustomerId = customerId
                        };
                        //判断该墓碑是否存在落葬关系? 存在解除再落葬
                        var tombRepeats = _databaseContext.TombstoneBuriedPeopleMaps.Where(a => a.TombstoneId == tombstoneId);
                        foreach (var tombRepeat in tombRepeats)
                        {
                            _databaseContext.TombstoneBuriedPeopleMaps.Remove(tombRepeat);
                        }
                        //判断该客户是否存在落葬关系
                        var repeat =
                              _databaseContext.TombstoneBuriedPeopleMaps.FirstOrDefault(a => a.BuriedCustomerId == customerId);
                        if (repeat != null && repeat.TombstoneId != tombstoneId)
                        {
                            var customer = _databaseContext.Customers.FirstOrDefault(a => a.Id == customerId);
                            var fullName = customer != null ? customer.LastName + customer.MiddleName + customer.FirstName : "未知";
                            result.code = MyErrorCode.ResDBError;
                            result.msg = "客户：" + fullName + "(" + customerId + ")" + "已经落葬,如需更改请先解除落葬关系";
                            result.success = false;
                            result.ResultOutDto = null;
                            submitFlag = false;
                            break;
                        }
                        _databaseContext.TombstoneBuriedPeopleMaps.Add(tombstoneBuriedPeopleMap);
                        result.ResultOutDtos.Add(_tombstoneBuriedPeopleMapMapper.Map(tombstoneBuriedPeopleMap, false));
                    }

                    _databaseContext.SaveChanges();
                    if (submitFlag)
                    {
                        tsScope.Complete();
                        result.code = MyErrorCode.ResOK;
                        result.msg = string.Empty;
                        result.success = true;
                    }
                }

                return result;
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
        /// 墓碑解除落葬
        /// </summary>
        /// <param name="tombstoneId"></param>
        /// <param name="customerIds"></param>
        /// <returns></returns>
        public DataControlResult<TombstoneBuriedPeopleMapDTO> UnburyPeopleTombstone(int tombstoneId, List<int> customerIds)
        {
            var result = new DataControlResult<TombstoneBuriedPeopleMapDTO>();
            try
            {
                var tombstoneBuriedPeopleMaps =
                      _databaseContext.TombstoneBuriedPeopleMaps.Where(a => customerIds.Contains(a.BuriedCustomerId)
                      && a.TombstoneId == tombstoneId);
                foreach (var tombstoneBuriedPeopleMap in tombstoneBuriedPeopleMaps)
                {
                    _databaseContext.TombstoneBuriedPeopleMaps.Remove(tombstoneBuriedPeopleMap);
                    result.ResultOutDtos.Add(_tombstoneBuriedPeopleMapMapper.Map(tombstoneBuriedPeopleMap, false));
                }
                _databaseContext.SaveChanges();
                result.ResultOutDto = null;
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
        /// 墓碑预定
        /// </summary>
        /// <param name="tombstoneId"></param>
        /// <returns></returns>
        public DataControlResult<TombstoneDTO> OrderTombstone(int tombstoneId, DateTime lastPaymentDate)
        {
            return JobManageTombstone(tombstoneId, 2, lastPaymentDate,null,false);
        }

        /// <summary>
        /// 墓碑出售
        /// </summary>
        /// <param name="tombstoneId"></param>
        /// <returns></returns>
        public DataControlResult<TombstoneDTO> SaleTombstone(int tombstoneId, DateTime buyDate)
        {
            return JobManageTombstone(tombstoneId, 3, buyDate, null, false);
        }

        /// <summary>
        /// 墓碑落葬
        /// </summary>
        /// <param name="tombstoneId"></param>
        /// <returns></returns>
        public DataControlResult<TombstoneDTO> BuryTombstone(int tombstoneId, DateTime buryDate, int manageLimit,
            bool supperManage)
        {
            //判断墓碑是否为双人墓碑 状态是否为部分落葬
            var tombstone = _databaseContext.Tombstones.FirstOrDefault(a => a.Id == tombstoneId);
            var result = new DataControlResult<TombstoneDTO>();
            if (tombstone == null)
            {
                result.success = false;
                result.msg = "该墓碑不存在";
                result.code = MyErrorCode.ResParamError;
                return result;
            }
            if (tombstone.TypeId == 2)//双人墓
            {
                if (tombstone.PaymentStatusId == 4)//部分落葬
                {
                    return JobManageTombstone(tombstoneId, 5, buryDate, manageLimit, supperManage);
                }
                else
                {
                    return JobManageTombstone(tombstoneId, 4, buryDate, manageLimit, supperManage);
                }
            }
            else
            {
                return JobManageTombstone(tombstoneId, 5, buryDate, manageLimit, supperManage);
            }
            
        }

      

        /// <summary>
        /// 落葬、预定、出售等业务操作
        /// </summary>
        /// <param name="tombstoneId"></param>
        /// <param name="paymentStatusId"></param>
        /// <returns></returns>
        private DataControlResult<TombstoneDTO> JobManageTombstone(int tombstoneId,int paymentStatusId,DateTime date,
            int? manageLimit, bool supperManage)
        {
            var result = new DataControlResult<TombstoneDTO>();
            try
            {
                var tombstone = _databaseContext.Tombstones.SingleOrDefault(n => n.Id == tombstoneId);
                if (tombstone == null)
                {
                    result.success = false;
                    result.msg = "该墓碑不存在";
                    result.code = MyErrorCode.ResParamError;
                    return result;
                }

                #region 赋值

                tombstone.PaymentStatusId = paymentStatusId;
                
                switch (paymentStatusId)
                {
                    case 2:
                        //预定 设置补交时间
                        tombstone.LastPaymentDate = date;
                        break;
                    case 3:
                        //出售 设置出售时间
                        tombstone.BuyDate = date;
                        break;
                    case 5:
                        //落葬 设置落葬时间
                        tombstone.BuryDate = date;
                        tombstone.ManageLimit = manageLimit;
                        tombstone.SupperManage = supperManage ? 1 : 0;
                        break;
                }
              
                #endregion

                _databaseContext.SaveChanges();
                result.ResultOutDto = _tombstoneMapper.Map(tombstone, false);
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
        /// 更新墓碑管理费到期时间
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        public DataControlResult<TombstoneDTO> UpdateExpiryDate(TombstoneDTO csDto)
        {
            var result = new DataControlResult<TombstoneDTO>();
            try
            {
                var tombstone = _databaseContext.Tombstones.SingleOrDefault(n => n.Id == csDto.Id);
                if (tombstone == null)
                {
                    result.success = false;
                    result.msg = "该墓碑不存在";
                    result.code = MyErrorCode.ResParamError;
                    return result;
                }
                //判断该墓碑是否存在
                var repeatCol =
                      _databaseContext.Tombstones.FirstOrDefault(a => a.AreaId == csDto.AreaId
                          && a.RowId == csDto.RowId
                          && a.ColumnId == csDto.ColumnId);
                if (repeatCol != null && repeatCol.Id != csDto.Id)
                {
                    result.code = MyErrorCode.ResDBError;
                    result.msg = "该墓碑行号已存在";
                    result.success = false;
                    result.ResultOutDto = null;
                    return result;
                }
                #region 赋值
                if (csDto.ExpiryDate != null) tombstone.ExpiryDate = (DateTime)csDto.ExpiryDate;
                #endregion
                _databaseContext.SaveChanges();
                result.ResultOutDto = _tombstoneMapper.Map(tombstone, false);
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
        /// 续交管理费
        /// </summary>
        /// <param name="csDto"></param>
        /// <returns></returns>
        public DataControlResult<TombstoneDTO> RenewManageLimit(TombstoneDTO csDto)
        {
            var result = new DataControlResult<TombstoneDTO>();
            try
            {
                var tombstone = _databaseContext.Tombstones.SingleOrDefault(n => n.Id == csDto.Id);
                if (tombstone == null)
                {
                    result.success = false;
                    result.msg = "该墓碑不存在";
                    result.code = MyErrorCode.ResParamError;
                    return result;
                }
                //判断该墓碑是否存在
                var repeatCol =
                      _databaseContext.Tombstones.FirstOrDefault(a => a.AreaId == csDto.AreaId
                          && a.RowId == csDto.RowId
                          && a.ColumnId == csDto.ColumnId);
                if (repeatCol != null && repeatCol.Id != csDto.Id)
                {
                    result.code = MyErrorCode.ResDBError;
                    result.msg = "该墓碑行号已存在";
                    result.success = false;
                    result.ResultOutDto = null;
                    return result;
                }
                #region 赋值
                //if (csDto.ExpiryDate != null) tombstone.ExpiryDate = (DateTime)csDto.ExpiryDate;
                tombstone.ExpiryDate = tombstone.ExpiryDate.AddYears((int)csDto.ManageLimit);
                tombstone.ManageLimit += csDto.ManageLimit;
                
                #endregion
                _databaseContext.SaveChanges();
                result.ResultOutDto = _tombstoneMapper.Map(tombstone, false);
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
    }
}
