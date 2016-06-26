CREATE PROCEDURE [dbo].[Album_Publish]

	@AlbumId int, 
	@PublishedBy NVARCHAR (128)
AS	
	DECLARE @PublishedAlbumId	int = 0

	IF(NOT EXISTS(
		select TOP 1 AlbumId from Album where VersionGuid in (select VersionGuid from Album where AlbumId  = @AlbumId) and AlbumId  != @AlbumId and Published = 1
		))

		BEGIN
																				
			INSERT INTO Album
				(Timestamp, AlbumGuid, Name, Description, Published, PublishedOn, PublishedBy, VersionGuid, CreatedOn, Deleted,SchoolId,MediaFolderId)
			SELECT 
				GETDATE(), AlbumGuid, Name, Description, 1, GETDATE(), @PublishedBy ,VersionGuid, CreatedOn, 0,SchoolId,MediaFolderId
			FROM Album
			WHERE AlbumId = @AlbumId

			SET @PublishedAlbumId = (SELECT @@IDENTITY)

		END
	ELSE --Update Album
		BEGIN
			SET @PublishedAlbumId	 = (
				select TOP 1 AlbumId from Album where VersionGuid in (select VersionGuid from Album where AlbumId  = @AlbumId) and AlbumId  != @AlbumId and Published = 1
			)

			Update Album SET
				
				Album.Name = tV.Name,
				Album.[Description] = tV.[Description],				
				Album.PublishedOn = GETDATE(),
				Album.PublishedBy = @PublishedBy,
				Album.[Timestamp] = GETDATE(),
				Album.Deleted = 0,
				Album.SchoolId=tV.SchoolId,
				Album.MediaFolderId = tV.MediaFolderId
			from Album
			INNER JOIN
				Album tV
			ON
				Album.VersionGuid = tV.VersionGuid
			WHERE
				tV.AlbumId= @AlbumId
				AND Album.AlbumId != @AlbumId
		END


	--Set published date on draft record to be displayed in Admin
	Update Album Set PublishedOn = GetDaTe(), PublishedBy = @PublishedBy where AlbumId = @AlbumId

	--return new AlbumId
	SELECT @PublishedAlbumId AS AlbumId


