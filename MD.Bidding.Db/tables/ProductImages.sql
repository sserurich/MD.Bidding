CREATE TABLE [dbo].[ProductMedia]
(
	[ProductId]	[int] NOT NULL,
	[MediaId]	[bigint] NOT NULL,	
	PRIMARY KEY CLUSTERED ([ProductId] ASC, [MediaId] ASC),
	CONSTRAINT [FK_dbo_ProductMedia_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product](ProductId),
	CONSTRAINT [FK_dbo_ProductMedia_MediaId] FOREIGN KEY ([MediaId]) REFERENCES [dbo].[Media](MediaId)
)
