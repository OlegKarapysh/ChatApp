namespace Chat.UnitTests.TestHelpers;

internal static class DbSeedHelper
{
    internal static void RecreateAndSeedDb(ChatDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        context.Database.ExecuteSqlRaw(
            """
            INSERT INTO AspNetUsers (Id, FirstName, LastName, AvatarUrl, RefreshToken, TokenExpiresAt, CreatedAt, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
            VALUES (1, 'Oleh', 'Karapysh', NULL, 'ALIGbNR8TfEIb2tyMEseZmgiNcMcFm33B/Sisa3zhJE=', '2023-11-23T14:34:29.8394322', '2023-11-13T17:49:14.4862915', 'OlehKarapysh', 'OLEHKARAPYSH', 'oleh@a.a', 'OLEH@A.A', 0, 'AQAAAAIAAYagAAAAEFlfrTgjyBvSk1pRB44dx/S2wAaOCbcD8V7+Ecg5oXTDsYvaWhlvVwaZpFcnJgbFkQ==', 'C7PATH3P44DWWA6LK7XU4ESA3FXYTOYR', '19af510e-9db2-4d3a-913a-7cbf3bc524c6', '+380412341234', 0, 0, NULL, 1, 0);
            INSERT INTO AspNetUsers (Id, FirstName, LastName, AvatarUrl, RefreshToken, TokenExpiresAt, CreatedAt, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
            VALUES (2, 'Nick', 'Chaps', NULL, 'qsXBz+cEnv8ZDVY9ZoCoYIE/lG/LfC1gSmVO330pd6o=', '2023-11-22T12:05:21.9546845', '2023-11-13T17:49:58.6873838', 'NickChaps', 'NICKCHAPS', 'a@a.a', 'A@A.A', 0, 'AQAAAAIAAYagAAAAEI7emho8QojyEgTphirOBLvwtYTZzwT8tGr8ZheVrSN0EZ4r/AGXcSwR+DYoKcK4Cw==', '6MT32M367UURHCER3I74QVNWEY664R6E', '51f893b3-3361-4953-93b8-76e350315ccb', '+2390423423', 0, 0, NULL, 1, 0);
            INSERT INTO AspNetUsers (Id, FirstName, LastName, AvatarUrl, RefreshToken, TokenExpiresAt, CreatedAt, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
            VALUES (3, 'Bill', 'Lie', NULL, 'f4T600byDAd07tDOtqCqn3Y+KZYkWbjtpp/lerwq2xc=', '2023-11-23T14:34:10.8727304', '2023-11-14T11:38:15.6054106', 'Billy', 'BILLY', 'a@a.a2', 'A@A.A2', 0, 'AQAAAAIAAYagAAAAEMR9bdlnYblSuVmYXocpa9V10BDlzt29E8jC6pBSot1qNDZihdV0eyUZMOCsABE3ZA==', 'VSMPM3OOKSMIRPXYIW67TFUM3OZCK2I5', '700979fa-7e8a-4ac6-b540-3c3acb28dbbc', '+3800342342342', 0, 0, NULL, 1, 0);
            INSERT INTO AspNetUsers (Id, FirstName, LastName, AvatarUrl, RefreshToken, TokenExpiresAt, CreatedAt, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
            VALUES (4, NULL, NULL, NULL, 'B4nOZZVkZ/n+dUT3fa/GB1R/FYs7Z7gT/De7Ojl7FnM=', '2023-11-16T14:39:00.3595711', '2023-11-14T14:39:00.3595738', 'Alice', 'ALICE', 'a@a.a3', 'A@A.A3', 0, 'AQAAAAIAAYagAAAAEE0Y7HJlLMeOkj9kAV+MYewu6pyvA28CnW6ju52psqqMQQ+TR4Gpv7gNjVD6i01igA==', 'FF3LURW7GJ2RJQZO4SUJBA6LPYKFE3MJ', 'e2f6d2b5-e72c-4d7c-8ffb-0af96aeb5d00', NULL, 0, 0, NULL, 1, 0);
            INSERT INTO Conversations (Id, Title, Type, CreatedAt, UpdatedAt)
            VALUES (16, 'OlehKarapysh - Billy Dialog', 0, '2023-11-19T18:11:20.3367615', '2023-11-19T18:11:20.3367630');
            INSERT INTO Conversations (Id, Title, Type, CreatedAt, UpdatedAt)
            VALUES (17, 'gr1', 1, '2023-11-19T18:13:45.3034359', '2023-11-19T18:13:45.3034360');
            INSERT INTO Conversations (Id, Title, Type, CreatedAt, UpdatedAt)
            VALUES (18, 'gr2', 1, '2023-11-19T18:19:30.0651799', '2023-11-19T18:19:30.0651800');
            INSERT INTO ConversationParticipants (Id, ConversationId, UserId, MembershipType)
            VALUES (29, 16, 1, 0);
            INSERT INTO ConversationParticipants (Id, ConversationId, UserId, MembershipType)
            VALUES (30, 16, 3, 0);
            INSERT INTO ConversationParticipants (Id, ConversationId, UserId, MembershipType)
            VALUES (31, 17, 3, 0);
            INSERT INTO ConversationParticipants (Id, ConversationId, UserId, MembershipType)
            VALUES (32, 17, 3, 0);
            INSERT INTO ConversationParticipants (Id, ConversationId, UserId, MembershipType)
            VALUES (33, 17, 1, 0);
            INSERT INTO ConversationParticipants (Id, ConversationId, UserId, MembershipType)
            VALUES (34, 18, 1, 0);
            INSERT INTO ConversationParticipants (Id, ConversationId, UserId, MembershipType)
            VALUES (36, 17, 2, 0);
            INSERT INTO Messages (Id, TextContent, IsRead, SenderId, ConversationId, CreatedAt, UpdatedAt)
            VALUES (109, '1', 0, 1, 17, '2023-11-19T18:17:03.8835002', '2023-11-19T18:17:03.8835007');
            INSERT INTO Messages (Id, TextContent, IsRead, SenderId, ConversationId, CreatedAt, UpdatedAt)
            VALUES (110, '2', 0, 1, 17, '2023-11-19T18:17:13.7880675', '2023-11-19T18:17:13.7880683');
            INSERT INTO Messages (Id, TextContent, IsRead, SenderId, ConversationId, CreatedAt, UpdatedAt)
            VALUES (111, '3', 0, 3, 17, '2023-11-19T18:17:20.4698125', '2023-11-19T18:17:20.4698136');
            INSERT INTO Messages (Id, TextContent, IsRead, SenderId, ConversationId, CreatedAt, UpdatedAt)
            VALUES (112, 'asd', 0, 1, 18, '2023-11-19T18:19:50.9313614', '2023-11-19T18:19:50.9313618');
            INSERT INTO Messages (Id, TextContent, IsRead, SenderId, ConversationId, CreatedAt, UpdatedAt)
            VALUES (113, 'asdf', 0, 1, 18, '2023-11-19T18:20:05.4121396', '2023-11-19T18:20:05.4121401');
            INSERT INTO Messages (Id, TextContent, IsRead, SenderId, ConversationId, CreatedAt, UpdatedAt)
            VALUES (119, 'hi again', 0, 3, 16, '2023-11-20T12:01:46.5100096', '2023-11-20T12:01:46.5100110');
            INSERT INTO Messages (Id, TextContent, IsRead, SenderId, ConversationId, CreatedAt, UpdatedAt)
            VALUES (121, '4', 0, 2, 17, '2023-11-20T12:05:45.2981458', '2023-11-20T12:05:45.2981464');
            INSERT INTO Messages (Id, TextContent, IsRead, SenderId, ConversationId, CreatedAt, UpdatedAt)
            VALUES (124, '1', 0, 3, 16, '2023-11-20T17:01:47.9976603', '2023-11-20T17:01:47.9976611');
            INSERT INTO Messages (Id, TextContent, IsRead, SenderId, ConversationId, CreatedAt, UpdatedAt)
            VALUES (126, '2', 0, 1, 16, '2023-11-20T17:02:06.4549192', '2023-11-20T17:02:06.4549196');
            INSERT INTO Messages (Id, TextContent, IsRead, SenderId, ConversationId, CreatedAt, UpdatedAt)
            VALUES (127, 'ok?', 0, 3, 16, '2023-11-20T17:25:59.8769513', '2023-11-20T17:25:59.8769527');
            INSERT INTO Messages (Id, TextContent, IsRead, SenderId, ConversationId, CreatedAt, UpdatedAt)
            VALUES (128, 'ok!', 0, 1, 16, '2023-11-20T17:26:10.5486460', '2023-11-20T17:26:10.5486464');
            """);
        context.SaveChanges();
    }
}