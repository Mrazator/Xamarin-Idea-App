using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using IdeaApp;
using IdeaApp.Droid;

[assembly: ExportRenderer(typeof(FacebookLoginButton), typeof(FacebookLoginButtonRenderer))]
namespace IdeaApp.Droid
{
    public class FacebookLoginButtonRenderer : ViewRenderer
    {
        private readonly Context _ctx;
        bool _disposed;
        public FacebookLoginButtonRenderer(Context ctx) : base(ctx)
        {
            this._ctx = ctx;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            if (Control == null)
            {
                var fbLoginBtnView = e.NewElement as FacebookLoginButton;
                var fbLoginbBtnCtrl = new Xamarin.Facebook.Login.Widget.LoginButton(_ctx)
                {
                    LoginBehavior = LoginBehavior.NativeWithFallback
                };

                fbLoginbBtnCtrl.SetReadPermissions(fbLoginBtnView?.Permissions);
                fbLoginbBtnCtrl.RegisterCallback(MainActivity.CallbackManager, new MyFacebookCallback(this.Element as FacebookLoginButton));

                SetNativeControl(fbLoginbBtnCtrl);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !this._disposed)
            {
                if (this.Control != null)
                {
                    //(this.Control as Xamarin.Facebook.Login.Widget.LoginButton).UnregisterCallback(MainActivity.CallbackManager);
                    this.Control.Dispose();
                }
                this._disposed = true;
            }
            base.Dispose(disposing);
        }

        class MyFacebookCallback : Java.Lang.Object, IFacebookCallback
        {
            private readonly FacebookLoginButton _view;

            public MyFacebookCallback(FacebookLoginButton view)
            {
                this._view = view;
            }

            public void OnCancel() =>
                _view.OnCancel?.Execute(null);

            public void OnError(FacebookException fbException) =>
                _view.OnError?.Execute(fbException.Message);

            public void OnSuccess(Java.Lang.Object result) =>
                _view.OnSuccess?.Execute(((LoginResult)result).AccessToken.Token);

        }
    }
}