namespace Chat.Domain.Errors;

public enum ErrorType
{
    InvalidEmail = 1,
    InvalidUsername,
    InvalidPassword,
    InvalidEmailOrPassword,
    InvalidToken,
    InvalidAction,
    NotFound,
    Internal,
    TokenExpired,
    Unknown
}