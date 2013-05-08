create table Core_OnlineUser
(
   	 LoginKey char(36) NOT NULL,  --Guid
   	 UserID char(10) not NULL,   --软件、模块名。
   	 
   	 LoginTime datetime NULL,        --标题   	 
   	 LoginTime datetime  NULL  --所属软件Guid
)

--创建索引
alter table Core_OnlineUser add CONSTRAINT Core_OnlineUser_PK PRIMARY KEY CLUSTERED (LoginKey);

