CREATE TABLE [dbo].[用户表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[用户编号] [nvarchar](50) NULL,
	[用户名] [nchar](50) NULL,
	[用户密码] [nchar](50) NULL,
	[部门编号] [nvarchar](50) NULL,
	[姓名] [nchar](50) NULL,
	[年龄] [nchar](50) NULL,
	[电话] [nchar](50) NULL,
	[邮件] [nchar](50) NULL,
	[角色编号] [nchar](50) NULL,
	[负责人员编号] [nvarchar](50) NULL,
	[是否有效] [nchar](10) NULL)
	
	
CREATE TABLE [dbo].[部门表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[部门名称] [nvarchar](50) NULL,
	[部门编号] [nvarchar](50) NULL,
	[部门经理编号] [nvarchar](50) NULL,
	[是否有效] [nchar](10) NULL)
	

CREATE TABLE [dbo].[商品表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[商品编号] [nvarchar](50) NULL,
	[商品中文名称] [nvarchar](50) NULL,
	[商品英文名称] [nvarchar](50) NULL,
	[商品货号] [nvarchar](50) NULL,
	[商品类别] [nvarchar](50) NULL,
	[商品单位] [nvarchar](50) NULL,
	[商品关税率] [nvarchar](50) NULL,
	[商品增值税率] [nvarchar](50) NULL,
	[商品退税率] [nvarchar](50) NULL,
	[商品海关编码] [nvarchar](50) NULL,
	[商品建立人] [nvarchar](50) NULL,
	[商品备注] [nvarchar](max) NULL,
	[是否有效] [nchar](10) NULL)
	
	
CREATE TABLE [dbo].[客户表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[客户编号] [nvarchar](50) NULL,
	[客户类别] [nvarchar](50) NULL,
	[客户中文名称] [nvarchar](50) NULL,
	[客户英文名称] [nvarchar](50) NULL,
	[客户简称] [nvarchar](50) NULL,
	[客户国家] [nvarchar](50) NULL,
	[客户地址] [nvarchar](50) NULL,
	[客户电话] [nvarchar](50) NULL,
	[客户邮箱] [nvarchar](50) NULL,
	[客户联系人] [nvarchar](50) NULL,
	[客户建立人] [nvarchar](50) NULL,
	[客户建立日期] [date] NULL,
	[客户备注] [nvarchar](max) NULL,
	[是否有效] [nchar](10) NULL)
	
	
CREATE TABLE [dbo].[供应商表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[供应商编号] [nvarchar](50) NULL,
	[供应商类别] [nvarchar](50) NULL,
	[供应商中文名称] [nvarchar](50) NULL,
	[供应商简称] [nvarchar](50) NULL,
	[供应商国家] [nvarchar](50) NULL,
	[供应商地址] [nvarchar](50) NULL,
	[供应商电话] [nvarchar](50) NULL,
	[供应商邮箱] [nvarchar](50) NULL,
	[供应商联系人] [nvarchar](50) NULL,
	[供应商开户行] [nvarchar](50) NULL,
	[供应商开户行账号] [nvarchar](50) NULL,
	[供应商建立人] [nvarchar](50) NULL,
	[供应商建立时间] [smalldatetime] NULL,
	[供应商备注] [nvarchar](max) NULL,
	[是否有效] [nchar](10) NULL)
	

				
		