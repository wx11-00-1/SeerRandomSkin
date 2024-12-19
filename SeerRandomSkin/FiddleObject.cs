using System.Text.RegularExpressions;

namespace SeerRandomSkin
{
    public sealed class FiddleObject
    {
        public string From { get; set; }
        public Regex FromReg { get; set; }
        public string To { get; set; }
        public string Description { get; set; }
        public bool IsUrl { get; set; }
    }
}
