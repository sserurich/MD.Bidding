CREATE PROCEDURE [dbo].[ProductImages_IsDraft]
	@draftId int = 0
AS
	DECLARE @PublishedProductImagesId	bigint = 0
	DECLARE @IsDraft	bit = 0

	SET @PublishedProductImagesId	 = (
				select TOP 1 ProductId from Product where VersionGuid in (
					select VersionGuid from Album where AlbumId  = @draftId
				) 
				AND AlbumId  != @draftId and Published = 1
	)

IF @PublishedAlbumId IS NULL
	SET @IsDraft = 1
ELSE
	BEGIN
		--check if draft timestamp is greater than PublishedOn date
		DECLARE @TS DATETIME, @PublishedOn DATETIME
		SET @TS = (select TOP 1  [TimeStamp] from Album where AlbumId  = @draftId)
		SET @PublishedOn = (select TOP 1  [PublishedOn] from Album where AlbumId  = @PublishedAlbumId)

		IF @TS > @PublishedOn
			SET @IsDraft = 1

	END
select @IsDraft as IsDraft
