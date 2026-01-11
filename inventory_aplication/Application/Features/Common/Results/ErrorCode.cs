namespace inventory_aplication.Application.Features.Common.Results
{
    public static class ErrorCodes
    {
        public const string UserNotFound = "USER_NOT_FOUND";
        public const string InvalidCredentials = "INVALID_CREDENTIALS";
        public const string UserInactive = "USER_INACTIVE";
        public const string InternalError = "INTERNAL_ERROR";
        public const string ExistingUser = "EXISTING_USER";
        public const string NoFound = "NO_FOUND";
        public const string BadRequest = "BAD_REQUEST";
        public const string ExistingItem = "EXISTING_ITEM";
        public const string NotFound = "ITEM_NOT_FOUND";
        public const string InvalidOperation = "INVALID_OPERATION";

    }
}
