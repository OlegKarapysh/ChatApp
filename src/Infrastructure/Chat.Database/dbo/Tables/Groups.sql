CREATE TABLE [dbo].[Groups] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (100)  NOT NULL,
    [Instructions] NVARCHAR (1000) NOT NULL,
    [AssistantId]  NVARCHAR (200)  NOT NULL,
    [CreatorId]    INT             NOT NULL,
    CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Groups_AspNetUsers_CreatorId] FOREIGN KEY ([CreatorId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Groups_CreatorId]
    ON [dbo].[Groups]([CreatorId] ASC);

