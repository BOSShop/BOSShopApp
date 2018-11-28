namespace ModernBOSShopApp.Animation
{
    public static class PageTransitionAnimationHelper
    {
        public static PageTransitionAnimation GetOpposite(this PageTransitionAnimation animation)
        {
            switch (animation)
            {
                case PageTransitionAnimation.None:
                    return PageTransitionAnimation.None;
                case PageTransitionAnimation.SlideTop:
                    return PageTransitionAnimation.SlideBottom;
                case PageTransitionAnimation.SlideLeft:
                    return PageTransitionAnimation.SlideRight;
                case PageTransitionAnimation.SlideBottom:
                    return PageTransitionAnimation.SlideTop;
                case PageTransitionAnimation.SlideRight:
                    return PageTransitionAnimation.SlideLeft;
                default:
                    return PageTransitionAnimation.None;
            }
        }
    }
}
