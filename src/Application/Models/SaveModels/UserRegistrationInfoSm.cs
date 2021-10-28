using System;

namespace YA.WebClient.Application.Models.SaveModels
{
    public class UserRegistrationInfoSm
    {
        public string AccessToken { get; set; }
        public Guid? JoinTeamToken { get; set; }
    }
}
