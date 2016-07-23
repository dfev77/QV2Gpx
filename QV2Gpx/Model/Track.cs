
namespace QV2Gpx.Model
{
    internal class Track
    {
        public int Id;
        public string Name;

        public string GetFileName()
        {
            string returnValue = Name;

            if (returnValue.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
            {
                const char replacementChar = '_';

                foreach (char invalidChar in System.IO.Path.GetInvalidFileNameChars())
                {
                    returnValue = returnValue.Replace(invalidChar, replacementChar);
                }
            }

            return returnValue + ".gpx";
        }
    }
}
