using System;
using System.IO;
using System.Runtime.CompilerServices;
using Android.App;
using Android.Content;
using Android.Support.V4.Content;
using Android.Widget;
using ListView_CollectionView_TableView.Droid;
using ListView_CollectionView_TableView.Services;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(OpenPDF))]
namespace ListView_CollectionView_TableView.Droid
{
    public class OpenPDF: IOpenPDF
    {
        bool IOpenPDF.OpenPDF(string pathPDFFile)
        {
            //Open it up
            //Android.Net.Uri pdfPath = Android.Net.Uri.FromFile(new Java.IO.File(filePath));
            Context context = Forms.Context;
            Android.Net.Uri pdfPath = FileProvider.GetUriForFile(context, "com.mydomain.fileprovider", new Java.IO.File(pathPDFFile));

            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(pdfPath, "application/pdf");
            intent.SetFlags(ActivityFlags.GrantReadUriPermission);
            intent.SetFlags(ActivityFlags.NewTask);
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset);
            context.StartActivity(intent);
            //var bytes = File.ReadAllBytes(pathPDFFile);

            ////Copy the private file's data to the EXTERNAL PUBLIC location
            //string externalStorageState = global::Android.OS.Environment.ExternalStorageState;
            ////var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/" + global::Android.OS.Environment.DirectoryDownloads + "/" + fileName;
            //File.WriteAllBytes(pathPDFFile, bytes);

            //Java.IO.File file = new Java.IO.File(pathPDFFile);
            //file.SetReadable(true);

            //string application = "";
            //string extension = Path.GetExtension(pathPDFFile);

            //// get mimeTye
            //switch (extension.ToLower())
            //{
            //    case ".txt":
            //        application = "text/plain";
            //        break;
            //    case ".doc":
            //    case ".docx":
            //        application = "application/msword";
            //        break;
            //    case ".pdf":
            //        application = "application/pdf";
            //        break;
            //    case ".xls":
            //    case ".xlsx":
            //        application = "application/vnd.ms-excel";
            //        break;
            //    case ".jpg":
            //    case ".jpeg":
            //    case ".png":
            //        application = "image/jpeg";
            //        break;
            //    default:
            //        application = "*/*";
            //        break;
            //}

            ////Android.Net.Uri uri = Android.Net.Uri.Parse("file://" + filePath);
            //Android.Net.Uri uri = Android.Net.Uri.FromFile(file);
            //Intent intent = new Intent(Intent.ActionView);
            //intent.SetDataAndType(uri, application);
            //intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

            //Forms.Context.StartActivity(intent);

            return true;
        }
    }
}