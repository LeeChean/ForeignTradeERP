USE [master]
GO
/****** Object:  Database [iup_erp]    Script Date: 04/23/2015 12:51:44 ******/
CREATE DATABASE [iup_erp] ON  PRIMARY 
( NAME = N'iup_erp', FILENAME = N'E:\Sql2008 data\Sql2008 data\iup_erp.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'iup_erp_log', FILENAME = N'E:\Sql2008 data\Sql2008 data\iup_erp_log.ldf' , SIZE = 3840KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [iup_erp] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [iup_erp].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [iup_erp] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [iup_erp] SET ANSI_NULLS OFF
GO
ALTER DATABASE [iup_erp] SET ANSI_PADDING OFF
GO
ALTER DATABASE [iup_erp] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [iup_erp] SET ARITHABORT OFF
GO
ALTER DATABASE [iup_erp] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [iup_erp] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [iup_erp] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [iup_erp] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [iup_erp] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [iup_erp] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [iup_erp] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [iup_erp] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [iup_erp] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [iup_erp] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [iup_erp] SET  DISABLE_BROKER
GO
ALTER DATABASE [iup_erp] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [iup_erp] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [iup_erp] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [iup_erp] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [iup_erp] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [iup_erp] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [iup_erp] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [iup_erp] SET  READ_WRITE
GO
ALTER DATABASE [iup_erp] SET RECOVERY FULL
GO
ALTER DATABASE [iup_erp] SET  MULTI_USER
GO
ALTER DATABASE [iup_erp] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [iup_erp] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'iup_erp', N'ON'
GO
USE [iup_erp]
GO
/****** Object:  Table [dbo].[用户表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
	[是否有效] [nchar](10) NULL,
 CONSTRAINT [PK_用户表] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_USER] ON [dbo].[用户表] 
(
	[用户编号] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[销售合同从表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[销售合同从表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[销售合同编号] [nvarchar](50) NULL,
	[销售合同序号] [nvarchar](50) NULL,
	[商品编号] [nvarchar](50) NULL,
	[商品中文名称] [nvarchar](50) NULL,
	[商品英文名称] [nvarchar](50) NULL,
	[商品货号] [nvarchar](50) NULL,
	[商品海关代码] [nvarchar](50) NULL,
	[商品单位] [nchar](10) NULL,
	[商品数量] [nvarchar](50) NULL,
	[商品单价] [money] NULL,
	[商品金额] [money] NULL,
	[增值税率] [nchar](10) NULL,
	[退税率] [nchar](10) NULL,
 CONSTRAINT [PK_销售合同从表] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[销售合同表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[销售合同表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[销售合同编号] [nchar](20) NULL,
	[评审编号] [nchar](20) NULL,
	[业务员] [nchar](10) NULL,
	[业务部门ID] [nchar](10) NULL,
	[交提货时间描述] [nvarchar](100) NULL,
	[运输方式] [nchar](10) NULL,
	[货代公司] [nvarchar](50) NULL,
	[结算方式] [nvarchar](50) NULL,
	[结汇金额] [money] NULL,
	[客户名称] [nvarchar](50) NULL,
	[客户电话] [nvarchar](50) NULL,
	[客户网址] [nvarchar](50) NULL,
	[合同建立日期] [date] NULL,
	[签约日期] [date] NULL,
	[合同终止日期] [date] NULL,
	[条款信息] [nvarchar](max) NULL,
	[合同审批状态] [nchar](10) NULL,
	[合同审批人] [nchar](10) NULL,
	[合同审批时间] [date] NULL,
 CONSTRAINT [PK_销售合同表] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[系统表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[系统表](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[公司名称] [nchar](100) NULL,
	[公司名称E] [nchar](100) NULL,
	[公司电话] [nchar](20) NULL,
	[公司传真] [nchar](20) NULL,
	[公司地址] [nchar](100) NULL,
	[公司地址E] [nchar](100) NULL,
	[评审流水号] [int] NULL,
	[备单流水号] [int] NULL,
 CONSTRAINT [PK_系统表] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所有评审的流水统一用这个号码，在每次评审保存后生效，写入系统表。评审编号的编码规则为：年份+业务类型编号+' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'系统表', @level2type=N'COLUMN',@level2name=N'评审流水号'
GO
/****** Object:  Table [dbo].[审批管理表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[审批管理表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[审批类型] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[审批队列表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[审批队列表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[业务单号] [nvarchar](50) NULL,
	[提交人] [nvarchar](50) NULL,
	[提交时间] [nvarchar](50) NULL,
	[审批状态] [nvarchar](50) NULL,
	[提交描述] [nvarchar](250) NULL,
	[审批角色] [nvarchar](50) NULL,
	[审批人] [nvarchar](50) NULL,
	[审批时间] [nvarchar](50) NULL,
	[审批意见] [nvarchar](50) NULL,
	[是否有效] [nchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[商品表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
	[是否有效] [nchar](10) NULL,
 CONSTRAINT [PK_商品表] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[入库单从表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[入库单从表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[入库单编号] [nvarchar](50) NULL,
	[序号] [nchar](10) NULL,
	[采购合同编号] [nvarchar](50) NULL,
	[货号] [nvarchar](50) NULL,
	[商品编号] [nvarchar](50) NULL,
	[商品名称] [nvarchar](50) NULL,
	[入库数量] [nvarchar](50) NULL,
	[单位] [nvarchar](50) NULL,
	[含税单价] [nvarchar](50) NULL,
	[不含税单价] [nvarchar](50) NULL,
	[税率] [nvarchar](50) NULL,
	[价税合计] [nvarchar](50) NULL,
	[不含税金额] [nvarchar](50) NULL,
	[税额] [nvarchar](50) NULL,
	[币别] [nchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[入库单表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[入库单表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[入库单编号] [nchar](10) NULL,
	[入库日期] [date] NULL,
	[制单人] [nvarchar](50) NULL,
	[仓库] [nvarchar](50) NULL,
	[入库类型] [nvarchar](50) NULL,
	[财务核算人] [nvarchar](50) NULL,
	[供应商] [nvarchar](50) NULL,
	[供应商编号] [nchar](10) NULL,
	[部门] [nvarchar](50) NULL,
	[业务员] [nvarchar](50) NULL,
	[外销发票号] [nvarchar](50) NULL,
	[备注] [nvarchar](250) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[评审从表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[评审从表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[评审编号] [nvarchar](50) NULL,
	[销售序号] [nvarchar](50) NULL,
	[商品编号] [nvarchar](50) NULL,
	[商品品名] [nvarchar](50) NULL,
	[商品货号] [nvarchar](50) NULL,
	[商品单位] [nchar](10) NULL,
	[销售单价] [decimal](18, 4) NULL,
	[销售数量] [decimal](18, 4) NULL,
	[销售总价] [decimal](18, 4) NULL,
	[增值税率] [nchar](10) NULL,
	[退税率] [nchar](10) NULL,
	[采购序号] [nvarchar](50) NULL,
	[供应商名] [nvarchar](50) NULL,
	[采购单价] [decimal](18, 4) NULL,
	[采购数量] [decimal](18, 4) NULL,
	[采购总价] [decimal](18, 4) NULL,
	[退税额] [decimal](18, 4) NULL,
 CONSTRAINT [PK_评审从表] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[评审表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[评审表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[建立日期] [date] NULL,
	[评审编号] [nchar](10) NULL,
	[评审类型] [nchar](10) NULL,
	[客户名称] [nchar](50) NULL,
	[客户国别] [nchar](10) NULL,
	[业务部门] [nchar](10) NULL,
	[业务部门ID] [nchar](10) NULL,
	[业务员] [nchar](10) NULL,
	[币别] [nchar](10) NULL,
	[汇率] [nchar](10) NULL,
	[结汇方式] [nchar](10) NULL,
	[结汇天数] [int] NULL,
	[价格条款] [nchar](10) NULL,
	[装运期限] [date] NULL,
	[交货地点] [nchar](10) NULL,
	[成本费率] [nchar](10) NULL,
	[国内运杂费] [money] NULL,
	[辅料费RMB] [money] NULL,
	[运输方式] [nchar](10) NULL,
	[运抵国] [nchar](10) NULL,
	[结汇金额USD] [money] NULL,
	[采购成本RMB] [money] NULL,
	[总费用RMB] [money] NULL,
	[出口退税] [money] NULL,
	[利润RMB] [money] NULL,
	[利润率] [nchar](10) NULL,
	[换汇成本] [nchar](10) NULL,
	[审批状态] [nchar](10) NULL,
	[审批日期] [date] NULL,
	[审批人] [nchar](10) NULL,
	[佣金率] [nchar](10) NULL,
	[是否有效] [nchar](10) NULL,
 CONSTRAINT [PK_评审表] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'费用率是小数，要除以100' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'评审表', @level2type=N'COLUMN',@level2name=N'成本费率'
GO
/****** Object:  Table [dbo].[客户表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
	[是否有效] [nchar](10) NULL,
 CONSTRAINT [PK_客户表] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[角色权限表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[角色权限表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[角色名称] [nvarchar](50) NULL,
	[角色权限集合] [nvarchar](max) NULL,
 CONSTRAINT [PK_角色权限表] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[国家表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[国家表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[国家编号] [nchar](10) NULL,
	[国家名称] [nvarchar](50) NULL,
	[国家英文名称] [nvarchar](100) NULL,
	[是否常用] [nchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[供应商表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
	[是否有效] [nchar](10) NULL,
 CONSTRAINT [PK_供应商表] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[单据制作表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[单据制作表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[制单编号] [nchar](10) NULL,
	[备货单编号] [nchar](16) NULL,
	[出口发票号] [nchar](16) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[出口确认单表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[出口确认单表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[确认单编号] [nchar](16) NULL,
	[出口发票号] [nchar](16) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[采购合同从表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[采购合同从表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[采购合同编号] [nvarchar](50) NULL,
	[采购合同序号] [nvarchar](50) NULL,
	[商品编号] [nvarchar](50) NULL,
	[商品中文名称] [nvarchar](50) NULL,
	[商品货号] [nvarchar](50) NULL,
	[商品单位] [nchar](10) NULL,
	[商品单价] [money] NULL,
	[商品数量] [nvarchar](50) NULL,
	[商品采购总价] [money] NULL,
	[交货时间] [nvarchar](50) NULL,
 CONSTRAINT [PK_采购合同从表] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[采购合同表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[采购合同表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[采购合同编号] [nchar](20) NULL,
	[评审编号] [nchar](20) NULL,
	[业务员] [nchar](10) NULL,
	[业务部门ID] [nchar](10) NULL,
	[交提货日期] [nvarchar](50) NULL,
	[运输方式] [nvarchar](50) NULL,
	[交货地点] [nvarchar](50) NULL,
	[付款方式] [nvarchar](50) NULL,
	[采购总金额] [money] NULL,
	[供应商名称] [nvarchar](50) NULL,
	[供应商电话] [nvarchar](50) NULL,
	[供应商网址] [nvarchar](50) NULL,
	[建立日期] [date] NULL,
	[签约日期] [date] NULL,
	[中止日期] [date] NULL,
	[一般条款] [nvarchar](max) NULL,
	[合同审批状态] [nchar](10) NULL,
	[合同审批人] [nchar](10) NULL,
	[合同审批时间] [date] NULL,
 CONSTRAINT [PK_采购合同表] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[部门表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[部门表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[部门名称] [nvarchar](50) NULL,
	[部门编号] [nvarchar](50) NULL,
	[部门经理编号] [nvarchar](50) NULL,
	[是否有效] [nchar](10) NULL,
 CONSTRAINT [PK_部门表] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_BUMEN] ON [dbo].[部门表] 
(
	[部门编号] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[备货单从表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[备货单从表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[备货单编号] [nchar](10) NULL,
	[销售合同号] [nvarchar](50) NULL,
	[采购合同号] [nvarchar](50) NULL,
	[商品货号] [nvarchar](50) NULL,
	[商品英文名称] [nvarchar](50) NULL,
	[商品数量] [nchar](10) NULL,
	[数量单位] [nchar](10) NULL,
	[商品单价] [nchar](10) NULL,
	[商品金额] [nchar](10) NULL,
	[币别] [nchar](10) NULL,
	[海关代码] [nchar](10) NULL,
	[包装数量] [nchar](10) NULL,
	[包装单位] [nchar](10) NULL,
	[箱号] [nchar](10) NULL,
	[单位净重] [nchar](10) NULL,
	[单位毛重] [nchar](10) NULL,
	[总净量] [nchar](10) NULL,
	[总毛重] [nchar](10) NULL,
	[重量单位] [nchar](10) NULL,
	[长] [nchar](10) NULL,
	[宽] [nchar](10) NULL,
	[高] [nchar](10) NULL,
	[长度单位] [nchar](10) NULL,
	[箱子尺寸] [nchar](10) NULL,
	[单位体积] [nchar](10) NULL,
	[体积] [nchar](10) NULL,
 CONSTRAINT [PK_备货单从表] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[备货单表]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[备货单表](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[出口备货单编号] [nvarchar](50) NULL,
	[外销发票编号] [nchar](10) NULL,
	[业务员] [nchar](10) NULL,
	[部门] [nvarchar](50) NULL,
	[发票日期] [date] NULL,
	[客户名称] [nvarchar](50) NULL,
	[结算方式] [nvarchar](50) NULL,
	[价格条款] [nchar](10) NULL,
	[币别] [nchar](10) NULL,
	[核销单号] [nvarchar](50) NULL,
	[报关单号] [nvarchar](50) NULL,
	[出口确认单号] [nvarchar](50) NULL,
	[预计出运日期] [date] NULL,
	[起运港] [nvarchar](50) NULL,
	[目的港] [nvarchar](50) NULL,
	[实际出运日期] [date] NULL,
	[制单日期] [date] NULL,
	[制单人] [nvarchar](50) NULL,
	[信用证号] [nvarchar](50) NULL,
	[运输方式] [nvarchar](50) NULL,
	[最终消费国] [nvarchar](50) NULL,
	[出口人] [nvarchar](50) NULL,
	[出口人电话] [nvarchar](50) NULL,
	[出口人传真] [nvarchar](50) NULL,
	[出口人地址] [nvarchar](50) NULL,
	[进口人] [nvarchar](50) NULL,
	[进口人电话] [nvarchar](50) NULL,
	[进口人传真] [nvarchar](50) NULL,
	[进口人地址] [nvarchar](50) NULL,
	[收货人] [nvarchar](50) NULL,
	[收货人电话] [nvarchar](50) NULL,
	[收货人传真] [nvarchar](50) NULL,
	[收货人地址] [nvarchar](50) NULL,
	[通知人] [nvarchar](50) NULL,
	[通知人电话] [nvarchar](50) NULL,
	[通知人传真] [nvarchar](50) NULL,
	[通知人地址] [nvarchar](50) NULL,
	[报关行] [nvarchar](50) NULL,
	[货代] [nvarchar](50) NULL,
	[运费] [nchar](10) NULL,
	[保费] [nchar](10) NULL,
	[佣金] [nchar](10) NULL,
	[货物体积] [nchar](10) NULL,
	[承载方式] [nchar](10) NULL,
	[集装箱型] [nvarchar](50) NULL,
	[柜数] [nchar](10) NULL,
	[唛头] [nvarchar](100) NULL,
	[备注] [nvarchar](100) NULL,
	[审批状态] [nchar](10) NULL,
	[审批人] [nchar](10) NULL,
	[审批时间] [date] NULL,
 CONSTRAINT [PK_备货单表] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[System_User]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[System_User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Zhanghao] [nvarchar](50) NULL,
	[Mima] [nvarchar](50) NULL,
	[Quanxian] [nvarchar](max) NULL,
	[Nianling] [nchar](10) NULL,
	[Dianhua] [nvarchar](50) NULL,
	[Youxiang] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[System_Hr]    Script Date: 04/23/2015 12:51:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[System_Hr](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[账号] [nvarchar](255) NULL,
	[姓名] [nvarchar](255) NULL,
	[性别] [nvarchar](255) NULL,
	[出生年月] [nvarchar](255) NULL,
	[籍贯] [nvarchar](255) NULL,
	[民族] [nvarchar](255) NULL,
	[政治面貌] [nvarchar](255) NULL,
	[部门] [nvarchar](255) NULL,
	[岗位] [nvarchar](255) NULL,
	[职工编号] [nvarchar](255) NULL,
	[现居住所] [nvarchar](255) NULL,
	[婚姻状况] [nvarchar](255) NULL,
	[宅电] [nvarchar](255) NULL,
	[手机] [nvarchar](255) NULL,
	[高中] [nvarchar](255) NULL,
	[院校专业时间1] [nvarchar](255) NULL,
	[全日制大学] [nvarchar](255) NULL,
	[院校专业时间2] [nvarchar](255) NULL,
	[进修学历] [nvarchar](255) NULL,
	[院校专业时间3] [nvarchar](255) NULL,
	[起止时间1] [nvarchar](255) NULL,
	[工作单位1] [nvarchar](255) NULL,
	[工作岗位1] [nvarchar](255) NULL,
	[起止时间2] [nvarchar](255) NULL,
	[工作单位2] [nvarchar](255) NULL,
	[工作岗位2] [nvarchar](255) NULL,
	[起止时间3] [nvarchar](255) NULL,
	[工作单位3] [nvarchar](255) NULL,
	[工作岗位3] [nvarchar](255) NULL,
	[起止时间4] [nvarchar](255) NULL,
	[工作单位4] [nvarchar](255) NULL,
	[工作岗位4] [nvarchar](255) NULL,
	[起止时间5] [nvarchar](255) NULL,
	[工作单位5] [nvarchar](255) NULL,
	[工作岗位5] [nvarchar](255) NULL,
	[姓名1] [nvarchar](255) NULL,
	[与本人关系1] [nvarchar](255) NULL,
	[单位1] [nvarchar](255) NULL,
	[联系电话1] [nvarchar](255) NULL,
	[姓名2] [nvarchar](255) NULL,
	[与本人关系2] [nvarchar](255) NULL,
	[单位2] [nvarchar](255) NULL,
	[联系电话2] [nvarchar](255) NULL,
	[姓名3] [nvarchar](255) NULL,
	[与本人关系3] [nvarchar](255) NULL,
	[单位3] [nvarchar](255) NULL,
	[联系电话3] [nvarchar](255) NULL,
	[姓名4] [nvarchar](255) NULL,
	[与本人关系4] [nvarchar](255) NULL,
	[单位4] [nvarchar](255) NULL,
	[联系电话4] [nvarchar](255) NULL,
	[姓名5] [nvarchar](255) NULL,
	[与本人关系5] [nvarchar](255) NULL,
	[单位5] [nvarchar](255) NULL,
	[联系电话5] [nvarchar](255) NULL,
	[紧急联系人] [nvarchar](255) NULL,
	[关系] [nvarchar](255) NULL,
	[紧急联系人电话] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[cghtb采购合同表cj]    Script Date: 04/23/2015 12:51:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[cghtb采购合同表cj]
AS
SELECT     dbo.采购合同表.评审编号, dbo.采购合同表.采购合同编号, dbo.采购合同表.供应商名称, dbo.采购合同表.业务员, dbo.采购合同表.业务部门ID, dbo.采购合同表.签约日期, dbo.采购合同表.中止日期, 
                      dbo.采购合同表.一般条款, dbo.采购合同表.合同审批状态, dbo.采购合同表.合同审批人, dbo.采购合同表.合同审批时间, dbo.采购合同表.采购总金额, dbo.评审表.业务部门
FROM         dbo.采购合同表 INNER JOIN
                      dbo.评审表 ON dbo.采购合同表.评审编号 = dbo.评审表.评审编号
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[51] 4[19] 2[23] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "采购合同表"
            Begin Extent = 
               Top = 0
               Left = 620
               Bottom = 331
               Right = 1179
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "评审表"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 345
               Right = 384
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'cghtb采购合同表cj'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'cghtb采购合同表cj'
GO
/****** Object:  View [dbo].[xshtb销售合同表cj]    Script Date: 04/23/2015 12:51:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[xshtb销售合同表cj]
AS
SELECT     dbo.销售合同表.ID, dbo.销售合同表.销售合同编号, dbo.销售合同表.评审编号, dbo.销售合同表.客户名称, dbo.评审表.结汇方式, dbo.评审表.结汇天数, dbo.销售合同表.合同建立日期, 
                      dbo.销售合同表.业务员, dbo.评审表.业务部门, dbo.销售合同表.合同审批状态, dbo.销售合同表.合同审批人, dbo.销售合同表.合同审批时间, dbo.销售合同表.结汇金额
FROM         dbo.销售合同表 INNER JOIN
                      dbo.评审表 ON dbo.销售合同表.评审编号 = dbo.评审表.评审编号
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "销售合同表"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 238
               Right = 504
            End
            DisplayFlags = 280
            TopColumn = 8
         End
         Begin Table = "评审表"
            Begin Extent = 
               Top = 0
               Left = 753
               Bottom = 221
               Right = 1100
            End
            DisplayFlags = 280
            TopColumn = 16
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 14
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'xshtb销售合同表cj'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'xshtb销售合同表cj'
GO
