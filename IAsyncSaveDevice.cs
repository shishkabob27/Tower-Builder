using System;

namespace EasyStorage
{
    // Token: 0x02000003 RID: 3
    public interface IAsyncSaveDevice
    {
        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000008 RID: 8
        bool IsBusy { get; }

        bool IsReady { get; }

        // Token: 0x06000013 RID: 19
        void SaveAsync(string containerName, string fileName);

        // Token: 0x06000014 RID: 20
        void SaveAsync(string containerName, string fileName, object userState);

        // Token: 0x06000015 RID: 21
        void LoadAsync(string containerName, string fileName);

        // Token: 0x06000016 RID: 22
        void LoadAsync(string containerName, string fileName, object userState);

        // Token: 0x06000017 RID: 23
        void DeleteAsync(string containerName, string fileName);

        // Token: 0x06000018 RID: 24
        void DeleteAsync(string containerName, string fileName, object userState);

        // Token: 0x06000019 RID: 25
        void FileExistsAsync(string containerName, string fileName);

        // Token: 0x0600001A RID: 26
        void FileExistsAsync(string containerName, string fileName, object userState);

        // Token: 0x0600001B RID: 27
        void GetFilesAsync(string containerName);

        // Token: 0x0600001C RID: 28
        void GetFilesAsync(string containerName, object userState);

        // Token: 0x0600001D RID: 29
        void GetFilesAsync(string containerName, string pattern);

        // Token: 0x0600001E RID: 30
        void GetFilesAsync(string containerName, string pattern, object userState);
    }
}
