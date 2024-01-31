namespace QLTS.Application.UserCases.jwt
{
    public class ClaimJwtResultModel
    {
        public string UserName { get; private set; } = string.Empty;
        public int ComId { get; private set; }

        public ClaimJwtResultModel(int comId, string username)
        {
            ComId = comId;
            UserName = username;
        }
    }
}
