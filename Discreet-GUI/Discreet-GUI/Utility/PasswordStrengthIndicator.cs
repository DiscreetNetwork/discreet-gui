using System.Linq;

namespace Discreet_GUI.Utility
{
    public class PasswordStrengthIndicator
    {
        public static PasswordStrength CalculatePasswordStrength(string password)
        {
            int strength = 0;
            if (password.Length >= 1) strength++;
            
            if (password.Any(c => char.IsLower(c)) && password.Any(c => char.IsUpper(c)) && password.Length >= 8)
            {
                strength++;
            }
            else return (PasswordStrength)strength;

            if (password.Any(c => char.IsDigit(c))) strength++;
            
            if (password.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) != -1) strength++;

            return (PasswordStrength)strength;
        }
    }

    public enum PasswordStrength
    {
        VeryWeak,
        Weak,
        Medium,
        Strong,
        ExtremelyStrong
    }
}
