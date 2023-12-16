CREATE TABLE [dbo].[AssistantFiles] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [FileId]      NVARCHAR (200) NOT NULL,
    [SizeInBytes] INT            NOT NULL,
    [GroupId]     INT            NOT NULL,
    [CreatedAt]   DATETIME2 (7)  NOT NULL,
    [UpdatedAt]   DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_AssistantFiles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AssistantFiles_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Groups] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AssistantFiles_GroupId]
    ON [dbo].[AssistantFiles]([GroupId] ASC);

