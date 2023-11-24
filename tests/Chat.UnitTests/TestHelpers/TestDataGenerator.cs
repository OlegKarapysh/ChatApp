namespace Chat.UnitTests.TestHelpers;

public static class TestDataGenerator
{
    public static readonly DateTime DefaultDate = DateTime.Parse("01.09.2023");
    private const int RefreshTokenLength = 44;
    
    static TestDataGenerator()
    {
        const int defaultSeed = 123456;
        Randomizer.Seed = new Random(defaultSeed);
    }
    
    public static List<User> GenerateUsers(int count)
    {
        return new Faker<User>()
               .RuleFor(x => x.Id, f => f.IndexFaker)!
               .RuleFor(x => x.FirstName, f => f.Name!.FirstName())!
               .RuleFor(x => x.LastName, f => f.Name!.LastName())!
               .RuleFor(x => x.UserName, f => f.Internet!.UserName())!
               .RuleFor(x => x.Email, f => f.Internet!.Email())!
               .RuleFor(x => x.TokenExpiresAt, f => f.Date!.Future(1, DefaultDate))!
               .RuleFor(x => x.RefreshToken, f => f.Random!.AlphaNumeric(RefreshTokenLength))!
               .RuleFor(x => x.PhoneNumber, f => f.Phone!.PhoneNumber()!)!
               .RuleFor(x => x.CreatedAt, f => DefaultDate)!.Generate(count)!;
    }
}