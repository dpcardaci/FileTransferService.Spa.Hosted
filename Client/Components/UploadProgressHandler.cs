﻿namespace FileTransferService.Spa.Hosted.Client.Components
{
    public class UploadProgressHandler : IProgress<long>
    {
        public long FileSize { get; set; }

        public void Report(long value)
        {
            decimal decValue = value;
            decimal decFileSize = FileSize;
            decimal rawCalculatedValue = decValue / decFileSize * 100;
            int calculatedPercentage = (int)Math.Floor(rawCalculatedValue);

            ProgressUpdatedEventArgs progressUpdatedEventArgs = new ProgressUpdatedEventArgs
            {
                BytesTransferred = value,
                PercentageAsNumber = calculatedPercentage,
                PercentageAsFormattedString = $"{calculatedPercentage}%"
            };
            OnProgressUpdated(progressUpdatedEventArgs);
        }

        protected virtual void OnProgressUpdated(ProgressUpdatedEventArgs e)
        {
            if (ProgressUpdated != null)
            {
                ProgressUpdatedEventHandler handler = ProgressUpdated;
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }

        public event ProgressUpdatedEventHandler? ProgressUpdated;

    }

    public class ProgressUpdatedEventArgs
    {
        public long BytesTransferred { get; set; } = 0;
        public int PercentageAsNumber { get; set; } = 0;
        public string PercentageAsFormattedString { get; set; } = "0%";
    }

    public delegate void ProgressUpdatedEventHandler(Object sender, ProgressUpdatedEventArgs e);
}
