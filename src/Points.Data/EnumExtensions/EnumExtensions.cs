using System.Text;

namespace Points.Data.EnumExtensions
{
    public static class EnumExtensions
    {
        public static string Spacify(this object obj)
        {
            var input = obj.ToString();
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }
            StringBuilder output = new StringBuilder(input.Substring(0, 1));
            for (int i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                {
                    output.Append(' ');
                }
                output.Append(input[i]);
            }
            return output.ToString();
        }
    }
}
