//using NUnit.Framework;
//using SAP.Middleware.Connector;
//using System;

//namespace USO.Core.Test
//{
//    [TestFixture]
//    public class TestSAP
//    {
//        [Test]
//        public void TestSAPConnect()
//        {
//            nco();
//        }


//        public void nco()
//        {
//            IDestinationConfiguration ID = new MyBackendConfig();
//            RfcDestinationManager.RegisterDestinationConfiguration(ID);
//            RfcDestination prd = RfcDestinationManager.GetDestination("R3-UPQ");
//            RfcDestinationManager.UnregisterDestinationConfiguration(ID);
//            nco(prd);

//        }

//        public void nco(RfcDestination prd)
//        {

//            RfcRepository repo = prd.Repository;

//            IRfcFunction companyBapi = repo.CreateFunction("ZRFC_READ_T171T"); //调用函数名

//            ////companyBapi.SetValue("XXXX", MATNR); //设置Import的参数

//            companyBapi.Invoke(prd); //执行函数
//            IRfcTable table = companyBapi.GetTable("t171t");  //获取相应的表

//            //string MAKTX = companyBapi.GetValue("BZIRK").ToString();  //获取品名
            
//            foreach (var item in table)
//            {
//                Console.WriteLine(item[1]);
//            }

//            //string customerTable = companyBapi.GetValue("KNVV").ToString(); //hu
//            //DataTable dt = new DataTable(); //新建表格
//            //dt.Columns.Add("品号"); //表格添加一列
//            //for (int i = 0; i < table.RowCount; i++)
//            //{
//            //    table.CurrentIndex = i; //当前内表的索引行
//            //    DataRow dr = dt.NewRow();
//            //    dr[0] = table.GetString("MATNR"); //获取表格的某行某列的值
//            //    dt.Rows.Add(dr); //填充该表格的值
//            //}
//        }

//    }


//    public class MyBackendConfig : IDestinationConfiguration
//    {
//        public RfcConfigParameters GetParameters(String destinationName)
//        {

//            if ("R3-UPQ".Equals(destinationName))
//            {
//                var parms = new RfcConfigParameters();
//                parms.Add(RfcConfigParameters.AppServerHost, "10.3.1.224");   //SAP主机IP
//                parms.Add(RfcConfigParameters.SystemNumber, "01");  //SAP实例
//                parms.Add(RfcConfigParameters.User, "CH-107832");  //用户名
//                parms.Add(RfcConfigParameters.Password, "abcd1234");  //密码
//                parms.Add(RfcConfigParameters.Client, "800");  // Client
//                parms.Add(RfcConfigParameters.Language, "ZH");  //登陆语言
//                parms.Add(RfcConfigParameters.PoolSize, "5");
//                parms.Add(RfcConfigParameters.MaxPoolSize, "10");
//                parms.Add(RfcConfigParameters.IdleTimeout, "60");
//                return parms;
//            }
//            else return null;
//        }

//        public bool ChangeEventsSupported()
//        {
//            return false;
//        }

//        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;
//    }
//}
