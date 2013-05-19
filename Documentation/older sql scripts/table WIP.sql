if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dnn_BTBWeatherFeed]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[dnn_BTBWeatherFeed]
GO

CREATE TABLE [dbo].[dnn_BTBWeatherFeed] (
	[weatherId] [int] IDENTITY (1, 1) NOT NULL ,
	[moduleId] [int] NOT NULL ,
	[ttl] [int] NOT NULL ,
	[updatedDate] [datetime] NOT NULL ,
	[cachedDate] [datetime] NOT NULL ,
	[feed] [text] COLLATE Latin1_General_CI_AS NOT NULL ,
	[url] [varchar] (255) COLLATE Latin1_General_CI_AS NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[dnn_BTBWeatherFeed] WITH NOCHECK ADD 
	CONSTRAINT [PK_dnn_BTBWeatherFeed] PRIMARY KEY  CLUSTERED 
	(
		[weatherId]
	)  ON [PRIMARY] 
GO

