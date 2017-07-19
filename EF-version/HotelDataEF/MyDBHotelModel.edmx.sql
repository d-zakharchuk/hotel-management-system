
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/05/2017 10:08:28
-- Generated from EDMX file: C:\Users\PRIVATE\Downloads\HotelManagement Entity 3 Mar\HotelManagement\HotelDataEF\HotelModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [HotelManagementDatabase];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Reservation_Client_Relation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reservation] DROP CONSTRAINT [FK_Reservation_Client_Relation];
GO
IF OBJECT_ID(N'[dbo].[FK_Reservation_M2M_Relation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Res_serv] DROP CONSTRAINT [FK_Reservation_M2M_Relation];
GO
IF OBJECT_ID(N'[dbo].[FK_Reservation_Room_Relation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reservation] DROP CONSTRAINT [FK_Reservation_Room_Relation];
GO
IF OBJECT_ID(N'[dbo].[FK_Room-Type-Relation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Room] DROP CONSTRAINT [FK_Room-Type-Relation];
GO
IF OBJECT_ID(N'[dbo].[FK_Service_M2M_Relation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Res_serv] DROP CONSTRAINT [FK_Service_M2M_Relation];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Client]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Client];
GO
IF OBJECT_ID(N'[dbo].[HotelIncome]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HotelIncome];
GO
IF OBJECT_ID(N'[dbo].[Login]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Login];
GO
IF OBJECT_ID(N'[dbo].[Res_serv]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Res_serv];
GO
IF OBJECT_ID(N'[dbo].[Reservation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reservation];
GO
IF OBJECT_ID(N'[dbo].[Room]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Room];
GO
IF OBJECT_ID(N'[dbo].[Services]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Services];
GO
IF OBJECT_ID(N'[dbo].[Type]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Type];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Client'
CREATE TABLE [dbo].[Client] (
    [Client_id] varchar(10)  NOT NULL,
    [Client_first_name] varchar(30)  NOT NULL,
    [Client_middle_name] varchar(30)  NULL,
    [Client_last_name] varchar(30)  NOT NULL,
    [Client_date_of_birth] datetime  NOT NULL,
    [Client_tel] varchar(30)  NULL,
    [Client_country] varchar(30)  NULL
);
GO

-- Creating table 'HotelIncome'
CREATE TABLE [dbo].[HotelIncome] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Room_num] int  NOT NULL,
    [Residents] int  NOT NULL,
    [Date_in] datetime  NOT NULL,
    [Date_out] datetime  NOT NULL,
    [Room_cost] float  NOT NULL,
    [Services_cost] float  NOT NULL,
    [Total] float  NOT NULL
);
GO

-- Creating table 'Login'
CREATE TABLE [dbo].[Login] (
    [Id] int  NOT NULL,
    [Login1] varchar(max)  NOT NULL,
    [Password] varchar(max)  NOT NULL
);
GO

-- Creating table 'Res_serv'
CREATE TABLE [dbo].[Res_serv] (
    [Id_res] varchar(10)  NOT NULL,
    [Id_serv] int  NOT NULL,
    [Id_res_serv] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Reservation'
CREATE TABLE [dbo].[Reservation] (
    [Res_id] varchar(10)  NOT NULL,
    [Client_id] varchar(10)  NOT NULL,
    [Room_num] int  NOT NULL,
    [Date_in] datetime  NOT NULL,
    [Services] varchar(max)  NOT NULL
);
GO

-- Creating table 'Room'
CREATE TABLE [dbo].[Room] (
    [room_num] int  NOT NULL,
    [type] varchar(10)  NOT NULL,
    [floor] int  NOT NULL,
    [people] int  NULL
);
GO

-- Creating table 'Services'
CREATE TABLE [dbo].[Services] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Service] varchar(30)  NOT NULL,
    [Price] float  NOT NULL
);
GO

-- Creating table 'Type'
CREATE TABLE [dbo].[Type] (
    [type1] varchar(10)  NOT NULL,
    [C_people] int  NULL,
    [Price_1] float  NULL,
    [Price] float  NULL,
    [Area] float  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Client_id] in table 'Client'
ALTER TABLE [dbo].[Client]
ADD CONSTRAINT [PK_Client]
    PRIMARY KEY CLUSTERED ([Client_id] ASC);
GO

-- Creating primary key on [Id] in table 'HotelIncome'
ALTER TABLE [dbo].[HotelIncome]
ADD CONSTRAINT [PK_HotelIncome]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Login'
ALTER TABLE [dbo].[Login]
ADD CONSTRAINT [PK_Login]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id_res_serv] in table 'Res_serv'
ALTER TABLE [dbo].[Res_serv]
ADD CONSTRAINT [PK_Res_serv]
    PRIMARY KEY CLUSTERED ([Id_res_serv] ASC);
GO

-- Creating primary key on [Res_id] in table 'Reservation'
ALTER TABLE [dbo].[Reservation]
ADD CONSTRAINT [PK_Reservation]
    PRIMARY KEY CLUSTERED ([Res_id] ASC);
GO

-- Creating primary key on [room_num] in table 'Room'
ALTER TABLE [dbo].[Room]
ADD CONSTRAINT [PK_Room]
    PRIMARY KEY CLUSTERED ([room_num] ASC);
GO

-- Creating primary key on [Id] in table 'Services'
ALTER TABLE [dbo].[Services]
ADD CONSTRAINT [PK_Services]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [type1] in table 'Type'
ALTER TABLE [dbo].[Type]
ADD CONSTRAINT [PK_Type]
    PRIMARY KEY CLUSTERED ([type1] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Client_id] in table 'Reservation'
ALTER TABLE [dbo].[Reservation]
ADD CONSTRAINT [FK_Reservation_Client_Relation]
    FOREIGN KEY ([Client_id])
    REFERENCES [dbo].[Client]
        ([Client_id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Reservation_Client_Relation'
CREATE INDEX [IX_FK_Reservation_Client_Relation]
ON [dbo].[Reservation]
    ([Client_id]);
GO

-- Creating foreign key on [Id_res] in table 'Res_serv'
ALTER TABLE [dbo].[Res_serv]
ADD CONSTRAINT [FK_Reservation_M2M_Relation]
    FOREIGN KEY ([Id_res])
    REFERENCES [dbo].[Reservation]
        ([Res_id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Reservation_M2M_Relation'
CREATE INDEX [IX_FK_Reservation_M2M_Relation]
ON [dbo].[Res_serv]
    ([Id_res]);
GO

-- Creating foreign key on [Id_serv] in table 'Res_serv'
ALTER TABLE [dbo].[Res_serv]
ADD CONSTRAINT [FK_Service_M2M_Relation]
    FOREIGN KEY ([Id_serv])
    REFERENCES [dbo].[Services]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Service_M2M_Relation'
CREATE INDEX [IX_FK_Service_M2M_Relation]
ON [dbo].[Res_serv]
    ([Id_serv]);
GO

-- Creating foreign key on [Room_num] in table 'Reservation'
ALTER TABLE [dbo].[Reservation]
ADD CONSTRAINT [FK_Reservation_Room_Relation]
    FOREIGN KEY ([Room_num])
    REFERENCES [dbo].[Room]
        ([room_num])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Reservation_Room_Relation'
CREATE INDEX [IX_FK_Reservation_Room_Relation]
ON [dbo].[Reservation]
    ([Room_num]);
GO

-- Creating foreign key on [type] in table 'Room'
ALTER TABLE [dbo].[Room]
ADD CONSTRAINT [FK_Room_Type_Relation]
    FOREIGN KEY ([type])
    REFERENCES [dbo].[Type]
        ([type1])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Room_Type_Relation'
CREATE INDEX [IX_FK_Room_Type_Relation]
ON [dbo].[Room]
    ([type]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------