using ModernBOSShopApp.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ModernBOSShopApp.Pages
{
    public class BasePage : Page
    {
        public PageTransitionAnimation animation = PageTransitionAnimation.SlideRight;
        public float animationDuration = 0.5f;

        public string PageName
        {
            get
            {
                return GetType().Name;
            }
        }

        public async void StartAnimationAsync(bool inwards)
        {
            if (inwards)
                await StartInAnimation(animation);
            else
                await StartOutAnimation(animation);
        }

        public async void StartAnimationAsync(bool inwards, PageTransitionAnimation transitionAnimation)
        {
            if (inwards)
                await StartInAnimation(animation);
            else
                await StartOutAnimation(animation);
        }


        public async Task StartAnimation(bool inwards)
        {
            if (inwards)
                await StartInAnimation(animation);
            else
                await StartOutAnimation(animation);
        }

        public async Task StartAnimation(bool inwards, PageTransitionAnimation transitionAnimation)
        {
            if (inwards)
                await StartInAnimation(animation);
            else
                await StartOutAnimation(animation);
        }
        private async Task StartInAnimation(PageTransitionAnimation transitionAnimation)
        {
            if (transitionAnimation == PageTransitionAnimation.None)
                return;

            Storyboard sb = new Storyboard();

            Timeline animation = null;
            DoubleAnimation fadeAnimation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(animationDuration)),
                From = 0,
                To = 1,
                DecelerationRatio = 0.9f
            };

            switch (transitionAnimation)
            {
                case PageTransitionAnimation.SlideTop:
                    animation = new ThicknessAnimation
                    {
                        Duration = new Duration(TimeSpan.FromSeconds(animationDuration)),
                        From = new Thickness(0, -MainWindow.Instance.Height, 0, MainWindow.Instance.Height),
                        To = new Thickness(0),
                        DecelerationRatio = 0.9f
                    };
                    break;
                case PageTransitionAnimation.SlideLeft:
                    animation = new ThicknessAnimation
                    {
                        Duration = new Duration(TimeSpan.FromSeconds(animationDuration)),
                        From = new Thickness(-MainWindow.Instance.Width, 0, MainWindow.Instance.Width, 0),
                        To = new Thickness(0),
                        DecelerationRatio = 0.9f
                    };
                    break;
                case PageTransitionAnimation.SlideBottom:
                    animation = new ThicknessAnimation
                    {
                        Duration = new Duration(TimeSpan.FromSeconds(animationDuration)),
                        From = new Thickness(0, MainWindow.Instance.Height, 0, -MainWindow.Instance.Height),
                        To = new Thickness(0),
                        DecelerationRatio = 0.9f
                    };
                    break;
                case PageTransitionAnimation.SlideRight:
                    animation = new ThicknessAnimation
                    {
                        Duration = new Duration(TimeSpan.FromSeconds(animationDuration)),
                        From = new Thickness(MainWindow.Instance.Width, 0, -MainWindow.Instance.Width, 0),
                        To = new Thickness(0),
                        DecelerationRatio = 0.9f
                    };
                    break;
            }

            if (animation == null)
                return;

            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
            Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath("Opacity"));
            sb.Children.Add(animation);
            sb.Children.Add(fadeAnimation);

            sb.Begin(this);

            await Task.Delay((int)(animationDuration * 1000));
        }

        private async Task StartOutAnimation(PageTransitionAnimation transitionAnimation)
        {
            if (transitionAnimation == PageTransitionAnimation.None)
                return;

            Storyboard sb = new Storyboard();

            Timeline animation = null;
            DoubleAnimation fadeAnimation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(animationDuration)),
                From = 1,
                To = 0,
                DecelerationRatio = 0.9f
            };

            switch (transitionAnimation)
            {
                case PageTransitionAnimation.SlideTop:
                    animation = new ThicknessAnimation
                    {
                        Duration = new Duration(TimeSpan.FromSeconds(animationDuration)),
                        To = new Thickness(0, -MainWindow.Instance.Height, 0, MainWindow.Instance.Height),
                        From = new Thickness(0),
                        DecelerationRatio = 0.9f
                    };
                    break;
                case PageTransitionAnimation.SlideLeft:
                    animation = new ThicknessAnimation
                    {
                        Duration = new Duration(TimeSpan.FromSeconds(animationDuration)),
                        To = new Thickness(MainWindow.Instance.Width, 0, -MainWindow.Instance.Width, 0),
                        From = new Thickness(0),
                        DecelerationRatio = 0.9f
                    };
                    break;
                case PageTransitionAnimation.SlideBottom:
                    animation = new ThicknessAnimation
                    {
                        Duration = new Duration(TimeSpan.FromSeconds(animationDuration)),
                        To = new Thickness(0, MainWindow.Instance.Height, 0, -MainWindow.Instance.Height),
                        From = new Thickness(0),
                        DecelerationRatio = 0.9f
                    };
                    break;
                case PageTransitionAnimation.SlideRight:
                    animation = new ThicknessAnimation
                    {
                        Duration = new Duration(TimeSpan.FromSeconds(animationDuration)),
                        To = new Thickness(-MainWindow.Instance.Width, 0, MainWindow.Instance.Width, 0),
                        From = new Thickness(0),
                        DecelerationRatio = 0.9f
                    };
                    break;
            }

            if (animation == null)
                return;

            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
            Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath("Opacity"));
            sb.Children.Add(animation);
            sb.Children.Add(fadeAnimation);

            sb.Begin(this);

            await Task.Delay((int)(animationDuration * 1000));
        }

        public virtual void OnLoad()
        {

        }

        public virtual void OnUnload()
        {

        }
    }
}