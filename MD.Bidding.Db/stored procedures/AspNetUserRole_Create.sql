CREATE PROCEDURE [dbo].[AspNetUserRole_Create]
	@UserId NVARCHAR (128),
	@RoleId NVARCHAR (128)
	 AS
	BEGIN
		INSERT INTO AspNetUserRoles (UserId,RoleId)
		VALUES (@UserId,@RoleId);
		
	END
