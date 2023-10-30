CREATE TABLE [dbo].[Users] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (MAX) NOT NULL,
    [Email]        NVARCHAR (75)  NOT NULL,
    [PasswordSalt] NVARCHAR (255) NULL,
    [PasswordHash] NVARCHAR (255) NULL,
    [AvatarUrl]    NVARCHAR (MAX) NOT NULL,
    [CreatedAt]    DATETIME2 (7)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);





