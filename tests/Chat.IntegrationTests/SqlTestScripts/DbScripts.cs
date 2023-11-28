namespace Chat.IntegrationTests.SqlTestScripts;

public class DbScripts
{
    public const string DeleteAllData =
        """
        DELETE FROM [dbo].[Attachments]
        DELETE FROM [dbo].[Messages]
        DELETE FROM [dbo].[ConversationParticipants]
        DELETE FROM [dbo].[Conversations]
        DELETE FROM [dbo].[AspNetUserTokens]
        DELETE FROM [dbo].[AspNetUserLogins]
        DELETE FROM [dbo].[AspNetUserClaims]
        DELETE FROM [dbo].[AspNetUserRoles]
        DELETE FROM [dbo].[AspNetUsers]
        DELETE FROM [dbo].[AspNetRoleClaims]
        DELETE FROM [dbo].[AspNetRoles]
        """;

    public const string SeedTestData =
        """
        SET IDENTITY_INSERT [dbo].[AspNetUsers] ON 
        INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [AvatarUrl], [RefreshToken], [TokenExpiresAt], [CreatedAt], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (1, N'Oleh', N'Karapysh', NULL, N'zedXN98t88bYCNZ0jU3nWKuqSo01Mtmn/Xnrbh9bnfg=', CAST(N'2023-11-30T07:51:28.6912393' AS DateTime2), CAST(N'2023-11-13T17:49:14.4862915' AS DateTime2), N'OlehKarapysh', N'OLEHKARAPYSH', N'oleh@a.a', N'OLEH@A.A', 0, N'AQAAAAIAAYagAAAAEFlfrTgjyBvSk1pRB44dx/S2wAaOCbcD8V7+Ecg5oXTDsYvaWhlvVwaZpFcnJgbFkQ==', N'C7PATH3P44DWWA6LK7XU4ESA3FXYTOYR', N'e5451b00-8245-4e43-a110-67cf3c6e6bc4', N'+380412341234', 0, 0, NULL, 1, 0)
        INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [AvatarUrl], [RefreshToken], [TokenExpiresAt], [CreatedAt], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (2, N'Nick', N'Chaps', NULL, N'qsXBz+cEnv8ZDVY9ZoCoYIE/lG/LfC1gSmVO330pd6o=', CAST(N'2023-11-22T12:05:21.9546845' AS DateTime2), CAST(N'2023-11-13T17:49:58.6873838' AS DateTime2), N'NickChaps', N'NICKCHAPS', N'a@a.a', N'A@A.A', 0, N'AQAAAAIAAYagAAAAEI7emho8QojyEgTphirOBLvwtYTZzwT8tGr8ZheVrSN0EZ4r/AGXcSwR+DYoKcK4Cw==', N'6MT32M367UURHCER3I74QVNWEY664R6E', N'51f893b3-3361-4953-93b8-76e350315ccb', N'+2390423423', 0, 0, NULL, 1, 0)
        INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [AvatarUrl], [RefreshToken], [TokenExpiresAt], [CreatedAt], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (3, N'Bill', N'Lie', NULL, N'FjJY9iWQ0OT/rMikXum+ephqFNpwbhEJdmFNSpLa8os=', CAST(N'2023-11-30T07:49:03.8409260' AS DateTime2), CAST(N'2023-11-14T11:38:15.6054106' AS DateTime2), N'Billy', N'BILLY', N'a@a.a2', N'A@A.A2', 0, N'AQAAAAIAAYagAAAAEMR9bdlnYblSuVmYXocpa9V10BDlzt29E8jC6pBSot1qNDZihdV0eyUZMOCsABE3ZA==', N'VSMPM3OOKSMIRPXYIW67TFUM3OZCK2I5', N'5568c3ff-eee4-4e51-972e-a45d6755b1de', N'+3800342342342', 0, 0, NULL, 1, 0)
        INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [AvatarUrl], [RefreshToken], [TokenExpiresAt], [CreatedAt], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (4, N'Alice', N'Bobkova', NULL, N'8BAe8wbXIKKMM9ZhXPWo3bzI2KhlqZrt2qbfYKpaI78=', CAST(N'2023-11-30T07:49:30.6333291' AS DateTime2), CAST(N'2023-11-14T14:39:00.3595738' AS DateTime2), N'Alice', N'ALICE', N'a@a.a3', N'A@A.A3', 0, N'AQAAAAIAAYagAAAAEE0Y7HJlLMeOkj9kAV+MYewu6pyvA28CnW6ju52psqqMQQ+TR4Gpv7gNjVD6i01igA==', N'FF3LURW7GJ2RJQZO4SUJBA6LPYKFE3MJ', N'52923622-b877-44fb-a748-b3cd90882877', N'+2342341345', 0, 0, NULL, 1, 0)
        INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [AvatarUrl], [RefreshToken], [TokenExpiresAt], [CreatedAt], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (5, N'Of', N'Persia', NULL, N'ylZIr1SQwrjTMj7jk6rYX5qkLX1NOoS5Vox9/Q/8hXE=', CAST(N'2023-11-30T07:50:04.8489565' AS DateTime2), CAST(N'2023-11-28T07:46:10.7355849' AS DateTime2), N'Prince', N'PRINCE', N'prince@a.a', N'PRINCE@A.A', 0, N'AQAAAAIAAYagAAAAEMoKd1UZyjhmaqPYzyFPAeyN3A37LaBkm//czHvZLM2KCEKWybMbFaAp+XZpVH+WGQ==', N'U4NM2PUCG7LMHFMJE5AIJESXXLZD34G2', N'db8da4e6-2188-49f6-9b0e-dd75556b520e', N'+23092302323', 0, 0, NULL, 1, 0)
        INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [AvatarUrl], [RefreshToken], [TokenExpiresAt], [CreatedAt], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (6, NULL, NULL, NULL, N'CGzZVUJeUMX1yqehLoRtuysuarOxH0o4hdh4if8NnPU=', CAST(N'2023-11-30T07:48:09.4715835' AS DateTime2), CAST(N'2023-11-28T07:48:09.4715845' AS DateTime2), N'user4', N'USER4', N'a@a.a4', N'A@A.A4', 0, N'AQAAAAIAAYagAAAAEE4Rnc3gLlRSfxqrwaRYaYgxLe62u8GeJin3qwWawFdpaytglHZ8nH7adzkmRu1g==', N'OLAJBHEGTS7MTR6TFBIZMSEXHQRA4OXT', N'01a0c009-0d94-4ce2-bee9-fb9bc3ccb76e', NULL, 0, 0, NULL, 1, 0)
        INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [AvatarUrl], [RefreshToken], [TokenExpiresAt], [CreatedAt], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (7, NULL, NULL, NULL, N'aUq+BBL6UQZZlfPxmwG/ECubJqN3B+UwV5uPIJruhr8=', CAST(N'2023-11-30T07:48:34.4926905' AS DateTime2), CAST(N'2023-11-28T07:48:34.4926920' AS DateTime2), N'user5', N'USER5', N'a@a.a5`', N'A@A.A5`', 0, N'AQAAAAIAAYagAAAAECszHClihxa+iNNKzEQdFP75hFi6XRciThegy/fL4SN9EPD+bdQQ9XydhlNJNAc8YA==', N'M52U3TSKTCFMSGSRR67BNEXFJW7XLUIT', N'57c91a55-85dc-4721-b90a-d4df243e07f8', NULL, 0, 0, NULL, 1, 0)
        SET IDENTITY_INSERT [dbo].[AspNetUsers] OFF
        SET IDENTITY_INSERT [dbo].[Conversations] ON 
        INSERT [dbo].[Conversations] ([Id], [Title], [Type], [CreatedAt], [UpdatedAt]) VALUES (16, N'OlehKarapysh - Billy Dialog', 0, CAST(N'2023-11-19T18:11:20.3367615' AS DateTime2), CAST(N'2023-11-19T18:11:20.3367630' AS DateTime2))
        INSERT [dbo].[Conversations] ([Id], [Title], [Type], [CreatedAt], [UpdatedAt]) VALUES (17, N'gr1', 1, CAST(N'2023-11-19T18:13:45.3034359' AS DateTime2), CAST(N'2023-11-19T18:13:45.3034360' AS DateTime2))
        INSERT [dbo].[Conversations] ([Id], [Title], [Type], [CreatedAt], [UpdatedAt]) VALUES (18, N'gr2', 1, CAST(N'2023-11-19T18:19:30.0651799' AS DateTime2), CAST(N'2023-11-19T18:19:30.0651800' AS DateTime2))
        INSERT [dbo].[Conversations] ([Id], [Title], [Type], [CreatedAt], [UpdatedAt]) VALUES (19, N'Alice - OlehKarapysh Dialog', 0, CAST(N'2023-11-28T07:47:09.4623856' AS DateTime2), CAST(N'2023-11-28T07:47:09.4623860' AS DateTime2))
        INSERT [dbo].[Conversations] ([Id], [Title], [Type], [CreatedAt], [UpdatedAt]) VALUES (20, N'billy''s group', 1, CAST(N'2023-11-28T07:49:12.9135443' AS DateTime2), CAST(N'2023-11-28T07:49:12.9135443' AS DateTime2))
        INSERT [dbo].[Conversations] ([Id], [Title], [Type], [CreatedAt], [UpdatedAt]) VALUES (21, N'alice group', 1, CAST(N'2023-11-28T07:49:50.4452768' AS DateTime2), CAST(N'2023-11-28T07:49:50.4452768' AS DateTime2))
        INSERT [dbo].[Conversations] ([Id], [Title], [Type], [CreatedAt], [UpdatedAt]) VALUES (22, N'Prince - Billy Dialog', 0, CAST(N'2023-11-28T07:50:21.2842834' AS DateTime2), CAST(N'2023-11-28T07:50:21.2842835' AS DateTime2))
        INSERT [dbo].[Conversations] ([Id], [Title], [Type], [CreatedAt], [UpdatedAt]) VALUES (23, N'Prince - OlehKarapysh Dialog', 0, CAST(N'2023-11-28T07:50:58.4869758' AS DateTime2), CAST(N'2023-11-28T07:50:58.4869759' AS DateTime2))
        SET IDENTITY_INSERT [dbo].[Conversations] OFF
        SET IDENTITY_INSERT [dbo].[ConversationParticipants] ON 
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (29, 16, 1, 0)
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (30, 16, 3, 0)
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (31, 17, 3, 0)
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (32, 17, 3, 0)
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (33, 17, 1, 0)
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (34, 18, 1, 0)
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (36, 17, 2, 0)
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (37, 19, 4, 0)
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (38, 19, 1, 0)
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (39, 18, 7, 0)
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (40, 20, 3, 0)
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (41, 20, 4, 0)
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (42, 20, 6, 0)
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (43, 21, 4, 0)
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (44, 21, 5, 0)
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (45, 22, 5, 0)
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (46, 22, 3, 0)
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (47, 23, 5, 0)
        INSERT [dbo].[ConversationParticipants] ([Id], [ConversationId], [UserId], [MembershipType]) VALUES (48, 23, 1, 0)
        SET IDENTITY_INSERT [dbo].[ConversationParticipants] OFF
        SET IDENTITY_INSERT [dbo].[Messages] ON 
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (109, N'1', 0, 1, 17, CAST(N'2023-11-19T18:17:03.8835002' AS DateTime2), CAST(N'2023-11-19T18:17:03.8835007' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (110, N'2', 0, 1, 17, CAST(N'2023-11-19T18:17:13.7880675' AS DateTime2), CAST(N'2023-11-19T18:17:13.7880683' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (111, N'3', 0, 3, 17, CAST(N'2023-11-19T18:17:20.4698125' AS DateTime2), CAST(N'2023-11-19T18:17:20.4698136' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (112, N'asd', 0, 1, 18, CAST(N'2023-11-19T18:19:50.9313614' AS DateTime2), CAST(N'2023-11-19T18:19:50.9313618' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (113, N'asdf', 0, 1, 18, CAST(N'2023-11-19T18:20:05.4121396' AS DateTime2), CAST(N'2023-11-19T18:20:05.4121401' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (119, N'hi again', 0, 3, 16, CAST(N'2023-11-20T12:01:46.5100096' AS DateTime2), CAST(N'2023-11-20T12:01:46.5100110' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (121, N'4', 0, 2, 17, CAST(N'2023-11-20T12:05:45.2981458' AS DateTime2), CAST(N'2023-11-20T12:05:45.2981464' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (124, N'1', 0, 3, 16, CAST(N'2023-11-20T17:01:47.9976603' AS DateTime2), CAST(N'2023-11-20T17:01:47.9976611' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (126, N'2', 0, 1, 16, CAST(N'2023-11-20T17:02:06.4549192' AS DateTime2), CAST(N'2023-11-20T17:02:06.4549196' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (127, N'ok?', 0, 3, 16, CAST(N'2023-11-20T17:25:59.8769513' AS DateTime2), CAST(N'2023-11-20T17:25:59.8769527' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (128, N'ok!', 0, 1, 16, CAST(N'2023-11-20T17:26:10.5486460' AS DateTime2), CAST(N'2023-11-20T17:26:10.5486464' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (129, N'Hey 1', 0, 4, 19, CAST(N'2023-11-28T07:47:19.6932114' AS DateTime2), CAST(N'2023-11-28T07:47:19.6932117' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (130, N'thanks for adding to group', 0, 5, 21, CAST(N'2023-11-28T07:50:14.2051999' AS DateTime2), CAST(N'2023-11-28T07:50:14.2052000' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (131, N'Hey Billy', 0, 5, 22, CAST(N'2023-11-28T07:50:28.1284424' AS DateTime2), CAST(N'2023-11-28T07:50:28.1284425' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (132, N'Hey Oleh', 0, 5, 23, CAST(N'2023-11-28T07:51:10.1868611' AS DateTime2), CAST(N'2023-11-28T07:51:10.1868611' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (133, N'How are you?', 0, 5, 23, CAST(N'2023-11-28T07:51:16.2527356' AS DateTime2), CAST(N'2023-11-28T07:51:16.2527356' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (134, N'Hey, fine', 0, 1, 23, CAST(N'2023-11-28T07:51:41.0657591' AS DateTime2), CAST(N'2023-11-28T07:51:41.0657593' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (135, N'Hey 2', 0, 1, 19, CAST(N'2023-11-28T07:52:23.6381581' AS DateTime2), CAST(N'2023-11-28T07:52:23.6381582' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (136, N'something strange in this group', 0, 1, 18, CAST(N'2023-11-28T07:52:37.5051749' AS DateTime2), CAST(N'2023-11-28T07:52:37.5051751' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (137, N'5', 0, 1, 17, CAST(N'2023-11-28T07:52:44.1853532' AS DateTime2), CAST(N'2023-11-28T07:52:44.1853532' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (138, N'6', 0, 1, 17, CAST(N'2023-11-28T07:53:10.3869084' AS DateTime2), CAST(N'2023-11-28T07:53:10.3869086' AS DateTime2))
        INSERT [dbo].[Messages] ([Id], [TextContent], [IsRead], [SenderId], [ConversationId], [CreatedAt], [UpdatedAt]) VALUES (139, N'once upon a time', 0, 1, 16, CAST(N'2023-11-28T07:53:33.7150798' AS DateTime2), CAST(N'2023-11-28T07:53:33.7150798' AS DateTime2))
        SET IDENTITY_INSERT [dbo].[Messages] OFF
        """;
}