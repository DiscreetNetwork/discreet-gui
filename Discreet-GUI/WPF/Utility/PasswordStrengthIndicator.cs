using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Utility
{
    public class PasswordStrengthIndicator
    {
        public static PasswordStrength CalculatePasswordStrength(string password)
        {
            int strength = 0;
            if (password.Length >= 8) strength++;
            if (password.Any(c => char.IsLower(c)) && password.Any(c => char.IsUpper(c))) strength++;
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
