CREATE TABLE [dbo].[Product]
(
	[ProductId] INT IDENTITY(1,1) NOT NULL,
	[Name] varchar (255) NOT NULL,
	[Description] varchar (max) NULL,
	[MinimumPrice] DECIMAL NOT NULL,
	[BiddingPeriod] int NULL,
	[CategoryId]  int null,
	[BiddingPeriodEndsOn] DATETIME NOT NULL,
	[CreatedOn]		DATETIME NOT NULL DEFAULT GETDATE(),
	[TimeStamp]		DATETIME NOT NULL DEFAULT GETDATE(),
	[CreatedBy]		NVARCHAR(128) NULL, 
	[UpdatedBy]		NVARCHAR(128) NULL, 
	[MediaFolderId]	[bigint] NOT NULL,	
	[Deleted]		BIT NOT NULL,
	[DeletedOn]		DATETIME NULL,
	[DeletedBy]		NVARCHAR(128) NULL,

	CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([ProductId] ASC),
	CONSTRAINT [FK_dbo_Product_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category](CategoryId),
	CONSTRAINT [FK_dbo_Product_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
	CONSTRAINT [FK_dbo_Product_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),	
	CONSTRAINT [FK_dbo_Product_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),	
	CONSTRAINT [FK_dbo_Product_MediaId] FOREIGN KEY  ([MediaFolderId]) REFERENCES [dbo].[Media](MediaId)
) ON [PRIMARY]   
