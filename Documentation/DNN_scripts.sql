if exists (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}BTBWeatherFeedAdd]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure {databaseOwner}[{objectQualifier}BTBWeatherFeedAdd]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}BTBWeatherFeedDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure {databaseOwner}[{objectQualifier}BTBWeatherFeedDelete]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}BTBWeatherFeedGet]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure {databaseOwner}[{objectQualifier}BTBWeatherFeedGet]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}BTBWeatherFeedGetByModule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure {databaseOwner}[{objectQualifier}BTBWeatherFeedGetByModule]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}BTBWeatherFeedUpdate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure {databaseOwner}[{objectQualifier}BTBWeatherFeedUpdate]
GO




if exists (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}BTBWeatherFeed]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
begin
 
  ALTER TABLE {databaseOwner}[{objectQualifier}BTBWeatherFeed]] DROP CONSTRAINT [FK_{ objectQualifier}BTBWeatherFeed]_{objectQualifier}Modules]


 ALTER TABLE {databaseOwner}[{objectQualifier}BTBWeatherFeed]] DROP CONSTRAINT [PK_{ objectQualifier}BTBWeatherFeed]]

 drop table {databaseOwner}[{objectQualifier}BTBWeatherFeed]

end
GO

CREATE TABLE {databaseOwner}[{objectQualifier}BTBWeatherFeed] (
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

ALTER TABLE {databaseOwner}[{objectQualifier}BTBWeatherFeed] WITH NOCHECK ADD 
	CONSTRAINT [PK_{objectQualifier}BTBWeatherFeed] PRIMARY KEY  CLUSTERED 
	(
		[weatherId]
	)  ON [PRIMARY] 
GO

ALTER TABLE {databaseOwner}[{objectQualifier}BTBWeatherFeed] WITH NOCHECK ADD 
	CONSTRAINT [FK_{objectQualifier}BTBWeatherFeed_{objectQualifier}Modules] FOREIGN KEY ([ModuleID]) 
	REFERENCES {databaseOwner}[{objectQualifier}Modules] 
	([ModuleID]) 
	ON DELETE CASCADE NOT FOR REPLICATION
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO



create PROCEDURE {objectQualifier}BTBWeatherFeedAdd
	@moduleId int,
	@ttl int,
	@updatedDate datetime,
	@cachedDate datetime,
	@feed text,
	@url varchar(255),
	@transformedFeed text
AS

INSERT INTO {objectQualifier}BTBWeatherFeed (
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



CREATE PROCEDURE {objectQualifier}BTBWeatherFeedDelete
	@weatherId int
AS

DELETE FROM {objectQualifier}BTBWeatherFeed
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



create PROCEDURE {objectQualifier}BTBWeatherFeedGet
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
FROM {objectQualifier}BTBWeatherFeed
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



create PROCEDURE {objectQualifier}BTBWeatherFeedGetByModule
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
FROM {objectQualifier}BTBWeatherFeed
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



create PROCEDURE {objectQualifier}BTBWeatherFeedUpdate
	@weatherId int, 
	@moduleId int, 
	@ttl int, 
	@updatedDate datetime, 
	@cachedDate datetime, 
	@feed text, 
	@url varchar(255),
        @transformedFeed text
AS

UPDATE {objectQualifier}BTBWeatherFeed SET
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

