namespace Lab4ParkhomenkoCSharp2019.Tools.Navigation
{
    internal enum ViewType
    {
        Users,
        Main
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}
