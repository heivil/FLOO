using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialMediaLinks : MonoBehaviour
{
   public void LinkToFacebook()
    {
        Application.OpenURL("https://www.facebook.com/profile.php?id=100073088419031");
    }

    public void LinkToInstagram()
    {
        Application.OpenURL("https://www.instagram.com/themaruuna");
    }
}
