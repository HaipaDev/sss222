using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screenflash : MonoBehaviour{
    [SerializeField] Color damageFlashColor;
    [SerializeField] float damageFlashSpeed;
    [SerializeField] Color healFlashColor;
    [SerializeField] float healedFlashSpeed;
    [SerializeField] Color shadowFlashColor;
    [SerializeField] float shadowFlashSpeed;
    [SerializeField] Color flameFlashColor;
    [SerializeField] float flameFlashSpeed;
    [SerializeField] Color electrcFlashColor;
    [SerializeField] float electrcFlashSpeed;
    Player player;
    Image image;
    // Start is called before the first frame update
    void Start(){
        player=FindObjectOfType<Player>().GetComponent<Player>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update(){
        if(player.damaged==true){image.color = damageFlashColor;player.damaged = false;}
        else { image.color = Color.Lerp(image.color, Color.clear, damageFlashSpeed * Time.deltaTime); }
        if(player.healed==true){image.color = healFlashColor;player.healed = false;}
        else { image.color = Color.Lerp(image.color, Color.clear, healedFlashSpeed * Time.deltaTime); }
        if (player.shadowed==true){image.color = shadowFlashColor;player.shadowed = false;}
        else { image.color = Color.Lerp(image.color, Color.clear, shadowFlashSpeed * Time.deltaTime); }
        if (player.flamed==true){image.color = flameFlashColor;player.flamed = false;}
        else { image.color = Color.Lerp(image.color, Color.clear, flameFlashSpeed * Time.deltaTime); }
        if (player.electricified==true){image.color = electrcFlashColor;player.electricified = false;}
        else { image.color = Color.Lerp(image.color, Color.clear, electrcFlashSpeed * Time.deltaTime); }
    }
}
