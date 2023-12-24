namespace Chat.UnitTests.TestHelpers;

public static class MockHelper
{
    public static Mock<UserManager<User>> MockUserManager()
    {
        var store = new Mock<IUserStore<User>>();
        var mock = new Mock<UserManager<User>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        mock.Object.UserValidators.Add(new UserValidator<User>());
        mock.Object.PasswordValidators.Add(new PasswordValidator<User>());
        return mock;
    }
}