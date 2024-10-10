namespace CodeMonkeys.CMS.Public.Services
{
    public class MenuConfigurationService
    {
        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                foreach (Action action in actions)
                {
                    action();
                }

            }
        }
        public HashSet<Action> actions { get; set; } = new();
    }
}
