using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Util;
using Google.Android.Vending.Expansion.ZipFile;

namespace IRMGARD
{
    public abstract class AssetHelper
    {
        const string TAG = "AssetHelper";
        static AssetHelper instance;

        protected readonly Context ctx;
        private readonly string tmpDir;

        protected AssetHelper(Context ctx)
        {
            this.ctx = ctx;
            tmpDir = Path.Combine(ctx.CacheDir.Path, "tpdIgTmp");
            if (Directory.Exists(tmpDir))
            {
                foreach (var s in Directory.GetFiles(tmpDir))
                {
                    if (Env.Debug) Log.Debug(TAG, s);
                }
                Directory.Delete(tmpDir, true);
            }
            Directory.CreateDirectory(tmpDir);
        }

        public static AssetHelper Instance
        {
            get { return instance; }
        }

        public static void CreateInstance(Context ctx, int mainVersion, int patchVersion)
        {
            if (Env.UseOBB)
            {
                instance = new OBBAssetHelper(ctx, mainVersion, patchVersion);
            }
            else
            {
                instance = new FSAssetHelper(ctx);
            }
        }

        protected string GetTempFilePath(string path)
        {
            return Path.Combine(tmpDir, path.Replace('/', '_'));
        }

        public string GetExtractedTempFilePath(string path)
        {
            string tempFilePath = GetTempFilePath(path);
            using (var inStream = AssetHelper.Instance.Open(path))
            {
                using (var tempOutStream = File.Create(tempFilePath))
                {
                    inStream.CopyTo(tempOutStream);
                }
            }

            return tempFilePath;
        }

        public void DeleteExtractedTempFile(string path)
        {
            File.Delete(GetTempFilePath(path));
        }

        public bool IsValid()
        {
            return List("Videos").Contains("IRMGARD_Intro.mp4") && List("Fonts").Contains("GlacialIndifference-Bold.otf");
        }

        public abstract Stream Open(string path);
        public abstract List<string> List(string path);
        public abstract AssetFileDescriptor OpenFd(string path);

        class OBBAssetHelper : AssetHelper
        {
            readonly ZipResourceFile zipResourceFile;
            readonly List<string> filePaths;

            protected internal OBBAssetHelper(Context ctx, int mainVersion, int patchVersion) : base(ctx)
            {
                zipResourceFile = APKExpansionSupport.GetAPKExpansionZipFile(ctx, mainVersion, patchVersion);

                filePaths = new List<string>();
                foreach (var zipFileEntry in zipResourceFile.GetAllEntries())
                {
                    filePaths.Add(zipFileEntry.MFileName);
                }
            }

            public override Stream Open(string path)
            {
                return zipResourceFile.GetInputStream(path);
            }

            public override List<string> List(string path)
            {
                return filePaths.FindAll(fp => fp.StartsWith(path, StringComparison.InvariantCultureIgnoreCase)).Select(fullPath => Path.GetFileName(fullPath)).ToList();
            }

            public override AssetFileDescriptor OpenFd(string path)
            {
                return zipResourceFile.GetAssetFileDescriptor(path);
            }
        }

        class FSAssetHelper : AssetHelper
        {
            protected internal FSAssetHelper(Context ctx) : base(ctx)
            {
            }

            public override Stream Open(string path)
            {
                return ctx.Assets.Open(path);
            }

            public override List<string> List(string path)
            {
                return ctx.Assets.List(path).ToList();
            }

            public override AssetFileDescriptor OpenFd(string path)
            {
                return ctx.Assets.OpenFd(path);
            }
        }
    }
}