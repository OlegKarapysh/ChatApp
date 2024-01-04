CREATE TABLE [dbo].[Messages] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [TextContent]    NVARCHAR (1000) NOT NULL,
    [IsRead]         BIT             NOT NULL,
    [SenderId]       INT             NULL,
    [ConversationId] INT             NULL,
    [CreatedAt]      DATETIME2 (7)   NOT NULL,
    [UpdatedAt]      DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [IsAiAssisted]   BIT             NOT NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Messages_AspNetUsers_SenderId] FOREIGN KEY ([SenderId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Messages_Conversations_ConversationId] FOREIGN KEY ([ConversationId]) REFERENCES [dbo].[Conversations] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_Messages_SenderId]
    ON [dbo].[Messages]([SenderId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Messages_ConversationId]
    ON [dbo].[Messages]([ConversationId] ASC);

