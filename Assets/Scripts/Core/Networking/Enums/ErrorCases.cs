using System;

[Serializable]
public enum ErrorCases
{
    UserNameInUse,
    UserDoesNotExist,
    IncorrectPassword,
    AllGood,
    UserMismatch,
    UnknownError,
    IncorrectEmail,
    OtpIncorrect,
    OtpExpired,
    AccountNotVerified,
    NoChangesToSave
}