// Any comments, input: @KevinDockx
// Any issues, requests: https://github.com/KevinDockx/JsonPatch
//
// Enjoy :-)

namespace Marvin.JsonPatch.Helpers
{
    internal class ConversionResult
    {
        public bool CanBeConverted { get; private set; }
        public object ConvertedInstance { get; private set; }


        public ConversionResult(bool canBeConverted, object convertedInstance)
        {
            CanBeConverted = canBeConverted;
            ConvertedInstance = convertedInstance;

        }
    }
}
