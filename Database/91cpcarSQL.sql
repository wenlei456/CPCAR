USE [master]
GO
/****** Object:  Database [CarParts]    Script Date: 2016/8/7 星期日 下午 1:27:01 ******/
CREATE DATABASE [CarParts]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CarParts_', FILENAME = N'D:\DataBase\CarParts_.mdf' , SIZE = 2930560KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'CarParts__log', FILENAME = N'D:\DataBase\CarParts__log.ldf' , SIZE = 1008000KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [CarParts] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CarParts].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CarParts] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CarParts] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CarParts] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CarParts] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CarParts] SET ARITHABORT OFF 
GO
ALTER DATABASE [CarParts] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CarParts] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [CarParts] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CarParts] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CarParts] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CarParts] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CarParts] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CarParts] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CarParts] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CarParts] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CarParts] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CarParts] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CarParts] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CarParts] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CarParts] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CarParts] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CarParts] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CarParts] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CarParts] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CarParts] SET  MULTI_USER 
GO
ALTER DATABASE [CarParts] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CarParts] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CarParts] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CarParts] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'CarParts', N'ON'
GO
USE [CarParts]
GO
/****** Object:  Table [dbo].[aaa]    Script Date: 2016/8/7 星期日 下午 1:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[aaa](
	[ID] [int] NOT NULL,
	[PartName] [nvarchar](500) NULL,
	[CategoryID] [int] NULL,
	[PartTitle] [ntext] NULL,
	[PartSubtitle] [ntext] NULL,
	[PartBrand] [int] NULL,
	[PartModel] [nvarchar](50) NULL,
	[PartSuppyNo] [nvarchar](50) NULL,
	[Price] [decimal](12, 2) NOT NULL,
	[PartNumber] [varchar](50) NULL,
	[ToCars] [nvarchar](max) NULL,
	[DESCRIBE] [ntext] NULL,
	[StockNUM] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Active]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Active](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ActiveIMG] [nvarchar](200) NULL,
	[Name] [nvarchar](50) NULL,
	[SubName] [nvarchar](50) NULL,
	[StarDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[Status] [nvarchar](10) NULL,
	[Decribe] [nvarchar](200) NULL,
	[Common] [nvarchar](200) NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_ActiveMember] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ActiveAttr]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActiveAttr](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ActiveID] [int] NOT NULL,
	[AProID] [int] NOT NULL,
	[AtrrID] [int] NOT NULL,
	[NewPrice] [decimal](12, 2) NOT NULL,
	[Stock] [int] NULL,
 CONSTRAINT [PK_ActiveAttr] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ActiveImg]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActiveImg](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ActiveID] [int] NOT NULL,
	[ImgPath] [nvarchar](100) NOT NULL,
	[UploadDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_ActiveImg] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ActivePros]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivePros](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ActiveID] [int] NOT NULL,
	[ProID] [int] NOT NULL,
	[ProName] [nvarchar](50) NULL,
	[NewPrice] [decimal](12, 2) NOT NULL,
	[ATitle] [nvarchar](500) NULL,
	[AContent] [nvarchar](500) NULL,
	[Adecribe] [nvarchar](500) NULL,
	[Stock] [int] NOT NULL,
 CONSTRAINT [PK_ActiveDetail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AddressBook]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddressBook](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Province] [nvarchar](50) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[Area] [nvarchar](50) NULL,
	[Address] [nvarchar](80) NOT NULL,
	[People] [nvarchar](50) NOT NULL,
	[Tel] [nvarchar](50) NOT NULL,
	[IsDefault] [bit] NULL,
 CONSTRAINT [PK_AddressBook] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Addship]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Addship](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UID] [int] NULL,
	[scity] [nvarchar](50) NULL,
	[province] [nvarchar](50) NULL,
	[region] [nvarchar](50) NULL,
	[address] [nvarchar](200) NULL,
	[name] [nvarchar](50) NULL,
	[phone] [nvarchar](50) NULL,
 CONSTRAINT [PK_shipping] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AdminAccount]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminAccount](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Role] [nvarchar](50) NULL,
	[AccountName] [nvarchar](50) NULL,
	[RealName] [nvarchar](50) NULL,
	[PassWord] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
 CONSTRAINT [PK_AdminAccount] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CarM]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarM](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ProYear] [nvarchar](50) NULL,
	[BrandEnglish] [nvarchar](50) NULL,
	[Brand] [nvarchar](50) NULL,
	[ModelEnglish] [nvarchar](50) NULL,
	[Model] [nvarchar](50) NULL,
	[Engine] [nvarchar](50) NULL,
	[Oil] [nvarchar](50) NULL,
	[State] [int] NULL,
 CONSTRAINT [PK_CarM] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CarsInfo]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarsInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Brand] [nvarchar](50) NULL,
	[Model] [nvarchar](50) NULL,
	[ProYear] [nvarchar](50) NULL,
	[Engine] [nvarchar](50) NULL,
	[Oil] [nvarchar](50) NULL,
	[State] [int] NULL,
 CONSTRAINT [PK_CarsInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[comments]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[comments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NULL,
	[ProductID] [int] NULL,
	[mentsname] [nvarchar](500) NULL,
	[Time] [datetime] NULL,
	[shown] [int] NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CrowdFunding]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CrowdFunding](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Pid] [int] NOT NULL,
	[ShowTime] [datetime] NOT NULL,
	[StarTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[isBool] [int] NULL,
	[ListTille] [nvarchar](500) NULL,
	[Banner] [nvarchar](300) NULL,
	[Number] [int] NULL,
 CONSTRAINT [PK_CrowdFunding] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ExpRecord]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExpRecord](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Uid] [int] NOT NULL,
	[Source] [nvarchar](50) NULL,
	[LowerUid] [int] NULL,
	[OrderNum] [nvarchar](50) NULL,
	[OrderPrice] [decimal](12, 2) NULL,
	[Exp] [int] NULL,
	[Balance] [int] NULL,
	[Datetime] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_ExpRecord] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FriendCode]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FriendCode](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[CreatUser] [int] NOT NULL,
	[UseUser] [int] NULL,
	[CreatDate] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_FriendCode] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HotTable]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HotTable](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[HotName] [varchar](100) NULL,
	[Status] [int] NULL,
	[px] [int] NULL,
 CONSTRAINT [PK_HotTable] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ImgStock]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ImgStock](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PartID] [int] NOT NULL,
	[ImgPath] [varchar](500) NOT NULL,
	[UploadDate] [datetime] NULL,
 CONSTRAINT [PK_ImgStock] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[information]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[information](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ordID] [nvarchar](100) NULL,
	[wuliuhao] [nvarchar](100) NULL,
	[isshouhuo] [int] NULL,
	[time] [datetime] NULL,
 CONSTRAINT [PK_wuliuxinxi] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LogInfo]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogInfo](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[OpearName] [nvarchar](50) NOT NULL,
	[EventName] [nvarchar](50) NOT NULL,
	[EvenMsg] [nvarchar](100) NOT NULL,
	[InAttr] [text] NOT NULL,
	[OutAttr] [text] NOT NULL,
	[Datetime] [datetime] NOT NULL,
 CONSTRAINT [PK_LogInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MemberBase]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MemberBase](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[LoginName] [nvarchar](50) NULL,
	[PassWord] [nvarchar](50) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[State] [int] NULL,
	[MemberType] [int] NULL,
	[Ucode] [nvarchar](50) NULL,
	[UpUser] [int] NOT NULL,
	[Levels] [int] NULL,
	[Integral] [int] NULL,
	[Source] [nvarchar](10) NULL,
	[RegDate] [datetime] NULL,
	[wxOpenid] [nvarchar](100) NULL,
	[wxNickname] [nvarchar](100) NULL,
	[wxSex] [int] NULL,
	[wxProvince] [nvarchar](50) NULL,
	[wxCity] [nvarchar](50) NULL,
	[wxCountry] [nvarchar](50) NULL,
	[wxHeadimgurl] [nvarchar](200) NULL,
	[wxPrivilege] [nvarchar](100) NULL,
 CONSTRAINT [PK_MemberBase] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MemberLevel]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MemberLevel](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LevelId] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[MinIntegral] [int] NOT NULL,
	[MaxIntegral] [int] NOT NULL,
	[Ratio] [decimal](5, 3) NOT NULL,
	[Income] [decimal](5, 3) NOT NULL,
	[Remark] [nvarchar](200) NULL,
 CONSTRAINT [PK_UserLevel] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Order]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Order](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [varchar](50) NULL,
	[Uid] [int] NULL,
	[UserName] [varchar](50) NULL,
	[UserPhone] [varchar](50) NULL,
	[OrderSource] [nvarchar](50) NULL,
	[OrderTime] [datetime] NULL,
	[OrderStatus] [varchar](50) NULL,
	[Consignee] [varchar](50) NULL,
	[Address] [varchar](100) NULL,
	[CellPhone] [varchar](50) NULL,
	[PayStatus] [varchar](50) NULL,
	[Payment] [varchar](50) NULL,
	[Remarks] [varchar](800) NULL,
	[Preferential] [varchar](50) NULL,
	[CashPwd] [varchar](100) NULL,
	[Ratio] [decimal](12, 2) NOT NULL,
	[DiscountAmount] [decimal](12, 2) NOT NULL,
	[ShippingCost] [decimal](12, 2) NOT NULL,
	[TotalPrice] [decimal](12, 2) NOT NULL,
	[SearchTag] [varchar](500) NULL,
	[OperatorRemarks] [varchar](500) NULL,
	[OrderType] [varchar](10) NULL,
	[OrderFrom] [int] NULL,
	[SendName] [nvarchar](50) NULL,
	[SendNum] [nvarchar](50) NULL,
	[Prov] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[Area] [nvarchar](50) NULL,
	[Beizhu] [varchar](1000) NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OrderPay]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderPay](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Uid] [int] NOT NULL,
	[Uname] [nvarchar](50) NULL,
	[OrderId] [nvarchar](50) NOT NULL,
	[PayState] [nvarchar](50) NOT NULL,
	[Bank] [nvarchar](50) NOT NULL,
	[Amount] [decimal](12, 2) NOT NULL,
	[Remark] [nvarchar](100) NULL,
	[PayType] [int] NOT NULL,
	[OrderType] [int] NOT NULL,
	[RecordState] [int] NOT NULL,
 CONSTRAINT [PK_OrderPay] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderProList]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OrderProList](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [varchar](50) NULL,
	[ProductID] [int] NULL,
	[ProductName] [nvarchar](50) NULL,
	[SupplyNo] [nvarchar](50) NULL,
	[Num] [int] NOT NULL,
	[Price] [decimal](12, 2) NOT NULL,
	[InputTime] [datetime] NULL,
	[AttrID] [int] NULL,
	[AttrDecribe] [nvarchar](50) NULL,
	[PType] [int] NULL,
	[IsFlag] [int] NULL,
	[activeName] [nvarchar](50) NULL,
 CONSTRAINT [PK_OrderProList] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PartAttr]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PartAttr](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[FlagName] [varchar](50) NULL,
	[State] [int] NULL,
 CONSTRAINT [PK_PartAttr] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PartBrand]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PartBrand](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BrandName] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL,
	[Describe] [text] NULL,
	[Common] [nvarchar](500) NULL,
 CONSTRAINT [PK_PartBrand] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Parts]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Parts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PartName] [nvarchar](500) NULL,
	[CategoryID] [int] NULL,
	[PartTitle] [ntext] NULL,
	[PartSubtitle] [ntext] NULL,
	[PartBrand] [int] NULL,
	[PartModel] [nvarchar](50) NULL,
	[PartSuppyNo] [nvarchar](50) NULL,
	[Price] [decimal](12, 2) NOT NULL,
	[PartNumber] [varchar](50) NULL,
	[ToCars] [nvarchar](max) NULL,
	[Describe] [ntext] NULL,
	[SpecParma] [ntext] NULL,
	[DetailList] [ntext] NULL,
	[SAS] [ntext] NULL,
	[Common] [ntext] NULL,
	[Orderby] [int] NULL,
	[ShowDate] [datetime] NULL,
	[State] [int] NULL,
	[mDescribe] [ntext] NULL,
	[ProType] [int] NULL,
	[product_child_json] [varchar](max) NULL,
	[child_product_dicts] [varchar](max) NULL,
	[sku] [varchar](50) NULL,
	[imgProduct] [varchar](5000) NULL,
	[HotProductId] [int] NULL,
	[insertDate] [datetime] NULL,
	[supply] [varchar](100) NULL,
	[storeName] [varchar](100) NULL,
	[material] [varchar](100) NULL,
	[specifications] [varchar](100) NULL,
	[useSite] [varchar](100) NULL,
	[ParsColour] [varchar](50) NULL,
	[herf] [varchar](200) NULL,
 CONSTRAINT [PK_Parts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PartsCategory]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PartsCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](50) NULL,
	[ParentID] [int] NULL,
	[ICOPath] [nvarchar](100) NULL,
	[BannerPath] [nvarchar](100) NULL,
	[OrderBy] [int] NULL,
	[IsShow] [int] NULL,
 CONSTRAINT [PK_PartsType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PartsExtend]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PartsExtend](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PartID] [int] NOT NULL,
	[ParentAttr] [int] NOT NULL,
	[AttrName] [nvarchar](50) NULL,
	[AttrFlagName] [nvarchar](50) NULL,
	[AttrValue] [nvarchar](50) NULL,
	[AttrPrice] [decimal](12, 2) NOT NULL,
	[Qty] [int] NOT NULL,
	[State] [int] NULL,
	[IsMain] [int] NULL,
 CONSTRAINT [PK_PartAug] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PartStock]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PartStock](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PartID] [int] NULL,
	[StockType] [int] NULL,
	[StockNUM] [int] NULL,
 CONSTRAINT [PK_PartStock] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PhoneMsg]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PhoneMsg](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UID] [int] NULL,
	[PhoneNum] [varchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[Code] [varchar](500) NULL,
	[MSG] [nvarchar](250) NULL,
	[State] [varchar](20) NULL,
	[Source] [varchar](50) NULL,
	[STime] [datetime] NULL,
	[Port] [varchar](50) NULL,
	[InfoNumber] [varchar](50) NULL,
 CONSTRAINT [PK_PhoneMsg] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RebateDraw]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RebateDraw](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UID] [int] NOT NULL,
	[Price] [decimal](12, 2) NOT NULL,
	[OrderNumber] [varchar](50) NULL,
	[PType] [nvarchar](10) NULL,
	[Payment] [nvarchar](10) NULL,
	[BankInfo] [nvarchar](50) NULL,
	[CardNumber] [nvarchar](50) NULL,
	[CardName] [nvarchar](50) NULL,
	[Datetime] [datetime] NULL,
	[Statu] [int] NULL,
 CONSTRAINT [PK_RebateDraw] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RebateRecord]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RebateRecord](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UID] [int] NOT NULL,
	[Price] [decimal](12, 2) NOT NULL,
	[Source] [nvarchar](20) NULL,
	[PType] [nvarchar](20) NULL,
	[LowerUID] [int] NULL,
	[LowerLoginName] [nvarchar](50) NULL,
	[LowerOrder] [varchar](50) NULL,
	[InCome] [decimal](12, 2) NOT NULL,
	[OrderPrice] [decimal](12, 2) NOT NULL,
	[Datetime] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_RebateRecord] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[testCarM]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[testCarM](
	[id] [float] NULL,
	[ProYear] [nvarchar](255) NULL,
	[BrandEnglish] [nvarchar](255) NULL,
	[Brand] [nvarchar](255) NULL,
	[ModelEnglish] [nvarchar](255) NULL,
	[Model] [nvarchar](255) NULL,
	[Engine] [nvarchar](255) NULL,
	[Oil] [nvarchar](255) NULL,
	[State] [float] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tml]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tml](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[pro] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_tml] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Voucher]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Voucher](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Uid] [int] NOT NULL,
	[VoucherNumber] [varchar](50) NOT NULL,
	[IsState] [int] NOT NULL,
	[EndTime] [datetime] NULL,
	[InsertTime] [datetime] NULL,
	[Price] [money] NOT NULL,
	[TypeName] [varchar](50) NULL,
	[N2] [varchar](50) NULL,
 CONSTRAINT [PK_Voucher] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WebsiteMenu]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebsiteMenu](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MenuName] [nvarchar](50) NULL,
	[MenuLevel] [int] NULL,
	[MenuParent] [int] NULL,
	[MenuOrderby] [int] NULL,
	[MenuBindURL] [nvarchar](200) NULL,
	[MenuState] [int] NULL,
	[Enable] [int] NULL,
 CONSTRAINT [PK_WebsiteMenu] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [ClusteredIndex-20160509-222914]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
CREATE UNIQUE CLUSTERED INDEX [ClusteredIndex-20160509-222914] ON [dbo].[ImgStock]
(
	[ID] ASC,
	[PartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_HotId]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
CREATE NONCLUSTERED INDEX [IX_HotId] ON [dbo].[Parts]
(
	[HotProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-20151223-221734]    Script Date: 2016/8/7 星期日 下午 1:27:10 ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20151223-221734] ON [dbo].[PartsCategory]
(
	[ID] ASC,
	[CategoryName] ASC,
	[ParentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ActiveAttr] ADD  CONSTRAINT [DF_ActiveAttr_NewPrice]  DEFAULT ((0)) FOR [NewPrice]
GO
ALTER TABLE [dbo].[ActiveImg] ADD  CONSTRAINT [DF_ActiveImg_ActiveID]  DEFAULT ((-1)) FOR [ActiveID]
GO
ALTER TABLE [dbo].[ActiveImg] ADD  CONSTRAINT [DF_ActiveImg_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[ActivePros] ADD  CONSTRAINT [DF_ActivePros_NewPrice]  DEFAULT ((0)) FOR [NewPrice]
GO
ALTER TABLE [dbo].[ActivePros] ADD  CONSTRAINT [DF_ActivePros_Stock]  DEFAULT ((0)) FOR [Stock]
GO
ALTER TABLE [dbo].[CarsInfo] ADD  CONSTRAINT [DF_CarsInfo_State]  DEFAULT ((1)) FOR [State]
GO
ALTER TABLE [dbo].[ExpRecord] ADD  CONSTRAINT [DF_ExpRecord_datetime]  DEFAULT (getdate()) FOR [Datetime]
GO
ALTER TABLE [dbo].[ExpRecord] ADD  CONSTRAINT [DF_ExpRecord_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[FriendCode] ADD  CONSTRAINT [DF_FriendCode_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[MemberBase] ADD  CONSTRAINT [DF_MemberBase_State]  DEFAULT ((1)) FOR [State]
GO
ALTER TABLE [dbo].[MemberBase] ADD  CONSTRAINT [DF_MemberBase_MemberType]  DEFAULT ((0)) FOR [MemberType]
GO
ALTER TABLE [dbo].[MemberBase] ADD  CONSTRAINT [DF_MemberBase_UpUser]  DEFAULT ((-1)) FOR [UpUser]
GO
ALTER TABLE [dbo].[MemberBase] ADD  CONSTRAINT [DF_MemberBase_Source]  DEFAULT (N'self') FOR [Source]
GO
ALTER TABLE [dbo].[MemberBase] ADD  CONSTRAINT [DF_MemberBase_RegDate]  DEFAULT (getdate()) FOR [RegDate]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_OrderSource]  DEFAULT ((0)) FOR [OrderSource]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_Preferential_1]  DEFAULT ('||') FOR [Preferential]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_Ratio]  DEFAULT ((0)) FOR [Ratio]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_DiscountAmount_1]  DEFAULT ((0)) FOR [DiscountAmount]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_ShippingCost_1]  DEFAULT ((0)) FOR [ShippingCost]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_TotalPrice_1]  DEFAULT ((0)) FOR [TotalPrice]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_OrderType]  DEFAULT ((0)) FOR [OrderType]
GO
ALTER TABLE [dbo].[Order] ADD  CONSTRAINT [DF_Order_OrderFrom]  DEFAULT ((0)) FOR [OrderFrom]
GO
ALTER TABLE [dbo].[OrderPay] ADD  CONSTRAINT [DF_OrderPay_OrderType]  DEFAULT ((0)) FOR [OrderType]
GO
ALTER TABLE [dbo].[OrderPay] ADD  CONSTRAINT [DF_OrderPay_RecordState]  DEFAULT ((1)) FOR [RecordState]
GO
ALTER TABLE [dbo].[OrderProList] ADD  CONSTRAINT [DF_Table_1_OType]  DEFAULT ((0)) FOR [PType]
GO
ALTER TABLE [dbo].[OrderProList] ADD  CONSTRAINT [DF_Table_1_IsTaste]  DEFAULT ((0)) FOR [IsFlag]
GO
ALTER TABLE [dbo].[PartAttr] ADD  CONSTRAINT [DF_PartAttr_State]  DEFAULT ((1)) FOR [State]
GO
ALTER TABLE [dbo].[Parts] ADD  CONSTRAINT [DF_Parts_PartBrand]  DEFAULT ((-1)) FOR [PartBrand]
GO
ALTER TABLE [dbo].[Parts] ADD  CONSTRAINT [DF_Parts_Orderby]  DEFAULT ((0)) FOR [Orderby]
GO
ALTER TABLE [dbo].[Parts] ADD  CONSTRAINT [DF_Parts_ShowDate]  DEFAULT (getdate()) FOR [ShowDate]
GO
ALTER TABLE [dbo].[Parts] ADD  CONSTRAINT [DF_Parts_Isshow]  DEFAULT ((1)) FOR [State]
GO
ALTER TABLE [dbo].[Parts] ADD  CONSTRAINT [DF_Parts_ProType]  DEFAULT ((0)) FOR [ProType]
GO
ALTER TABLE [dbo].[PartsCategory] ADD  CONSTRAINT [DF_PartsType_ParentID]  DEFAULT ((0)) FOR [ParentID]
GO
ALTER TABLE [dbo].[PartsCategory] ADD  CONSTRAINT [DF_PartsType_IsShow]  DEFAULT ((1)) FOR [IsShow]
GO
ALTER TABLE [dbo].[PartsExtend] ADD  CONSTRAINT [DF_PartsExtend_ParentAttr]  DEFAULT ((-1)) FOR [ParentAttr]
GO
ALTER TABLE [dbo].[PartsExtend] ADD  CONSTRAINT [DF_PartsExtend_Qty]  DEFAULT ((0)) FOR [Qty]
GO
ALTER TABLE [dbo].[PartsExtend] ADD  CONSTRAINT [DF_PartsExtend_State]  DEFAULT ((1)) FOR [State]
GO
ALTER TABLE [dbo].[PartsExtend] ADD  CONSTRAINT [DF_PartsExtend_IsMain]  DEFAULT ((0)) FOR [IsMain]
GO
ALTER TABLE [dbo].[PhoneMsg] ADD  CONSTRAINT [DF_PhoneMsg_State]  DEFAULT ((0)) FOR [State]
GO
ALTER TABLE [dbo].[PhoneMsg] ADD  CONSTRAINT [DF_PhoneMsg_mtime]  DEFAULT (getdate()) FOR [STime]
GO
ALTER TABLE [dbo].[RebateRecord] ADD  CONSTRAINT [DF_RebateRecord_Price]  DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[RebateRecord] ADD  CONSTRAINT [DF_RebateRecord_InCome]  DEFAULT ((0)) FOR [InCome]
GO
ALTER TABLE [dbo].[RebateRecord] ADD  CONSTRAINT [DF_RebateRecord_OrderPrice]  DEFAULT ((0)) FOR [OrderPrice]
GO
ALTER TABLE [dbo].[Voucher] ADD  CONSTRAINT [DF_Voucher_IsState]  DEFAULT ((0)) FOR [IsState]
GO
ALTER TABLE [dbo].[WebsiteMenu] ADD  CONSTRAINT [DF_WebsiteMenu_Enable]  DEFAULT ((1)) FOR [Enable]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态 0终止 1开启' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Active', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExpRecord', @level2type=N'COLUMN',@level2name=N'Uid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'来源' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExpRecord', @level2type=N'COLUMN',@level2name=N'Source'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下级用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExpRecord', @level2type=N'COLUMN',@level2name=N'LowerUid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'变动经验' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExpRecord', @level2type=N'COLUMN',@level2name=N'Exp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'余额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExpRecord', @level2type=N'COLUMN',@level2name=N'Balance'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'昵称/姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MemberBase', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MemberBase', @level2type=N'COLUMN',@level2name=N'LoginName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MemberBase', @level2type=N'COLUMN',@level2name=N'PassWord'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MemberBase', @level2type=N'COLUMN',@level2name=N'Mobile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MemberBase', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账户状态0冻结1启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MemberBase', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账户类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MemberBase', @level2type=N'COLUMN',@level2name=N'MemberType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'身份证' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MemberBase', @level2type=N'COLUMN',@level2name=N'Ucode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'引荐人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MemberBase', @level2type=N'COLUMN',@level2name=N'UpUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'admin管理员self自己注册' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MemberBase', @level2type=N'COLUMN',@level2name=N'Source'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注册时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MemberBase', @level2type=N'COLUMN',@level2name=N'RegDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等级层级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MemberLevel', @level2type=N'COLUMN',@level2name=N'LevelId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等级名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MemberLevel', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最小积分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MemberLevel', @level2type=N'COLUMN',@level2name=N'MinIntegral'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最大积分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MemberLevel', @level2type=N'COLUMN',@level2name=N'MaxIntegral'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'折扣' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MemberLevel', @level2type=N'COLUMN',@level2name=N'Ratio'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'佣金比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MemberLevel', @level2type=N'COLUMN',@level2name=N'Income'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'OrderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户手机号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'UserPhone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'OrderSource'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下单时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'OrderTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下单状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'OrderStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收货人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'Consignee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收货地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'Address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收货人联系电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'CellPhone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'PayStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'Payment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注,附言' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'Remarks'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'优惠方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'Preferential'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'优惠方式密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'CashPwd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'优惠金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'DiscountAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运费' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'ShippingCost'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'总金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'TotalPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'搜索标签' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'SearchTag'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单类型 0,正常订单 1，充值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'OrderType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'订单来源 0:网上订单;' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Order', @level2type=N'COLUMN',@level2name=N'OrderFrom'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderPay', @level2type=N'COLUMN',@level2name=N'PayState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付类型 0 直接支付 1 优惠券' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderPay', @level2type=N'COLUMN',@level2name=N'PayType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0正常订单 1充值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderPay', @level2type=N'COLUMN',@level2name=N'OrderType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付是否可用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderPay', @level2type=N'COLUMN',@level2name=N'RecordState'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联订单ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderProList', @level2type=N'COLUMN',@level2name=N'OrderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderProList', @level2type=N'COLUMN',@level2name=N'ProductName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供货号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderProList', @level2type=N'COLUMN',@level2name=N'SupplyNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderProList', @level2type=N'COLUMN',@level2name=N'Num'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单价' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderProList', @level2type=N'COLUMN',@level2name=N'Price'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'时间戳' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderProList', @level2type=N'COLUMN',@level2name=N'InputTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大小' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderProList', @level2type=N'COLUMN',@level2name=N'AttrDecribe'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0:配件1:改装' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderProList', @level2type=N'COLUMN',@level2name=N'PType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderProList', @level2type=N'COLUMN',@level2name=N'IsFlag'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'零件种类' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parts', @level2type=N'COLUMN',@level2name=N'CategoryID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'型号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parts', @level2type=N'COLUMN',@level2name=N'PartSuppyNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'试用车型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parts', @level2type=N'COLUMN',@level2name=N'ToCars'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品详情' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parts', @level2type=N'COLUMN',@level2name=N'Describe'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'规格参数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parts', @level2type=N'COLUMN',@level2name=N'SpecParma'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'售后' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parts', @level2type=N'COLUMN',@level2name=N'SAS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parts', @level2type=N'COLUMN',@level2name=N'Common'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'产品类型 0 普通产品 1 活动产品' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Parts', @level2type=N'COLUMN',@level2name=N'ProType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否显示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PartsCategory', @level2type=N'COLUMN',@level2name=N'IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'零件号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PartsExtend', @level2type=N'COLUMN',@level2name=N'PartID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0本地库存1海外库存' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PartStock', @level2type=N'COLUMN',@level2name=N'StockType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送的用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PhoneMsg', @level2type=N'COLUMN',@level2name=N'UID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PhoneMsg', @level2type=N'COLUMN',@level2name=N'PhoneNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PhoneMsg', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'短息内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PhoneMsg', @level2type=N'COLUMN',@level2name=N'MSG'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PhoneMsg', @level2type=N'COLUMN',@level2name=N'State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'短信入口' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PhoneMsg', @level2type=N'COLUMN',@level2name=N'Source'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'端口号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PhoneMsg', @level2type=N'COLUMN',@level2name=N'Port'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'信息编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PhoneMsg', @level2type=N'COLUMN',@level2name=N'InfoNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RebateDraw', @level2type=N'COLUMN',@level2name=N'UID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发生金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RebateDraw', @level2type=N'COLUMN',@level2name=N'Price'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'条目类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RebateDraw', @level2type=N'COLUMN',@level2name=N'PType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态0冻结1正常' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RebateDraw', @level2type=N'COLUMN',@level2name=N'Statu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RebateRecord', @level2type=N'COLUMN',@level2name=N'UID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发生金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RebateRecord', @level2type=N'COLUMN',@level2name=N'Price'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'条目类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RebateRecord', @level2type=N'COLUMN',@level2name=N'PType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态0冻结1正常' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RebateRecord', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0未使用1已使用2禁止使用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Voucher', @level2type=N'COLUMN',@level2name=N'IsState'
GO
USE [master]
GO
ALTER DATABASE [CarParts] SET  READ_WRITE 
GO
