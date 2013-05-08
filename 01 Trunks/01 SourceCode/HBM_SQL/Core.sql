-- 创建数据库 

USE master;
GO
CREATE DATABASE Core;

--授权程序使用



-- 创建数据表。

--创建软件模块表。
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Core_SoftSystem')
drop table Core_SoftSystem;
go
create table Core_SoftSystem
(
   	 GuidString nvarchar(36) NOT NULL,  --Guid
   	 SoftName nvarchar(100) not NULL,   --软件、模块名。
   	 Caption nvarchar(100) NULL,        --标题
   	 
   	 ObjVersion int not null,
   	 
   	 NameSpaceForClass nvarchar(100)  NULL, --所属模块类的命名空间
   	 NameSpaceForDB nvarchar(64)  NULL,     --创建数据表的前缀

	 CREATETIME datetime NULL,                    --创建时间
     LASTMODIFYTIME datetime NULL,                --最后修改时间
   	 DeleteFlag decimal(1, 0) NOT NULL default 0, --删除标识

   	 Author nvarchar(30)  NULL,
   	 SoftVersion nvarchar(10)  NULL
)

--创建索引
alter table Core_SoftSystem add CONSTRAINT Core_SoftSystem_PK PRIMARY KEY CLUSTERED (GuidString);
alter table Core_SoftSystem add CONSTRAINT Core_SoftSystem_SoftName UNIQUE  (SoftName);

CREATE NONCLUSTERED INDEX Core_SoftSystem_CreateTime ON Core_SoftSystem (CreateTime ASC);

--创建软件模块 

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Core_SoftModule')
drop table Core_SoftModule;
go
create table Core_SoftModule
(
   	 GuidString nvarchar(36) NOT NULL,  --Guid
   	 SoftName nvarchar(100) not NULL,   --软件、模块名。
   	 Caption nvarchar(100) NULL,        --标题
   	 
   	 SoftSystemGuidString nvarchar(36) NOT NULL,  --所属软件Guid
  	 ParentModuleGuidString nvarchar(36)  NULL,  --所属模块，可以为空

   	 
   	 ObjVersion int not null,
   	 
   	 NameSpaceForClass nvarchar(100)  NULL, --所属模块类的命名空间
   	 NameSpaceForDB nvarchar(64)  NULL,     --创建数据表的前缀

	 CREATETIME datetime NULL,                    --创建时间
     LASTMODIFYTIME datetime NULL,                --最后修改时间
   	 DeleteFlag decimal(1, 0) NOT NULL default 0, --删除标识

   	 Author nvarchar(30)  NULL,
   	 SoftVersion nvarchar(10)  NULL
)

--创建索引
alter table Core_SoftModule add CONSTRAINT Core_SoftModule_PK PRIMARY KEY CLUSTERED (GuidString);
alter table Core_SoftModule add CONSTRAINT Core_SoftModule_SoftName UNIQUE  (SoftName);

CREATE NONCLUSTERED INDEX Core_SoftModule_CreateTime ON Core_SoftModule (CreateTime ASC);


--创建组件类表
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Core_SysComponent')
drop table Core_SysComponent ;
go
Create table Core_SysComponent
(
  	 GuidString nvarchar(36) NOT NULL,  --Guid

   	 FullClassName    nvarchar(100) NOT NULL,  --Guid
   	 AssemblyName     nvarchar(100) NOT NULL,
   	 
   	 Author           nvarchar(20)       null,
   	 Memo             nvarchar(500)      null,
   	 
   	 RelationObject   nvarchar(100)      null ,
   	 ModuleGuidString nvarchar(36)       null ,
   	 ObjectType       int                 null default 0 
)
   	 
alter table Core_SysComponent add CONSTRAINT Core_SysComponent_PK PRIMARY KEY CLUSTERED (GuidString);   
alter table Core_SysComponent add CONSTRAINT Core_SysComponent_FullClassName UNIQUE  (FullClassName);	 


--创建组件类表
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Core_BusiEntity')
drop table Core_BusiEntity ;
go
Create table Core_BusiEntity
(
  	 GuidString    nvarchar(36)  NOT NULL,  --Guid

     EntityCode    nvarchar(50)  NOT NULL,
     EntityName    nvarchar(100) NOT NULL,
     EntityCatalog nvarchar(50)  NOT NULL,
     EntityScripe  nvarchar(36)  NOT NULL,
     
     ModuleGuidString nvarchar(36) NOT NULL,
     
     DeleteFlag       int          not null default 0 
)
   	 
alter table Core_BusiEntity add CONSTRAINT Core_BusiEntity_PK PRIMARY KEY CLUSTERED (GuidString);   
alter table Core_BusiEntity add CONSTRAINT Core_BusiEntity_EntityCode UNIQUE  (EntityCode);	

--创建组件类表
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Core_BusiDataItem')
drop table Core_BusiDataItem ;
go
Create table Core_BusiDataItem
(
  	 GuidString    nvarchar(36)  NOT NULL,  --Guid
     
     BusiEntityGuidString    nvarchar(36)  NOT NULL,  --Guid


     Caption       nvarchar(50)   NULL,
     DataLength    int            NULL,
     
     MaximumValue  int            NULL,
     MinimumValue  int            NULL,
     
     AllowNull     int            NULL,
     
     DeleteFlag       int         null default 0 
)
   	 
alter table Core_BusiDataItem add CONSTRAINT Core_BusiDataItem_PK PRIMARY KEY CLUSTERED (GuidString);   
 








