using System.Collections.Immutable;

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
                DataUpdated();

            }
        }

        // We absolutely do not want to provide a reference to the list that could be used to change it without triggering the notifications.
        private ImmutableList<NavMenuEntry> _entries = new List<NavMenuEntry>().ToImmutableList();
        public ImmutableList<NavMenuEntry> NavMenuEntries { get { return _entries; } }

        // Intent at time of coding is that the empty list above has priority 0, the default list from the NavMenu has priority 1,
        // and lists provided by page components have priority 2.
        private int currentPriority = 0;

        // Full replacement only.
        public void SetEntries(ImmutableList<NavMenuEntry> entries, int priority)
        {
            if (priority > currentPriority)
            {
                _entries = entries;
                currentPriority = priority;
                DataUpdated();
            }
        }

        public event Action DataUpdated = delegate { };
    }
}
