namespace Homework4
{
    public class UserPresenterFactory
    {
        public UserPresenter Create(UserInfo userInfo, PlayerLevel playerLevel, CharacterInfo characterInfo)
        {
            return new UserPresenter(userInfo, playerLevel, characterInfo);
        }
    }
}