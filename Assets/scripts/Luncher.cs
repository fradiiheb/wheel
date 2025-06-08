using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Envast.Components;
using Envast.Models;
using Random = System.Random;
using RandomUnity = UnityEngine.Random;

public class Luncher : MonoBehaviour
{
    [SerializeField] private List<CouponModel> couponList;
    [SerializeField] private WheelDiscountPanel WheelDiscountPanel;
    private CouponModel couponw;

    // Start is called before the first frame update
    private void Start()
    {
        FakeData();
    }
    private void Shuffle<T>(List<T> list)  
        {  
            Random rng = new Random();
            if (list == null || list.Count == 0) return;
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                (list[k], list[n]) = (list[n], list[k]);
            }  
        } 
        public void FakeData()
        {
            
            int i;
            /*for ( i = 0; i < 9; i++)
            {
                string id = i.ToString();
                string value = (i*10).ToString(); // Generates a random value between 0 and 100 as a string
                CouponModel coupon = new CouponModel { id = id, value = value };
                couponList.Add(coupon);
            }
            string idw = i.ToString();
            string valuew = 55+"";
            CouponModel couponw = new CouponModel { id = idw, value = valuew };
            couponList.Add(couponw);*/
            
            
            if (couponList.Count > 10 || couponList.Count <= 2)
            {
               Debug.LogError("couppon list size is too big or too small");
                return;
            }
            
            /*Shuffle(couponList);
            
            AssignItem(couponList, couponw);*/
            SelectWonCoupon();
        }
        private void SelectWonCoupon()
        {
            List<CouponModel> filteredItems = couponList.Where(item => item.numbersOfItems > 0).ToList();
            if (filteredItems.Count <= 0)
            {
               // noCouponLeftCallBack?.Invoke();
                return;
            }
            couponw = filteredItems[RandomUnity.Range(0, filteredItems.Count)];
            Shuffle(couponList);
            AssignItem(couponList, couponw);
        }
        private void AssignItem(List<CouponModel> inCoupon,CouponModel inWinner)
        {
            Debug.Log($"Winner Value{inWinner.value}");
                WheelDiscountPanel.SetWheelData(inCoupon.Count);

                for (var i = 0; i < inCoupon.Count; i++)
                {
                    var newCoupon = inCoupon[i];
                    if(newCoupon.id == inWinner.id)
                        WheelDiscountPanel.AddChild(newCoupon,i,true);
                    else                   
                        WheelDiscountPanel.AddChild(newCoupon,i);
                }
                WheelDiscountPanel.OpenPopUp();
        }

}
