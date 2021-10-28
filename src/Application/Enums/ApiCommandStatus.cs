namespace YA.WebClient.Application.Enums
{
    public enum ApiCommandStatus
    {
        Unknown = 0,
        Ok = 1,
        NotFound = 2,
        ArgumentInvalid = 3,
        BadRequest = 4,
        ModelInvalid = 5,
        UnprocessableEntity = 6,
        ConcurrencyIssue = 7,
        Unauthorized = 8,
        BadGateway = 9,
        ServiceUnavailable = 10,
        UnableToMakeApiCall = 11,
        InternalServerError = 12
    }
}
