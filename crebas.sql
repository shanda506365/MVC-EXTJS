/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     2014/7/21 17:22:19                           */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Customer') and o.name = 'FK_CUSTOMER_REFERENCE_NATIONAL')
alter table Customer
   drop constraint FK_CUSTOMER_REFERENCE_NATIONAL
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Customer') and o.name = 'FK_CUSTOMER_REFERENCE_CUSTOMER')
alter table Customer
   drop constraint FK_CUSTOMER_REFERENCE_CUSTOMER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Customer') and o.name = 'FK_CUSTOMER_REFERENCE_CUSTOMER123')
alter table Customer
   drop constraint FK_CUSTOMER_REFERENCE_CUSTOMER123
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('RoleFunctionMap') and o.name = 'FK_ROLEFUNC_REFERENCE_ROLE')
alter table RoleFunctionMap
   drop constraint FK_ROLEFUNC_REFERENCE_ROLE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('RoleFunctionMap') and o.name = 'FK_ROLEFUNC_REFERENCE_FUNCTION')
alter table RoleFunctionMap
   drop constraint FK_ROLEFUNC_REFERENCE_FUNCTION
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SysLog') and o.name = 'FK_SYSLOG_REFERENCE_USER')
alter table SysLog
   drop constraint FK_SYSLOG_REFERENCE_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Tombstone') and o.name = 'FK_TOMBSTON_REFERENCE_CEMETERY_AREA')
alter table Tombstone
   drop constraint FK_TOMBSTON_REFERENCE_CEMETERY_AREA
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Tombstone') and o.name = 'FK_TOMBSTON_REFERENCE_CEMETERY')
alter table Tombstone
   drop constraint FK_TOMBSTON_REFERENCE_CEMETERY
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Tombstone') and o.name = 'FK_TOMBSTON_REFERENCE_CEMETERY_ROW')
alter table Tombstone
   drop constraint FK_TOMBSTON_REFERENCE_CEMETERY_ROW
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Tombstone') and o.name = 'FK_TOMBSTON_REFERENCE_SECURITY')
alter table Tombstone
   drop constraint FK_TOMBSTON_REFERENCE_SECURITY
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Tombstone') and o.name = 'FK_TOMBSTON_REFERENCE_SERVICEL')
alter table Tombstone
   drop constraint FK_TOMBSTON_REFERENCE_SERVICEL
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Tombstone') and o.name = 'FK_TOMBSTON_TOMBSTONE_TOMBSTON')
alter table Tombstone
   drop constraint FK_TOMBSTON_TOMBSTONE_TOMBSTON
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Tombstone') and o.name = 'FK_TOMBSTON_TOMBSTONE_PAYMENTS')
alter table Tombstone
   drop constraint FK_TOMBSTON_TOMBSTONE_PAYMENTS
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TombstoneBuriedPeopleMap') and o.name = 'FK_TOMBSTON_REFERENCE_CUSTOMER13')
alter table TombstoneBuriedPeopleMap
   drop constraint FK_TOMBSTON_REFERENCE_CUSTOMER13
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TombstoneBuriedPeopleMap') and o.name = 'FK_TOMBSTON_REFERENCE_TOMBSTON')
alter table TombstoneBuriedPeopleMap
   drop constraint FK_TOMBSTON_REFERENCE_TOMBSTON
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('"User"') and o.name = 'FK_USER_REFERENCE_DEPARTME')
alter table "User"
   drop constraint FK_USER_REFERENCE_DEPARTME
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('UserRoleMap') and o.name = 'FK_USERROLE_REFERENCE_USER')
alter table UserRoleMap
   drop constraint FK_USERROLE_REFERENCE_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('UserRoleMap') and o.name = 'FK_USERROLE_REFERENCE_ROLE')
alter table UserRoleMap
   drop constraint FK_USERROLE_REFERENCE_ROLE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('CemeteryAreas')
            and   type = 'U')
   drop table CemeteryAreas
go

if exists (select 1
            from  sysobjects
           where  id = object_id('CemeteryColumns')
            and   type = 'U')
   drop table CemeteryColumns
go

if exists (select 1
            from  sysobjects
           where  id = object_id('CemeteryRows')
            and   type = 'U')
   drop table CemeteryRows
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Customer')
            and   type = 'U')
   drop table Customer
go

if exists (select 1
            from  sysobjects
           where  id = object_id('CustomerStatus')
            and   type = 'U')
   drop table CustomerStatus
go

if exists (select 1
            from  sysobjects
           where  id = object_id('CustomerType')
            and   type = 'U')
   drop table CustomerType
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Department')
            and   type = 'U')
   drop table Department
go

if exists (select 1
            from  sysobjects
           where  id = object_id('"Function"')
            and   type = 'U')
   drop table "Function"
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Nationality')
            and   type = 'U')
   drop table Nationality
go

if exists (select 1
            from  sysobjects
           where  id = object_id('PaymentStatus')
            and   type = 'U')
   drop table PaymentStatus
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Role')
            and   type = 'U')
   drop table Role
go

if exists (select 1
            from  sysobjects
           where  id = object_id('RoleFunctionMap')
            and   type = 'U')
   drop table RoleFunctionMap
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SecurityLevel')
            and   type = 'U')
   drop table SecurityLevel
go

if exists (select 1
            from  sysobjects
           where  id = object_id('ServiceLevel')
            and   type = 'U')
   drop table ServiceLevel
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SysLog')
            and   type = 'U')
   drop table SysLog
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Tombstone')
            and   type = 'U')
   drop table Tombstone
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TombstoneBuriedPeopleMap')
            and   type = 'U')
   drop table TombstoneBuriedPeopleMap
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TombstoneType')
            and   type = 'U')
   drop table TombstoneType
go

if exists (select 1
            from  sysobjects
           where  id = object_id('"User"')
            and   type = 'U')
   drop table "User"
go

if exists (select 1
            from  sysobjects
           where  id = object_id('UserRoleMap')
            and   type = 'U')
   drop table UserRoleMap
go

/*==============================================================*/
/* Table: CemeteryAreas                                         */
/*==============================================================*/
create table CemeteryAreas (
   Id                   int                  identity,
   Name                 nvarchar(200)        null,
   Alias                nvarchar(200)        null,
   Remark               nvarchar(max)        null,
   RowSort              nvarchar(10)         null,
   constraint PK_CEMETERYAREAS primary key (Id)
)
go

/*==============================================================*/
/* Table: CemeteryColumns                                       */
/*==============================================================*/
create table CemeteryColumns (
   Id                   int                  identity,
   Name                 nvarchar(200)        null,
   Alias                nvarchar(200)        null,
   Remark               nvarchar(max)        null,
   constraint PK_CEMETERYCOLUMNS primary key (Id)
)
go

/*==============================================================*/
/* Table: CemeteryRows                                          */
/*==============================================================*/
create table CemeteryRows (
   Id                   int                  identity,
   Name                 nvarchar(200)        null,
   Alias                nvarchar(200)        null,
   Remark               nvarchar(max)        null,
   constraint PK_CEMETERYROWS primary key (Id)
)
go

/*==============================================================*/
/* Table: Customer                                              */
/*==============================================================*/
create table Customer (
   Id                   int                  not null,
   FullName             nvarchar(200)        null,
   LastName             nvarchar(50)         null,
   FirstName            nvarchar(50)         null,
   MiddleName           nvarchar(50)         null,
   Remark               nvarchar(max)        null,
   Telephone            nvarchar(50)         null,
   Phone                nvarchar(50)         null,
   OtherPhone           nvarchar(50)         null,
   Address              nvarchar(500)        null,
   CustomerTypeId       int                  null,
   LinkCustomerId       int                  null,
   BuryDate             datetime             null,
   DeathDate            datetime             null,
   CustomerStatusId     int                  null,
   NationalityId        int                  null,
   IDNumber             varchar(200)         null,
   constraint PK_CUSTOMER primary key (Id)
)
go

/*==============================================================*/
/* Table: CustomerStatus                                        */
/*==============================================================*/
create table CustomerStatus (
   Id                   int                  identity,
   Name                 nvarchar(max)        null,
   constraint PK_CUSTOMERSTATUS primary key (Id)
)
go

/*==============================================================*/
/* Table: CustomerType                                          */
/*==============================================================*/
create table CustomerType (
   Id                   int                  identity,
   Name                 nvarchar(200)        null,
   Description          nvarchar(Max)        null,
   constraint PK_CUSTOMERTYPE primary key (Id)
)
go

/*==============================================================*/
/* Table: Department                                            */
/*==============================================================*/
create table Department (
   Id                   int                  identity,
   Name                 nvarchar(200)        null,
   constraint PK_DEPARTMENT primary key (Id)
)
go

/*==============================================================*/
/* Table: "Function"                                            */
/*==============================================================*/
create table "Function" (
   Id                   int                  identity,
   Name                 nvarchar(200)        null,
   Code                 nvarchar(200)        null,
   Url                  nvarchar(200)        null,
   ParentId             int                  null,
   constraint PK_FUNCTION primary key (Id)
)
go

/*==============================================================*/
/* Table: Nationality                                           */
/*==============================================================*/
create table Nationality (
   Id                   int                  identity,
   Name                 varchar(200)         null,
   constraint PK_NATIONALITY primary key (Id)
)
go

/*==============================================================*/
/* Table: PaymentStatus                                         */
/*==============================================================*/
create table PaymentStatus (
   Id                   int                  identity,
   Name                 nvarchar(200)        null,
   Alias                nvarchar(200)        null,
   Remark               nvarchar(max)        null,
   constraint PK_PAYMENTSTATUS primary key (Id)
)
go

/*==============================================================*/
/* Table: Role                                                  */
/*==============================================================*/
create table Role (
   Id                   int                  identity,
   Name                 nvarchar(200)        null,
   constraint PK_ROLE primary key (Id)
)
go

/*==============================================================*/
/* Table: RoleFunctionMap                                       */
/*==============================================================*/
create table RoleFunctionMap (
   Id                   int                  not null,
   RoleId               int                  null,
   FunctionId           int                  null,
   constraint PK_ROLEFUNCTIONMAP primary key (Id)
)
go

/*==============================================================*/
/* Table: SecurityLevel                                         */
/*==============================================================*/
create table SecurityLevel (
   Id                   int                  identity,
   Name                 nvarchar(200)        null,
   Alias                nvarchar(200)        null,
   Remark               nvarchar(max)        null,
   constraint PK_SECURITYLEVEL primary key (Id)
)
go

/*==============================================================*/
/* Table: ServiceLevel                                          */
/*==============================================================*/
create table ServiceLevel (
   Id                   int                  identity,
   Name                 nvarchar(200)        null,
   Alias                nvarchar(200)        null,
   Remark               nvarchar(max)        null,
   constraint PK_SERVICELEVEL primary key (Id)
)
go

/*==============================================================*/
/* Table: SysLog                                                */
/*==============================================================*/
create table SysLog (
   Id                   int                  identity,
   Type                 int                  null,
   ControlName          nvarchar(200)        null,
   Content              nvarchar(max)        null,
   UserId               int                  null,
   Date                 datetime             null,
   Applicanter          nvarchar(100)        null,
   Telephone            nvarchar(100)        null,
   IDNumber             nvarchar(100)        null,
   Money                money                null,
   ControllTid          int                  null,
   ControllIds          nvarchar(max)        null,
   BuryMan              nvarchar(100)        null,
   BuryDate             datetime             null,
   Remark               nvarchar(max)        null,
   _Reamrk2             nvarchar(max)        null,
   constraint PK_SYSLOG primary key (Id)
)
go

/*==============================================================*/
/* Table: Tombstone                                             */
/*==============================================================*/
create table Tombstone (
   Id                   int                  identity,
   Name                 nvarchar(200)        null,
   AreaId               int                  null,
   RowId                int                  null,
   ColumnId             int                  null,
   ParentId             int                  null,
   Alias                nvarchar(200)        null,
   Remark               nvarchar(max)        null,
   CustomerId           int                  null,
   CustomerName         nvarchar(200)        null,
   StoneText            nvarchar(max)        null,
   ExpiryDate           Datetime             null,
   BuyDate              datetime             null,
   LastPaymentDate      datetime             null,
   BuryDate             datetime             null,
   Width                decimal              null,
   Height               decimal              null,
   Acreage              decimal              null,
   SecurityLevelId      int                  null,
   Image                varchar(max)         null,
   ServiceLevelId       int                  null,
   TypeId               int                  null,
   PaymentStatusId      int                  null,
   SortNum              int                  null,
   SupperManage         int                  null,
   ManageLimit          int                  null,
   constraint PK_TOMBSTONE primary key (Id)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0Îª¸¸Ä¹±®
   ·Ç0Îª¸¸Ä¹±®ID',
   'user', @CurrentUser, 'table', 'Tombstone', 'column', 'ParentId'
go

/*==============================================================*/
/* Table: TombstoneBuriedPeopleMap                              */
/*==============================================================*/
create table TombstoneBuriedPeopleMap (
   Id                   int                  identity,
   TombstoneId          int                  null,
   BuriedCustomerId     int                  null,
   Remark               nvarchar(max)        null,
   constraint PK_TOMBSTONEBURIEDPEOPLEMAP primary key (Id)
)
go

/*==============================================================*/
/* Table: TombstoneType                                         */
/*==============================================================*/
create table TombstoneType (
   Id                   int                  identity,
   Name                 nvarchar(200)        null,
   Alias                nvarchar(200)        null,
   Remark               nvarchar(max)        null,
   constraint PK_TOMBSTONETYPE primary key (Id)
)
go

/*==============================================================*/
/* Table: "User"                                                */
/*==============================================================*/
create table "User" (
   Id                   int                  identity,
   Name                 nvarchar(200)        null,
   DepartmentId         int                  null,
   LoginName            nvarchar(200)        null,
   Code                 nvarchar(200)        null,
   Remark               nvarchar(max)        null,
   Password             nvarchar(max)        null,
   Position             nvarchar(200)        null,
   CreateDate           datetime             null,
   Status               int                  null,
   constraint PK_USER primary key (Id)
)
go

/*==============================================================*/
/* Table: UserRoleMap                                           */
/*==============================================================*/
create table UserRoleMap (
   Id                   int                  identity,
   UserId               int                  not null,
   RoleId               int                  not null,
   constraint PK_USERROLEMAP primary key (Id)
)
go

alter table Customer
   add constraint FK_CUSTOMER_REFERENCE_NATIONAL foreign key (NationalityId)
      references Nationality (Id)
go

alter table Customer
   add constraint FK_CUSTOMER_REFERENCE_CUSTOMER foreign key (CustomerStatusId)
      references CustomerStatus (Id)
go

alter table Customer
   add constraint FK_CUSTOMER_REFERENCE_CUSTOMER123 foreign key (CustomerTypeId)
      references CustomerType (Id)
go

alter table RoleFunctionMap
   add constraint FK_ROLEFUNC_REFERENCE_ROLE foreign key (RoleId)
      references Role (Id)
go

alter table RoleFunctionMap
   add constraint FK_ROLEFUNC_REFERENCE_FUNCTION foreign key (FunctionId)
      references "Function" (Id)
go

alter table SysLog
   add constraint FK_SYSLOG_REFERENCE_USER foreign key (UserId)
      references "User" (Id)
go

alter table Tombstone
   add constraint FK_TOMBSTON_REFERENCE_CEMETERY_AREA foreign key (AreaId)
      references CemeteryAreas (Id)
go

alter table Tombstone
   add constraint FK_TOMBSTON_REFERENCE_CEMETERY foreign key (ColumnId)
      references CemeteryColumns (Id)
go

alter table Tombstone
   add constraint FK_TOMBSTON_REFERENCE_CEMETERY_ROW foreign key (RowId)
      references CemeteryRows (Id)
go

alter table Tombstone
   add constraint FK_TOMBSTON_REFERENCE_SECURITY foreign key (SecurityLevelId)
      references SecurityLevel (Id)
go

alter table Tombstone
   add constraint FK_TOMBSTON_REFERENCE_SERVICEL foreign key (ServiceLevelId)
      references ServiceLevel (Id)
go

alter table Tombstone
   add constraint FK_TOMBSTON_TOMBSTONE_TOMBSTON foreign key (TypeId)
      references TombstoneType (Id)
go

alter table Tombstone
   add constraint FK_TOMBSTON_TOMBSTONE_PAYMENTS foreign key (PaymentStatusId)
      references PaymentStatus (Id)
go

alter table TombstoneBuriedPeopleMap
   add constraint FK_TOMBSTON_REFERENCE_CUSTOMER13 foreign key (BuriedCustomerId)
      references Customer (Id)
go

alter table TombstoneBuriedPeopleMap
   add constraint FK_TOMBSTON_REFERENCE_TOMBSTON foreign key (TombstoneId)
      references Tombstone (Id)
go

alter table "User"
   add constraint FK_USER_REFERENCE_DEPARTME foreign key (DepartmentId)
      references Department (Id)
go

alter table UserRoleMap
   add constraint FK_USERROLE_REFERENCE_USER foreign key (UserId)
      references "User" (Id)
go

alter table UserRoleMap
   add constraint FK_USERROLE_REFERENCE_ROLE foreign key (RoleId)
      references Role (Id)
go

