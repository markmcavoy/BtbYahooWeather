if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dnn_BTBWeatherFeedAdd]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dnn_BTBWeatherFeedAdd]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dnn_BTBWeatherFeedDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dnn_BTBWeatherFeedDelete]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dnn_BTBWeatherFeedGet]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dnn_BTBWeatherFeedGet]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dnn_BTBWeatherFeedList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dnn_BTBWeatherFeedList]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dnn_BTBWeatherFeedUpdate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dnn_BTBWeatherFeedUpdate]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dnn_BTBWeatherFeed]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[dnn_BTBWeatherFeed]
GO

CREATE TABLE [dbo].[dnn_BTBWeatherFeed] (
	[weatherId] [int] IDENTITY (1, 1) NOT NULL ,
	[moduleId] [int] NOT NULL ,
	[ttl] [int] NOT NULL ,
	[updatedDate] [datetime] NULL ,
	[cachedDate] [datetime] NULL ,
	[feed] [text] COLLATE Latin1_General_CI_AS NULL ,
	[url] [varchar] (255) COLLATE Latin1_General_CI_AS NOT NULL ,
	[transformedFeed] [text] COLLATE Latin1_General_CI_AS NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[dnn_BTBWeatherFeed] WITH NOCHECK ADD 
	CONSTRAINT [PK_dnn_BTBWeatherFeed] PRIMARY KEY  CLUSTERED 
	(
		[weatherId]
	)  ON [PRIMARY] 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


create PROCEDURE dnn_BTBWeatherFeedAdd
	@moduleId int,
	@ttl int,
	@updatedDate datetime,
	@cachedDate datetime,
	@feed text,
	@url varchar(255),
	@transformedFeed text
AS

INSERT INTO dnn_BTBWeatherFeed (
	[moduleId],
	[ttl],
	[updatedDate],
	[cachedDate],
	[feed],
	[url],
	[transformedFeed]
) VALUES (
	@moduleId,
	@ttl,
	@updatedDate,
	@cachedDate,
	@feed,
	@url,
	@transformedFeed
)

select SCOPE_IDENTITY()

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dnn_BTBWeatherFeedDelete
	@weatherId int
AS

DELETE FROM dnn_BTBWeatherFeed
WHERE
	[weatherId] = @weatherId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


create PROCEDURE dnn_BTBWeatherFeedGet
	@weatherId int
AS

SELECT
	[weatherId],
	[moduleId],
	[ttl],
	[updatedDate],
	[cachedDate],
	[feed],
	[url],
	[transformedFeed]
FROM dnn_BTBWeatherFeed
WHERE
	[weatherId] = @weatherId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


create PROCEDURE dnn_BTBWeatherFeedGetByModule
(
 @moduleId int
)
AS

SELECT
	[weatherId],
	[moduleId],
	[ttl],
	[updatedDate],
	[cachedDate],
	[feed],
	[url],
	[transformedFeed]
FROM dnn_BTBWeatherFeed
	WHERE moduleId = @moduleId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


create PROCEDURE dnn_BTBWeatherFeedUpdate
	@weatherId int, 
	@moduleId int, 
	@ttl int, 
	@updatedDate datetime, 
	@cachedDate datetime, 
	@feed text, 
	@url varchar(255),
        @transformedFeed text
AS

UPDATE dnn_BTBWeatherFeed SET
	[moduleId] = @moduleId,
	[ttl] = @ttl,
	[updatedDate] = @updatedDate,
	[cachedDate] = @cachedDate,
	[feed] = @feed,
	[url] = @url,
	[transformedFeed] = @transformedFeed
WHERE
	[weatherId] = @weatherId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

