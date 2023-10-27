CREATE TABLE [dbo].[Users] (
    [Id]    INT            NOT NULL,
    [Name]  NVARCHAR (MAX) NOT NULL,
    [Email] NVARCHAR (75)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

