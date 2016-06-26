CREATE PROCEDURE [dbo].[Media_SetPath]
  @MediaId	bigint = 0
AS 

SET NOCOUNT ON;

DECLARE @Path varchar(255)

	-- Workout path
	;With cte As
	(
		Select MediaId, Name, ParentId From Media Where MediaId = @MediaId
		-- Recursive going up to parents
		Union All
		Select t.MediaId,  t.Name, t.ParentId From Media t Inner Join cte c On c.ParentId = t.MediaId
	)

	SELECT @Path = COALESCE(@Path+'\' ,'') + Name
		From cte where MediaId ! = @MediaId

	--Update record with  path 
	Update Media set Path = @Path where MediaId = @MediaId

