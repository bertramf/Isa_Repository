using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCourage : MonoBehaviour {

    public Image[] courageFill;
    public Image[] courageFillWhite;
    public Color colorWhite;
    public Color colorBlue;
    public float maxCourage = 5f;
    public float courage = 3f;

    private void Start(){
        CalculateUI(courage);
    }

    public void CourageUp(float courageBoost) {
        if(courage < maxCourage){
            float oldCourage = courage;
            if (courage + courageBoost > maxCourage){
                courage = maxCourage;
            }
            else{
                courage = courage + courageBoost;
            }
            CalculateUI(oldCourage);
        }
    }

    public void CourageDown(float courageCost){
        if (courage > 0){
            float oldCourage = courage;
            if (courage - courageCost < 0){
                courage = 0;
                //Player goes dead
            }
            else{
                courage = courage - courageCost;
            }
            CalculateUI(oldCourage);
        }
    }

    private void CalculateUI(float previousCourage){
        if (previousCourage > courage){
            //Oude courage even leeg maken
            for (int i = 0; i < (int)maxCourage; i++){
                courageFill[i].fillAmount = 0;
            }
            //CourageFillWhite wit maken
            for (int i = 0; i <= (int)Mathf.Floor(previousCourage); i++){
                float fillAmountWhite = 0f;
                if (i == (int)Mathf.Floor(previousCourage)){
                    fillAmountWhite = previousCourage - i;
                }
                else{
                    fillAmountWhite = 1;
                }
                if (i < maxCourage){
                    courageFillWhite[i].fillAmount = fillAmountWhite;
                }
            }
            StartCoroutine(CourageDownCalculations(previousCourage));
        }

        //Nieuwe courage berekenen
        for (int i = 0; i <= (int)Mathf.Floor(courage); i++){
            float fillAmount = 0f;
            if (i == (int)Mathf.Floor(courage)){
                fillAmount = courage - i;
            }
            else{
                fillAmount = 1;
            }
            StartCoroutine(FillUI(i, fillAmount, previousCourage));
        }
    }

    private IEnumerator CourageDownCalculations(float oldCourage){
        yield return new WaitForSeconds(0.2f);
        //CourageFillWhite leeg maken
        for (int i = 0; i < (int)maxCourage; i++){
            courageFillWhite[i].fillAmount = 0;
        }
    }

    private IEnumerator FillUI(int i, float fillAmount, float oldCourage){
        //Courage going down
        if (oldCourage > courage){
            //Normale courage meteen goed setten
            courageFill[i].fillAmount = fillAmount;
        }
        //Courage constant
        else if(oldCourage == courage){
            if (i < maxCourage){
                courageFill[i].fillAmount = fillAmount;
            }
        }
        //Courage going up
        else{
            if (i < maxCourage){
                courageFill[i].color = colorWhite;
                courageFill[i].fillAmount = fillAmount;
                yield return new WaitForSeconds(0.2f);
                courageFill[i].color = colorBlue;
            }
        }
        
    }

}
