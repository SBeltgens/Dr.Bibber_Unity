using UnityEngine;
using UnityEngine.UI;

public class PassWordManager : MonoBehaviour
{
    [SerializeField] private InputField userNameInput;
    [SerializeField] private InputField passWordInput;
    [SerializeField] private Text errorMessage;
    public void CheckPassWord()
    {
        string password = passWordInput.text;
        bool hasDigit = false;
        bool hasSpecial = false;

        foreach (char c in password)
        {
            if (char.IsDigit(c)) hasDigit = true;
            if (!char.IsLetterOrDigit(c)) hasSpecial = true;
        }

        if (password.Length < 8)
        {
            errorMessage.text = "Password must be at least 8 characters long.";
        }
        else if (!hasDigit)
        {
            errorMessage.text = "Password must contain at least one digit.";
        }
        else if (!hasSpecial)
        {
            errorMessage.text = "Password must contain at least one special character.";
        }
        else
        {
            // Password is valid, proceed with registration or login
        }
    }
}
