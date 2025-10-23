-- ========================================
-- Kreiranje baze Glossary
-- ========================================
CREATE DATABASE Glossary;
GO

USE Glossary;
GO

-- ========================================
-- Tabela: Users
-- ========================================

/****** Object:  Table [dbo].[Users]    Script Date: 10/22/2025 10:39:11 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [DF_Users_Role]
DROP TABLE [dbo].[Users]
END
GO

/****** Object:  Table [dbo].[Users]    Script Date: 10/22/2025 10:39:11 PM ******/
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


-- ========================================
-- Tabela: GlossaryTerms
-- ========================================
/****** Object:  Table [dbo].[GlossaryTerms]    Script Date: 10/17/2025 8:50:42 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GlossaryTerms]') AND type in (N'U'))
DROP TABLE [dbo].[GlossaryTerms]
GO

/****** Object:  Table [dbo].[GlossaryTerms]    Script Date: 10/17/2025 7:35:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GlossaryTerms](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Term] [nvarchar](255) NULL,
	[Definition] [nvarchar](max) NULL,
	[CreatedBy] [int] null,
	[Status] [nvarchar](255) NULL,	
 CONSTRAINT [PK_GlossaryTerms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

-- ========================================
-- Primer podataka (seed)
-- ========================================

 
IF  NOT EXISTS (SELECT 1 FROM [dbo].[GlossaryTerms] )
INSERT INTO [dbo].[GlossaryTerms] ([Term], [Definition], [CreatedBy], [Status])
VALUES 
('API', 'Application Programming Interface — skup pravila za komunikaciju izmedju softverskih komponenti.', 1, 'Published'),
('Frontend', 'Deo aplikacije koji korisnik vidi i sa kojim interaguje.', 2, 'Published'),
('Backend', 'Serverski deo aplikacije koji obradjuje podatke i rukuje logikom.', 2, 'Draft'),
('Database', 'Organizovana kolekcija podataka kojima se pristupa elektronski.', 3, 'Published'),
('JWT', 'JSON Web Token — standard za autentifikaciju putem tokena.', 1, 'Published'),
('REST', 'Representational State Transfer — arhitekturalni stil za izgradnju web servisa.', 1, 'Draft'),
('Entity Framework', 'ORM alat za rad sa bazom podataka u .NET aplikacijama.', 4, 'Published'),
('Vue.js', 'JavaScript framework za izgradnju korisničkih interfejsa.', 2, 'Published'),
('HTTP', 'Hypertext Transfer Protocol — protokol za prenos podataka na webu.', 3, 'Published'),
('CORS', 'Cross-Origin Resource Sharing — mehanizam koji omogucava deljenje resursa izmedju razlicitih domena.', 4, 'Draft');
GO

IF  NOT EXISTS (SELECT 1 FROM [dbo].[Users])
INSERT INTO [dbo].[Users] (Username, Email, PasswordHash, Role)
VALUES
('admin', 'admin@example.com', 'admin123', 'Administration'),
('pub', 'pub@example.com', 'pub123', 'Publisher'),
('user', 'user@example.com', 'user123', 'User'),
('editor', 'editor123', 'editor@glossary.com','Publisher');
GO
 