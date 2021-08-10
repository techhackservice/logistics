

namespace Web.Api.Core.Dto
{
    public sealed class Error
    {
        public string Code { get; }
        public string Description { get; }

        public int ErrorId { get; }

        public Error(string code, string description, int errorId = 0)
        {
            Code = code;
            Description = description;
            ErrorId = errorId;
        }
    }
}
