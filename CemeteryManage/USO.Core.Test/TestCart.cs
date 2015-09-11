using NUnit.Framework;
using System;
using System.Data.Entity;
using USO.Domain;
using USO.Domain.Customer;
using USO.Infrastructure;
using USO.Infrastructure.Services.Customer;
namespace USO.Core.Test
{
    public class TestCart
    {
        [Test]
        public void TestInsertCart()
        {
            var usoEntities = new USOEntities();
            var cartDetail = new CartDetail { 
            
            };
        }
        public void TestInsertR3CustomerPlace()
        {
            var usoEntities = new USOEntities();
            var R3CustomerPlace = new R3CustomerPlace()
            {
                CustomerLegalId = 2,
                R3CustomerCode = "dddd",
                UnloadPlace = "dddddddd"
            };
            usoEntities.R3CustomerPlace.Add(R3CustomerPlace);
            usoEntities.SaveChanges();
        }
    }

}
