﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.17929
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace USO.Infrastructure.R3UpdateOrderWebService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:sap-com:document:sap:soap:functions:mc-style", ConfigurationName="R3UpdateOrderWebService.ZWS_ZINT_SD0033")]
    public interface ZWS_ZINT_SD0033 {
        
        // CODEGEN: 操作 ZintSd0033 以后生成的消息协定不是 RPC，也不是换行文档。
        [System.ServiceModel.OperationContractAttribute(Action="urn:sap-com:document:sap:soap:functions:mc-style:ZWS_ZINT_SD0033:ZintSd0033Reques" +
            "t", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033Response1 ZintSd0033(USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033Request request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:sap-com:document:sap:soap:functions:mc-style:ZWS_ZINT_SD0033:ZintSd0033Reques" +
            "t", ReplyAction="*")]
        System.Threading.Tasks.Task<USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033Response1> ZintSd0033Async(USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033Request request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:sap-com:document:sap:soap:functions:mc-style")]
    public partial class ZintSd0033 : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string vbelnField;
        
        private Zchanso[] zchansoField;
        
        private Zxgxsdhed zxgheadField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string Vbeln {
            get {
                return this.vbelnField;
            }
            set {
                this.vbelnField = value;
                this.RaisePropertyChanged("Vbeln");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public Zchanso[] Zchanso {
            get {
                return this.zchansoField;
            }
            set {
                this.zchansoField = value;
                this.RaisePropertyChanged("Zchanso");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public Zxgxsdhed Zxghead {
            get {
                return this.zxgheadField;
            }
            set {
                this.zxgheadField = value;
                this.RaisePropertyChanged("Zxghead");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:soap:functions:mc-style")]
    public partial class Zchanso : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string itmNumberField;
        
        private string storeLocField;
        
        private decimal reqQtyField;
        
        private string plantField;
        
        private decimal unitPriceField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string ItmNumber {
            get {
                return this.itmNumberField;
            }
            set {
                this.itmNumberField = value;
                this.RaisePropertyChanged("ItmNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string StoreLoc {
            get {
                return this.storeLocField;
            }
            set {
                this.storeLocField = value;
                this.RaisePropertyChanged("StoreLoc");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public decimal ReqQty {
            get {
                return this.reqQtyField;
            }
            set {
                this.reqQtyField = value;
                this.RaisePropertyChanged("ReqQty");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string Plant {
            get {
                return this.plantField;
            }
            set {
                this.plantField = value;
                this.RaisePropertyChanged("Plant");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public decimal UnitPrice {
            get {
                return this.unitPriceField;
            }
            set {
                this.unitPriceField = value;
                this.RaisePropertyChanged("UnitPrice");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:soap:functions:mc-style")]
    public partial class Zxgxsdhed : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string partnNumbField;
        
        private string partnNameField;
        
        private string stcegField;
        
        private string partnAddrField;
        
        private string telNumberField;
        
        private string bankNameField;
        
        private string bankAccountField;
        
        private string dlvschduseField;
        
        private string cityField;
        
        private string billBlockField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string PartnNumb {
            get {
                return this.partnNumbField;
            }
            set {
                this.partnNumbField = value;
                this.RaisePropertyChanged("PartnNumb");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string PartnName {
            get {
                return this.partnNameField;
            }
            set {
                this.partnNameField = value;
                this.RaisePropertyChanged("PartnName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string Stceg {
            get {
                return this.stcegField;
            }
            set {
                this.stcegField = value;
                this.RaisePropertyChanged("Stceg");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string PartnAddr {
            get {
                return this.partnAddrField;
            }
            set {
                this.partnAddrField = value;
                this.RaisePropertyChanged("PartnAddr");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string TelNumber {
            get {
                return this.telNumberField;
            }
            set {
                this.telNumberField = value;
                this.RaisePropertyChanged("TelNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string BankName {
            get {
                return this.bankNameField;
            }
            set {
                this.bankNameField = value;
                this.RaisePropertyChanged("BankName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string BankAccount {
            get {
                return this.bankAccountField;
            }
            set {
                this.bankAccountField = value;
                this.RaisePropertyChanged("BankAccount");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string Dlvschduse {
            get {
                return this.dlvschduseField;
            }
            set {
                this.dlvschduseField = value;
                this.RaisePropertyChanged("Dlvschduse");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string City {
            get {
                return this.cityField;
            }
            set {
                this.cityField = value;
                this.RaisePropertyChanged("City");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public string BillBlock {
            get {
                return this.billBlockField;
            }
            set {
                this.billBlockField = value;
                this.RaisePropertyChanged("BillBlock");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:sap-com:document:sap:soap:functions:mc-style")]
    public partial class ZintSd0033Response : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string returnidField;
        
        private string returnmsgField;
        
        private Zchanso[] zchansoField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string Returnid {
            get {
                return this.returnidField;
            }
            set {
                this.returnidField = value;
                this.RaisePropertyChanged("Returnid");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string Returnmsg {
            get {
                return this.returnmsgField;
            }
            set {
                this.returnmsgField = value;
                this.RaisePropertyChanged("Returnmsg");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public Zchanso[] Zchanso {
            get {
                return this.zchansoField;
            }
            set {
                this.zchansoField = value;
                this.RaisePropertyChanged("Zchanso");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ZintSd0033Request {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:soap:functions:mc-style", Order=0)]
        public USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033 ZintSd0033;
        
        public ZintSd0033Request() {
        }
        
        public ZintSd0033Request(USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033 ZintSd0033) {
            this.ZintSd0033 = ZintSd0033;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ZintSd0033Response1 {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:sap-com:document:sap:soap:functions:mc-style", Order=0)]
        public USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033Response ZintSd0033Response;
        
        public ZintSd0033Response1() {
        }
        
        public ZintSd0033Response1(USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033Response ZintSd0033Response) {
            this.ZintSd0033Response = ZintSd0033Response;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ZWS_ZINT_SD0033Channel : USO.Infrastructure.R3UpdateOrderWebService.ZWS_ZINT_SD0033, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ZWS_ZINT_SD0033Client : System.ServiceModel.ClientBase<USO.Infrastructure.R3UpdateOrderWebService.ZWS_ZINT_SD0033>, USO.Infrastructure.R3UpdateOrderWebService.ZWS_ZINT_SD0033 {
        
        public ZWS_ZINT_SD0033Client() {
        }
        
        public ZWS_ZINT_SD0033Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ZWS_ZINT_SD0033Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ZWS_ZINT_SD0033Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ZWS_ZINT_SD0033Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033Response1 USO.Infrastructure.R3UpdateOrderWebService.ZWS_ZINT_SD0033.ZintSd0033(USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033Request request) {
            return base.Channel.ZintSd0033(request);
        }
        
        public USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033Response ZintSd0033(USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033 ZintSd00331) {
            USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033Request inValue = new USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033Request();
            inValue.ZintSd0033 = ZintSd00331;
            USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033Response1 retVal = ((USO.Infrastructure.R3UpdateOrderWebService.ZWS_ZINT_SD0033)(this)).ZintSd0033(inValue);
            return retVal.ZintSd0033Response;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033Response1> USO.Infrastructure.R3UpdateOrderWebService.ZWS_ZINT_SD0033.ZintSd0033Async(USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033Request request) {
            return base.Channel.ZintSd0033Async(request);
        }
        
        public System.Threading.Tasks.Task<USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033Response1> ZintSd0033Async(USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033 ZintSd0033) {
            USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033Request inValue = new USO.Infrastructure.R3UpdateOrderWebService.ZintSd0033Request();
            inValue.ZintSd0033 = ZintSd0033;
            return ((USO.Infrastructure.R3UpdateOrderWebService.ZWS_ZINT_SD0033)(this)).ZintSd0033Async(inValue);
        }
    }
}
