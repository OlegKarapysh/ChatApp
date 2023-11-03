CREATE TABLE [dbo].[Attachments] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [SourceUrl] NVARCHAR (MAX) NOT NULL,
    [Type]      INT            NOT NULL,
    [MessageId] INT            NOT NULL,
    CONSTRAINT [PK_Attachments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Attachments_Messages_MessageId] FOREIGN KEY ([MessageId]) REFERENCES [dbo].[Messages] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Attachments_MessageId]
    ON [dbo].[Attachments]([MessageId] ASC);

