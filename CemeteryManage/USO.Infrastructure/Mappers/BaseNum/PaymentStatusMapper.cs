using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USO.Core;
using USO.Domain;
using USO.Dto;

namespace USO.Infrastructure.Mappers
{

    public interface IPaymentStatusMapper : IDependency
    {
        PaymentStatusDTO Map(PaymentStatus entity);
    }

    public class PaymentStatusMapper : IPaymentStatusMapper
    {
        private readonly IDatabaseContext _databaseContext;

        public PaymentStatusMapper(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public PaymentStatusDTO Map(PaymentStatus entity)
        {
            var dto = LoadEntityData(entity);
            return dto;
        }

        private PaymentStatusDTO LoadEntityData(PaymentStatus entity)
        {

            var myDto = new PaymentStatusDTO()
            {
                Id = entity.Id,
                Name = entity.Name,
                Alias = entity.Alias,
                Remark = entity.Remark
            };


            return myDto;
        }
    }
}
