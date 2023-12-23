namespace Chat.Domain.Errors;

public enum ErrorType
{
    InvalidEmail = 1,
    InvalidUsername,
    InvalidPassword,
    InvalidEmailOrPassword,
    InvalidToken,
    InvalidAction,
    InvalidNaming,
    NotFound,
    Internal,
    TokenExpired,
    Unknown
}