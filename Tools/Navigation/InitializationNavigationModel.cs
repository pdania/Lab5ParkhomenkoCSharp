using System;
using MainWindow = Lab4ParkhomenkoCSharp2019.MainWindow;
using UserListControl = Lab4ParkhomenkoCSharp2019.Views.User.UserListControl;

namespace Lab4ParkhomenkoCSharp2019.Tools.Navigation
{
    internal class InitializationNavigationModel : BaseNavigationModel
    {
        public InitializationNavigationModel(IContentOwner contentOwner) : base(contentOwner)
        {
            
        }
   
        protected override void InitializeView(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Users:
                    ViewsDictionary.Add(viewType, new UserListControl());
                    break;
                case ViewType.Main:
                    ViewsDictionary.Add(viewType, new MainWindow());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }
    }
}
