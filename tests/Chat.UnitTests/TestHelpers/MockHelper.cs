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

    public static Mock<SignInManager<User>> MockSignInManager(UserManager<User> userManager)
    {
        return new Mock<SignInManager<User>>(userManager, Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<User>>(), null!, null!, null!, null!);
    }
}