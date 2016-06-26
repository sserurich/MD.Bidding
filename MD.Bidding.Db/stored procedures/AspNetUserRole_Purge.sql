CREATE PROCEDURE [dbo].[AspNetUserRole_Purge]
	@UserId NVARCHAR (128),
	@RoleId NVARCHAR (128)
AS
	DELETE AspNetUserRoles where UserId = @UserId and RoleId = @RoleId