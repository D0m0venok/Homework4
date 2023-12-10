namespace Homework4
{
    public class UserPresenterFactory
    {
        public UserPresenter Create(UserInfo userInfo, PlayerLevel playerLevel, CharacterInfo characterInfo)
        {
            return new UserPresenter(new CharacterInfoPresenter(userInfo, playerLevel), new CharacterStatsPresenter(characterInfo));
        }
    }
}