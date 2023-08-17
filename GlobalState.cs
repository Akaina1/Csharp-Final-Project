namespace FinalProject
{
    public static class GlobalState
    {
        private static User _currentUser;
        
        public static User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }
    }
}
