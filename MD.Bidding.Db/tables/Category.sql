CREATE TABLE [dbo].[Category]
(
	[CategoryId] INT NOT NULL IDENTITY(1,1) ,
	[Name] varchar(255) NOT NULL,
	[Description] varchar(255)  NULL,
	[ParentId]    int NULL,
	[CreatedOn]		DATETIME NOT NULL DEFAULT GETDATE(),
	[TimeStamp]		DATETIME NOT NULL DEFAULT GETDATE(),
	[CreatedBy]		NVARCHAR(128) NULL, 
	[UpdatedBy]		NVARCHAR(128) NULL, 

	[Deleted]		BIT NOT NULL,
	[DeletedOn]		DATETIME NULL,
	[DeletedBy]		NVARCHAR(128) NULL,
	---constraint for specifying primary key----
	CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([CategoryId] ASC),
	---specifying foreign keys for the user who as created, updated and deleted the category. (aspnetuser table stores all users of the system.
	CONSTRAINT [FK_dbo_Category_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Category](CategoryId),
	CONSTRAINT [FK_dbo_Category_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
	CONSTRAINT [FK_dbo_Category_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),	
	CONSTRAINT [FK_dbo_Category_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),	
) ON [PRIMARY]   

