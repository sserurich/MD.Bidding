CREATE PROCEDURE [dbo].[Media_GetDescendants]
  @MediaId bigint = 0
AS 

SET NOCOUNT ON;

WITH Recursive_CTE AS (
	 SELECT
		media.MediaId, CAST(media.ParentId as bigInt) ParentId, media.Name, media.MediaGuid, media.MediaTypeId
	 FROM Media media
	WHERE media.MediaId = @MediaId and media.Deleted = 0


 UNION ALL

	 SELECT media.MediaId, media.ParentId, media.Name, media.MediaGuid, media.MediaTypeId
	 FROM Recursive_CTE parent
	 INNER JOIN Media media ON media.ParentId = parent.MediaId
	 where media.Deleted = 0
)

SELECT * FROM Recursive_CTE r