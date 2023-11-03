CREATE TABLE [dbo].[Conversations] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Title]     NVARCHAR (100) NOT NULL,
    [Type]      INT            NOT NULL,
    [CreatedAt] DATETIME2 (7)  NOT NULL,
    [UpdatedAt] DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Conversations] PRIMARY KEY CLUSTERED ([Id] ASC)
);



