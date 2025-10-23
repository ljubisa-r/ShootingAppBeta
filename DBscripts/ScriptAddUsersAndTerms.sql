 

USE Glossary;
GO

-- Primer podataka 

IF  NOT EXISTS (SELECT 1 FROM [dbo].[Users])
INSERT INTO [dbo].[Users] (Username, Email, PasswordHash, Role)
VALUES
('admin', 'admin@example.com', 'admin123', 'Administration'),
('pub', 'pub@example.com', 'pub123', 'Publisher'),
('user', 'user@example.com', 'user123', 'User'),
('editor', 'editor123', 'editor@glossary.com','Publisher');
GO
 
 
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

