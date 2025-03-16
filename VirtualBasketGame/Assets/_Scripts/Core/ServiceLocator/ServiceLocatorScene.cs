namespace BasketGame.Core
{
    public class ServiceLocatorScene : ServiceLocatorBase
    {
        #region Singleton definition

        private static ServiceLocatorScene _instance;
        public static ServiceLocatorScene Instance => _instance;

        #endregion

        protected override void Awake()
        {
            #region Singleton initialize

            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            #endregion
            base.Awake();
        }
    }
}