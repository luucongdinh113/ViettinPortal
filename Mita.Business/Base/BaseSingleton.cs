namespace Mita.Business.Base
{
    public abstract class BaseSingleton<T> where T : BaseSingleton<T>, new()
    {
        private static T _instance = null;

        public static T GetInstance()
        {
            if (_instance == null)
            {
                _instance = new T();
            }

            return _instance;
        }
    }
}
