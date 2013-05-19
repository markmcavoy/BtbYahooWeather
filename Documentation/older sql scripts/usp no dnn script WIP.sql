/* =====================================================================================
/   TABLE: BTBWeatherFeed
/  ===================================================================================== */

/****** Object:  Stored Procedure dnn_BTBWeatherFeedGet    Script Date: 07 March 2006 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'dnn_BTBWeatherFeedGet') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dnn_BTBWeatherFeedGet
GO

/****** Object:  Stored Procedure dnn_BTBWeatherFeedList    Script Date: 07 March 2006 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'dnn_BTBWeatherFeedList') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dnn_BTBWeatherFeedList
GO

/****** Object:  Stored Procedure dnn_BTBWeatherFeedAdd    Script Date: 07 March 2006 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'dnn_BTBWeatherFeedAdd') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dnn_BTBWeatherFeedAdd
GO

/****** Object:  Stored Procedure dnn_BTBWeatherFeedUpdate    Script Date: 07 March 2006 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'dnn_BTBWeatherFeedUpdate') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dnn_BTBWeatherFeedUpdate
GO

/****** Object:  Stored Procedure dnn_BTBWeatherFeedDelete    Script Date: 07 March 2006 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'dnn_BTBWeatherFeedDelete') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dnn_BTBWeatherFeedDelete
GO


/* -------------------------------------------------------------------------------------
/   BTBWeatherFeedGet
/  ------------------------------------------------------------------------------------- */
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dnn_BTBWeatherFeedGet
	@weatherId int
	,@moduleId int 
AS

SELECT
	[weatherId],
	[moduleId],
	[ttl],
	[updatedDate],
	[cachedDate],
	[feed],
	[url]
FROM dnn_BTBWeatherFeed
WHERE
	[weatherId] = @weatherId
	AND [moduleid]=@moduleId 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/* -------------------------------------------------------------------------------------
/   BTBWeatherFeedList 
/  ------------------------------------------------------------------------------------- */
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dnn_BTBWeatherFeedList
AS

SELECT
	[weatherId],
	[moduleId],
	[ttl],
	[updatedDate],
	[cachedDate],
	[feed],
	[url]
FROM dnn_BTBWeatherFeed
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


/* -------------------------------------------------------------------------------------
/   BTBWeatherFeedAdd
/  ------------------------------------------------------------------------------------- */
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dnn_BTBWeatherFeedAdd
	@moduleId int,
	@ttl int,
	@updatedDate datetime,
	@cachedDate datetime,
	@feed text,
	@url varchar(255)
AS

INSERT INTO dnn_BTBWeatherFeed (
	[moduleId],
	[ttl],
	[updatedDate],
	[cachedDate],
	[feed],
	[url]
) VALUES (
	@moduleId,
	@ttl,
	@updatedDate,
	@cachedDate,
	@feed,
	@url
)

select SCOPE_IDENTITY()
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/* -------------------------------------------------------------------------------------
/   BTBWeatherFeedUpdate
/  ------------------------------------------------------------------------------------- */
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dnn_BTBWeatherFeedUpdate
	@weatherId int, 
	@moduleId int, 
	@ttl int, 
	@updatedDate datetime, 
	@cachedDate datetime, 
	@feed text, 
	@url varchar(255) 
AS

UPDATE dnn_BTBWeatherFeed SET
	[moduleId] = @moduleId,
	[ttl] = @ttl,
	[updatedDate] = @updatedDate,
	[cachedDate] = @cachedDate,
	[feed] = @feed,
	[url] = @url
WHERE
	[weatherId] = @weatherId
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/* -------------------------------------------------------------------------------------
/   BTBWeatherFeedDelete
/  ------------------------------------------------------------------------------------- */
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