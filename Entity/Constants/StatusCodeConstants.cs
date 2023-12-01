
namespace Entity.Constants
{
    public static class StatusCodeConstants
    {
       
        public static readonly int ERROR_SERVER = 500;
        public static readonly int BAD_REQUEST = 400;
        public static readonly int NOT_FOUND = 404;
        public static readonly int AUTHORIZATION = 401;
        public static readonly int FORBIDDEN = 403;
        public static readonly int CREATED = 201;
        public static readonly int OK = 200;

    }
    public static class Validations
    {
        public static readonly int MaximumNumberOfDegrees = 3;
        public static readonly int MaxLenghtName = 250;
        public static readonly int MinAge = 0;
        public static readonly int MaxAge = 150;
        public static readonly int MaxLenghtNameDegree = 250;
        public static readonly int MaxLeghtIssuedBy = 550;


    }
}