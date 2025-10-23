USE master;
GO

CREATE DATABASE Glossary;
GO

USE Glossary;
GO

-- Tabela: Users

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Users_Role]') AND type in (N'D '))
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [DF_Users_Role]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
DROP TABLE [dbo].[Users]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](255) NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Users_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Role]  DEFAULT ('User') FOR [Role]
GO


-- Tabela: GlossaryTerms

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FK_GlossaryTerms_Users_CreatedBy]') AND type in (N'F'))
ALTER TABLE [dbo].[GlossaryTerms] DROP CONSTRAINT [FK_GlossaryTerms_Users_CreatedBy]
GO
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GlossaryTerms]') AND type in (N'U'))
DROP TABLE [dbo].[GlossaryTerms]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GlossaryTerms](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Term] [nvarchar](255) NOT NULL,
	[Definition] [nvarchar](max) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[Status] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_GlossaryTerms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[GlossaryTerms]  WITH CHECK ADD  CONSTRAINT [FK_GlossaryTerms_Users_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[GlossaryTerms] CHECK CONSTRAINT [FK_GlossaryTerms_Users_CreatedBy]
GO



