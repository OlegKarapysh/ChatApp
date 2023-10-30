CREATE TABLE [dbo].[Messages] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [SenderId]       INT            NULL,
    [ConversationId] INT            NULL,
    [TextContent]    NVARCHAR (MAX) NOT NULL,
    [CreatedAt]      DATETIME2 (7)  NOT NULL,
    [UpdatedAt]      DATETIME2 (7)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK__Messages__Conversations] FOREIGN KEY ([ConversationId]) REFERENCES [dbo].[Conversations] ([Id]),
    CONSTRAINT [FK__Messages__Sender] FOREIGN KEY ([SenderId]) REFERENCES [dbo].[Users] ([Id])
);

