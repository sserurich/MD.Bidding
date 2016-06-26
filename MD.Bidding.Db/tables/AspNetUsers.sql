---table contains all users i.e buyers,sellers and administrators.----

CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,	
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,

	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,	
	[Location] [nvarchar](max) NULL,
	[UserName] [nvarchar](256) NOT NULL,
	
	[DateOfBirth] [datetime] NULL,
	[GenderId]  [int] NULL,


	[Deleted]	[bit] NULL,
	[CreatedBy] [nvarchar](128) NULL, 
    [UpdatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,

    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC),
CONSTRAINT [FK_AspNetUser_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_AspNetUser_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_AspNetUser_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]
