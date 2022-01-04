using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.IO;
using AndroidX.Core.Content;

namespace XamarinCamera
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        ImageView ivFotoContainer;
        string diretorio;        
        Java.IO.File arquivo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);            
            SetContentView(Resource.Layout.activity_main);

            CreateDirectoryForPictures();

            var btnTirarFoto = FindViewById<Button>(Resource.Id.btnTirarFoto);
            ivFotoContainer = FindViewById<ImageView>(Resource.Id.photoContainer);
            btnTirarFoto.Click += TirarFoto;            
        }

        private void TirarFoto(object sender, EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            try
            {                
                arquivo = new Java.IO.File((diretorio + String.Format("photo{0}.jpg", Guid.NewGuid())));
                intent.PutExtra(MediaStore.ExtraOutput,
                                FileProvider.GetUriForFile(this, 
                                                           PackageName + ".fileprovider",
                                                           arquivo));
            }
            catch(Exception ex)
            {
                Log.Debug("ERRO", ex.Message);
            }
                        
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
                        
            using (Bitmap bitmap = BitmapFactory.DecodeFile(arquivo.Path))
            {                
                ivFotoContainer.SetImageBitmap(bitmap);                
            }
        }

        private void CreateDirectoryForPictures()
        {            
            if (!Directory.Exists(GetExternalFilesDir(Android.OS.Environment.DirectoryPictures) + "/AppCamDemo/"))
            {
                Directory.CreateDirectory(GetExternalFilesDir(Android.OS.Environment.DirectoryPictures) + "/AppCamDemo/");                
            }
            
            diretorio = GetExternalFilesDir(Android.OS.Environment.DirectoryPictures) + "/AppCamDemo/";            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}