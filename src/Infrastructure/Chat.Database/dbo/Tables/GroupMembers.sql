CREATE TABLE [dbo].[GroupMembers] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [UserId]   INT            NULL,
    [GroupId]  INT            NULL,
    [ThreadId] NVARCHAR (200) NULL,
    CONSTRAINT [PK_GroupMembers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GroupMembers_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_GroupMembers_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Groups] ([Id])
);






GO
CREATE NONCLUSTERED INDEX [IX_GroupMembers_UserId]
    ON [dbo].[GroupMembers]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_GroupMembers_GroupId]
    ON [dbo].[GroupMembers]([GroupId] ASC);

