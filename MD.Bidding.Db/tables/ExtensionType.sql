CREATE TABLE [dbo].[ExtensionType]
(
	[ExtensionTypeId] BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Ext] VARCHAR(255) NOT NULL, 
    [MediaTypeId] BIGINT NULL,

	CONSTRAINT [Fk_dbo_ExtensionType_MediaTypeId] FOREIGN KEY ([MediaTypeId]) REFERENCES [dbo].[MediaType](MediaTypeId)

)