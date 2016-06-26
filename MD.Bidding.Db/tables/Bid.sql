CREATE TABLE [dbo].[Bid]
(
	[BidId] INT IDENTITY(1,1) NOT NULL,
	[Amount] DECIMAL NOT NULL,	
	[ProductId]  int  NOT NULL,
	[BidderId]		NVARCHAR(128) NOT NULL, 
	[Status]     int NOT NULL,  
	[CreatedOn]		DATETIME NOT NULL DEFAULT GETDATE(),
	[TimeStamp]		DATETIME NOT NULL DEFAULT GETDATE(),	
	[UpdatedBy]		NVARCHAR(128) NULL, 

	[Deleted]		BIT NOT NULL,
	[DeletedOn]		DATETIME NULL,
	[DeletedBy]		NVARCHAR(128) NULL,

	CONSTRAINT [PK_Bid] PRIMARY KEY CLUSTERED ([BidId] ASC),
	CONSTRAINT [FK_dbo_Bid_CategoryId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product](ProductId),
	CONSTRAINT [FK_dbo_Bid_CreatedBy] FOREIGN KEY ([BidderId]) REFERENCES [dbo].[AspNetUsers](Id),
	CONSTRAINT [FK_dbo_Bid_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),	
	CONSTRAINT [FK_dbo_Bid_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),	
) ON [PRIMARY]   
