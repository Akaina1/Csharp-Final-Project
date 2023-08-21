namespace FinalProject
{
    public static class GlobalState
    {
        private static User? _currentUser;
        private static int _resetDay = 1;
        
        public static User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        public static int ResetDay
        {
            get { return _resetDay; }
            set { _resetDay = value; }
        }

        public static void Logout()
        {
            _currentUser = null;
        }
    }
}
