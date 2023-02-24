using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleAnimController : MonoBehaviour{
    public bool animate=true;
    [SerializeField] List<SimpleAnim> animVals;
	public float animSpeed=0.05f;//Leave at 0 to make each frame speed controlable
	public bool addInitialImageToList=true;
    Coroutine _anim;int iAnim=0;Sprite animSpr;
    void Start(){
        if(addInitialImageToList){
            Sprite _spr=null;
            if(GetComponent<Sprite>()!=null){_spr=animSpr;}
            else if(GetComponent<Image>()!=null){_spr=animSpr;}
            else if(transform.GetChild(0).GetComponent<SpriteRenderer>()!=null){_spr=animSpr;}
            else if(transform.GetChild(0).GetComponent<Image>()!=null){_spr=animSpr;}

            if(_spr!=null)AddSimpleAnim(_spr);
        }

    }
    void Update(){
        if(animate&&animVals.Count>0){
            if(_anim==null){_anim=StartCoroutine(Animate());}

            if(animSpr!=null){
                if(GetComponent<Sprite>()!=null){GetComponent<SpriteRenderer>().sprite=animSpr;}
                else if(GetComponent<Image>()!=null){GetComponent<Image>().sprite=animSpr;}
                else if(transform.GetChild(0).GetComponent<SpriteRenderer>()!=null){transform.GetChild(0).GetComponent<SpriteRenderer>().sprite=animSpr;}
                else if(transform.GetChild(0).GetComponent<Image>()!=null){transform.GetChild(0).GetComponent<Image>().sprite=animSpr;}
            }
        }
    }
    IEnumerator Animate(){Sprite spr;
        if(animSpeed>0){yield return new WaitForSeconds(animSpeed);}
        else{yield return new WaitForSeconds(animVals[iAnim].delay);}
        spr=animVals[iAnim].spr;
        if(iAnim==animVals.Count-1)iAnim=0;
        if(iAnim<animVals.Count)iAnim++;
        animSpr=spr;
        if(animate)_anim=StartCoroutine(Animate());
        else{if(_anim!=null)StopCoroutine(_anim);_anim=null;iAnim=0;}
    }

    public void AddSimpleAnim(Sprite spr,float delay=0.02f){
        animVals.Add(new SimpleAnim{spr=spr, delay=delay});
    }
}
