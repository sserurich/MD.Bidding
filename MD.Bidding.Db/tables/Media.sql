CREATE TABLE [dbo].[Media]
(
	[MediaId]	BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
	[MediaGuid] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
	[Name]		VARCHAR(255) NOT NULL, 
	[ParentId]	BIGINT NULL,  
	[Path]		VARCHAR(255) NULL,	
	[TimeStamp] DATETIME NOT NULL DEFAULT GETDATE(), 
    [Filesize] INT NULL, 
    [MediaTypeId] BIGINT NULL, 
    [ExtensionTypeId] BIGINT NULL,
	[CreatedOn] DATETIME NOT NULL, 
	[Deleted] BIT NOT NULL, 
    [DeletedOn] DATETIME NULL,
    CONSTRAINT [FK_Media_ExtensionType] FOREIGN KEY ([ExtensionTypeId]) REFERENCES [ExtensionType]([ExtensionTypeId]),
	CONSTRAINT [FK_Media_MediaType] FOREIGN KEY ([MediaTypeId]) REFERENCES [MediaType]([MediaTypeId])
)
