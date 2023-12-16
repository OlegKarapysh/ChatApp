CREATE TABLE [dbo].[AssistantThreads] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [ThreadId]       NVARCHAR (200) NOT NULL,
    [ConversationId] INT            NOT NULL,
    CONSTRAINT [PK_AssistantThreads] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AssistantThreads_Conversations_ConversationId] FOREIGN KEY ([ConversationId]) REFERENCES [dbo].[Conversations] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_AssistantThreads_ConversationId]
    ON [dbo].[AssistantThreads]([ConversationId] ASC);

