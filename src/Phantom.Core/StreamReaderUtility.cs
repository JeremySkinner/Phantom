namespace Phantom.Core {
    using System;
    using System.IO;

    public static class StreamReaderUtility {
        public static string ReadAllAsString(this StreamReader stream) {
            try {
                    return stream.ReadToEnd();
            }
            catch (ObjectDisposedException)
            {

                return "";
            }
            
        }
    }
}