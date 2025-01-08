CREATE TABLE [dbo].[AllCountries] (
    countryID int primary key identity(1,1),
    [countryphone]      varchar(20)   NULL,
    [countrycode]       VARCHAR (10)  NULL,
    [countryname]       VARCHAR (100) NULL,
    [countrycodealpha3] VARCHAR (50)  NULL,
    [currency]          VARCHAR (50)  NULL,
    [flag]              VARCHAR (255) NULL,
    [symbol]            VARCHAR (10)  NULL,
);
CREATE TABLE [dbo].[AllStates] (
    [stateid]      INT           IDENTITY (1, 1) NOT NULL,
    [countryID] INT          not NULL,
    [statename]    VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([stateid] ASC),
    FOREIGN KEY ([countryID]) REFERENCES [dbo].[AllCountries] ([countryID])
);

drop table AllCountries