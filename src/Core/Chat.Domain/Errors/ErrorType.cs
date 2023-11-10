namespace Chat.Domain.Errors;

public enum ErrorType
{
    InvalidEmail = 1,
    InvalidUsername,
    InvalidPassword,
    InvalidEmailOrPassword,
    InvalidToken,
    NotFound,
    Internal,
    TokenExpired,
    Unknown
}