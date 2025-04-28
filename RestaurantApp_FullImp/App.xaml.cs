namespace RestaurantApp_FullImp
{
    using RestaurantApp_FullImp.Project.Controllers;

    public partial class App : Application
    {
        public static MenuController Menu;
        public static CartController Cart;
        
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new Project.Views.MainMenuPage());
        }

        static App()
        {
            Menu = new MenuController();
            Cart = new CartController();
        }
    }
}