
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/11/2018 18:11:31
-- Generated from EDMX file: D:\Projects\WebLibraryProject\WebLibraryProject\Models\LibraryDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [LibraryDB2];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PublicationAuthor_Publication]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PublicationAuthor] DROP CONSTRAINT [FK_PublicationAuthor_Publication];
GO
IF OBJECT_ID(N'[dbo].[FK_PublicationAuthor_Author]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PublicationAuthor] DROP CONSTRAINT [FK_PublicationAuthor_Author];
GO
IF OBJECT_ID(N'[dbo].[FK_PublicationBookLocation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BookLocation] DROP CONSTRAINT [FK_PublicationBookLocation];
GO
IF OBJECT_ID(N'[dbo].[FK_BookLocationReader]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BookLocation] DROP CONSTRAINT [FK_BookLocationReader];
GO
IF OBJECT_ID(N'[dbo].[FK_PublicationCourse_Publication]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PublicationCourse] DROP CONSTRAINT [FK_PublicationCourse_Publication];
GO
IF OBJECT_ID(N'[dbo].[FK_PublicationCourse_Course]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PublicationCourse] DROP CONSTRAINT [FK_PublicationCourse_Course];
GO
IF OBJECT_ID(N'[dbo].[FK_DisciplinePublication_Discipline]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DisciplinePublication] DROP CONSTRAINT [FK_DisciplinePublication_Discipline];
GO
IF OBJECT_ID(N'[dbo].[FK_DisciplinePublication_Publication]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DisciplinePublication] DROP CONSTRAINT [FK_DisciplinePublication_Publication];
GO
IF OBJECT_ID(N'[dbo].[FK_PublicationStats]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Stats] DROP CONSTRAINT [FK_PublicationStats];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Publication]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Publication];
GO
IF OBJECT_ID(N'[dbo].[Author]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Author];
GO
IF OBJECT_ID(N'[dbo].[BookLocation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BookLocation];
GO
IF OBJECT_ID(N'[dbo].[Reader]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reader];
GO
IF OBJECT_ID(N'[dbo].[Stats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Stats];
GO
IF OBJECT_ID(N'[dbo].[Course]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Course];
GO
IF OBJECT_ID(N'[dbo].[Discipline]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Discipline];
GO
IF OBJECT_ID(N'[dbo].[PublicationAuthor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PublicationAuthor];
GO
IF OBJECT_ID(N'[dbo].[PublicationCourse]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PublicationCourse];
GO
IF OBJECT_ID(N'[dbo].[DisciplinePublication]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DisciplinePublication];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Publication'
CREATE TABLE [dbo].[Publication] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(70)  NOT NULL,
    [DatePublished] datetime  NOT NULL,
    [PublicationType] tinyint  NOT NULL,
    [Publisher] nvarchar(25)  NULL,
    [InternetLocation] nvarchar(max)  NULL,
    [BookPublication] tinyint  NOT NULL
);
GO

-- Creating table 'Author'
CREATE TABLE [dbo].[Author] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [First] nvarchar(15)  NOT NULL,
    [Last] nvarchar(15)  NOT NULL,
    [Patronimic] nvarchar(15)  NOT NULL,
    [WriterType] tinyint  NOT NULL
);
GO

-- Creating table 'BookLocation'
CREATE TABLE [dbo].[BookLocation] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Room] int  NOT NULL,
    [Place] nvarchar(70)  NOT NULL,
    [IsTaken] bit  NOT NULL,
    [Publication_Id] int  NOT NULL,
    [Reader_Id] int  NULL
);
GO

-- Creating table 'Reader'
CREATE TABLE [dbo].[Reader] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [First] nvarchar(15)  NOT NULL,
    [Last] nvarchar(15)  NOT NULL,
    [Patronimic] nvarchar(15)  NOT NULL,
    [AccessLevel] tinyint  NOT NULL,
    [Group] nvarchar(9)  NOT NULL
);
GO

-- Creating table 'Stats'
CREATE TABLE [dbo].[Stats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DateTaken] datetime  NOT NULL,
    [Publication_Id] int  NOT NULL
);
GO

-- Creating table 'Course'
CREATE TABLE [dbo].[Course] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Course] tinyint  NOT NULL
);
GO

-- Creating table 'Discipline'
CREATE TABLE [dbo].[Discipline] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(25)  NOT NULL
);
GO

-- Creating table 'PublicationAuthor'
CREATE TABLE [dbo].[PublicationAuthor] (
    [Publications_Id] int  NOT NULL,
    [Authors_Id] int  NOT NULL
);
GO

-- Creating table 'PublicationCourse'
CREATE TABLE [dbo].[PublicationCourse] (
    [Publication_Id] int  NOT NULL,
    [Course_Id] int  NOT NULL
);
GO

-- Creating table 'DisciplinePublication'
CREATE TABLE [dbo].[DisciplinePublication] (
    [Discipline_Id] int  NOT NULL,
    [Publication_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Publication'
ALTER TABLE [dbo].[Publication]
ADD CONSTRAINT [PK_Publication]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Author'
ALTER TABLE [dbo].[Author]
ADD CONSTRAINT [PK_Author]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BookLocation'
ALTER TABLE [dbo].[BookLocation]
ADD CONSTRAINT [PK_BookLocation]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Reader'
ALTER TABLE [dbo].[Reader]
ADD CONSTRAINT [PK_Reader]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Stats'
ALTER TABLE [dbo].[Stats]
ADD CONSTRAINT [PK_Stats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Course'
ALTER TABLE [dbo].[Course]
ADD CONSTRAINT [PK_Course]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Discipline'
ALTER TABLE [dbo].[Discipline]
ADD CONSTRAINT [PK_Discipline]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Publications_Id], [Authors_Id] in table 'PublicationAuthor'
ALTER TABLE [dbo].[PublicationAuthor]
ADD CONSTRAINT [PK_PublicationAuthor]
    PRIMARY KEY CLUSTERED ([Publications_Id], [Authors_Id] ASC);
GO

-- Creating primary key on [Publication_Id], [Course_Id] in table 'PublicationCourse'
ALTER TABLE [dbo].[PublicationCourse]
ADD CONSTRAINT [PK_PublicationCourse]
    PRIMARY KEY CLUSTERED ([Publication_Id], [Course_Id] ASC);
GO

-- Creating primary key on [Discipline_Id], [Publication_Id] in table 'DisciplinePublication'
ALTER TABLE [dbo].[DisciplinePublication]
ADD CONSTRAINT [PK_DisciplinePublication]
    PRIMARY KEY CLUSTERED ([Discipline_Id], [Publication_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Publications_Id] in table 'PublicationAuthor'
ALTER TABLE [dbo].[PublicationAuthor]
ADD CONSTRAINT [FK_PublicationAuthor_Publication]
    FOREIGN KEY ([Publications_Id])
    REFERENCES [dbo].[Publication]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Authors_Id] in table 'PublicationAuthor'
ALTER TABLE [dbo].[PublicationAuthor]
ADD CONSTRAINT [FK_PublicationAuthor_Author]
    FOREIGN KEY ([Authors_Id])
    REFERENCES [dbo].[Author]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PublicationAuthor_Author'
CREATE INDEX [IX_FK_PublicationAuthor_Author]
ON [dbo].[PublicationAuthor]
    ([Authors_Id]);
GO

-- Creating foreign key on [Publication_Id] in table 'BookLocation'
ALTER TABLE [dbo].[BookLocation]
ADD CONSTRAINT [FK_PublicationBookLocation]
    FOREIGN KEY ([Publication_Id])
    REFERENCES [dbo].[Publication]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PublicationBookLocation'
CREATE INDEX [IX_FK_PublicationBookLocation]
ON [dbo].[BookLocation]
    ([Publication_Id]);
GO

-- Creating foreign key on [Reader_Id] in table 'BookLocation'
ALTER TABLE [dbo].[BookLocation]
ADD CONSTRAINT [FK_BookLocationReader]
    FOREIGN KEY ([Reader_Id])
    REFERENCES [dbo].[Reader]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BookLocationReader'
CREATE INDEX [IX_FK_BookLocationReader]
ON [dbo].[BookLocation]
    ([Reader_Id]);
GO

-- Creating foreign key on [Publication_Id] in table 'PublicationCourse'
ALTER TABLE [dbo].[PublicationCourse]
ADD CONSTRAINT [FK_PublicationCourse_Publication]
    FOREIGN KEY ([Publication_Id])
    REFERENCES [dbo].[Publication]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Course_Id] in table 'PublicationCourse'
ALTER TABLE [dbo].[PublicationCourse]
ADD CONSTRAINT [FK_PublicationCourse_Course]
    FOREIGN KEY ([Course_Id])
    REFERENCES [dbo].[Course]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PublicationCourse_Course'
CREATE INDEX [IX_FK_PublicationCourse_Course]
ON [dbo].[PublicationCourse]
    ([Course_Id]);
GO

-- Creating foreign key on [Discipline_Id] in table 'DisciplinePublication'
ALTER TABLE [dbo].[DisciplinePublication]
ADD CONSTRAINT [FK_DisciplinePublication_Discipline]
    FOREIGN KEY ([Discipline_Id])
    REFERENCES [dbo].[Discipline]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Publication_Id] in table 'DisciplinePublication'
ALTER TABLE [dbo].[DisciplinePublication]
ADD CONSTRAINT [FK_DisciplinePublication_Publication]
    FOREIGN KEY ([Publication_Id])
    REFERENCES [dbo].[Publication]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DisciplinePublication_Publication'
CREATE INDEX [IX_FK_DisciplinePublication_Publication]
ON [dbo].[DisciplinePublication]
    ([Publication_Id]);
GO

-- Creating foreign key on [Publication_Id] in table 'Stats'
ALTER TABLE [dbo].[Stats]
ADD CONSTRAINT [FK_PublicationStats]
    FOREIGN KEY ([Publication_Id])
    REFERENCES [dbo].[Publication]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PublicationStats'
CREATE INDEX [IX_FK_PublicationStats]
ON [dbo].[Stats]
    ([Publication_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------