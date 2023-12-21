CREATE TABLE [dbo].[ConversationParticipants] (
    [Id]             INT IDENTITY (1, 1) NOT NULL,
    [ConversationId] INT NULL,
    [UserId]         INT NULL,
    [MembershipType] INT NOT NULL,
    CONSTRAINT [PK_ConversationParticipants] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ConversationParticipants_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_ConversationParticipants_Conversations_ConversationId] FOREIGN KEY ([ConversationId]) REFERENCES [dbo].[Conversations] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_ConversationParticipants_UserId]
    ON [dbo].[ConversationParticipants]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ConversationParticipants_ConversationId]
    ON [dbo].[ConversationParticipants]([ConversationId] ASC);

