namespace LM.Storage.Api.Settings
{
    public class StorageSettings
    {
        public StorageAwsSettings StorageAws { get; set; }

        public class StorageAwsSettings
        {
            public string BucketName { get; set; }

            public string BaseUrl { get; set; }
        }
    }
}