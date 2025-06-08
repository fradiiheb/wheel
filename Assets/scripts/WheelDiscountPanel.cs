using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Envast.Models;
using Envast.Views.PlayerViews;
using UnityEngine;
using UnityEngine.UI;

namespace Envast.Components
{
    public class WheelDiscountPanel : MonoBehaviour
    {
        [Header ("OpenClose :")]
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private float animationDuration;
        [SerializeField] private Button closeBtn;
        [SerializeField] private Button closeBlurBtn; 
        [SerializeField] private Button sectionButton;
        [Header ("Wheel Generations :")]
        [SerializeField] private Transform wheelPiecesParent ;
        [SerializeField] private GameObject wheelItemPrefabName;
        [Header ("Wheel Spin :")]
        [SerializeField] private Button spinButton;  
        [SerializeField] private AnimationCurve spinCurves;
        [SerializeField] private float spinTime;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float decelerationRate;
        [SerializeField] private Transform wheelItemHolder ;
        [Header ("Win Objects:")]
        [SerializeField] private Transform wheelObjects;
        [SerializeField] private GameObject winObjects;
        [SerializeField] private Text winValueText;
        [SerializeField] private Luncher Luncher;
        
        
        private List<WheelItem> _wheelItems;
        private WheelItem _winnerItem;

        private Vector3 _closePosition;
        private int _numberOfItems;
        private float _pieceAngle ;
        private float _halfPieceAngle ;
        private int _sectionSize = 170;
        private float _childToSpinInitialRotation ;
     
        public  void Awake()
        {
            closeBtn.onClick.AddListener(ClosePopUp);
            closeBlurBtn.onClick.AddListener(ClosePopUp);
            spinButton.onClick.AddListener(SpinTheWheel);
            _closePosition = rectTransform.localPosition;
            _childToSpinInitialRotation = wheelItemHolder.localEulerAngles.z;
            _wheelItems = new List<WheelItem>();
            wheelObjects.gameObject.SetActive(false);
            winObjects.gameObject.SetActive(false);
        }
        
        protected  void ReleaseReferences()
        {
            closeBtn = null;
            closeBlurBtn = null;
            rectTransform = null;
            wheelPiecesParent = null;
            _wheelItems = null;
            _winnerItem = null;
            spinButton = null;
            spinCurves = null;
            wheelItemHolder = null;
            wheelObjects = null;
            winObjects = null;
            winValueText = null;
            sectionButton = null;
        }

        public void OpenPopUp()
        {
            //if(IsSubscribed) return;
            closeBlurBtn.gameObject.SetActive(true); 
            wheelObjects.gameObject.SetActive(true);

        }
        private void ClosePopUp()
        {
          
                    closeBlurBtn.gameObject.SetActive(false); 
                    RemoveItems();
                    ResetRotation();
                    Luncher.FakeData();
        }
        
        public void SetWheelData(int inNumberOfItems)
        {
            _numberOfItems = inNumberOfItems;
            _pieceAngle = 360 / _numberOfItems;
            _halfPieceAngle = _pieceAngle / 2f;
            switch (_numberOfItems)
            {
                case 3 : _sectionSize = 980;break;
                case 4: _sectionSize = 560;break;
                case 5: _sectionSize = 500;break;
                case 6:_sectionSize = 325;break;
                case 7:_sectionSize = 290;break;
                case 8:_sectionSize = 235;break;
                case 9:_sectionSize = 200;break;
                case 10: _sectionSize = 180;break;
            }
           
        }
        
        public void AddChild(CouponModel inCoupon,int index,bool isWinner=false)
        {
            var wheelItem = Instantiate(wheelItemPrefabName,Vector2.zero, Quaternion.identity,wheelPiecesParent);
            if (wheelItem == null) throw new NullReferenceException("WheelItem is null");
            var item=wheelItem.GetComponent<WheelItem>();
            var isFirst = index % 2 == 0;
            item.Initialize(_sectionSize,isFirst);
            item.BindData(inCoupon);
            _wheelItems.Insert(0, item);
            if (isWinner) _winnerItem = item;
            wheelItem.GetComponent<RectTransform>().RotateAround (wheelPiecesParent.position, Vector3.back, _pieceAngle * index) ;
            wheelItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        
        private void SpinTheWheel()
        {
            Debug.Log("SpinTheWheel");
            StartCoroutine(AnimateSpin());
        }

        IEnumerator AnimateSpin()
        {
            var spinTimer = 0f;
            var startAngle = -(360 / _numberOfItems);
            var spinMagnitude = 360;
            var landingAngle = (spinMagnitude * spinTime) + _winnerItem.transform.localEulerAngles.z -
                               ((360 / _numberOfItems / 2) * 2);
            if (spinCurves == null) CreateSampleAnimation();
            var lastFrameTime = Time.time;
            while (spinTimer < spinTime - 0.1f)
            {
                float newRotation;
                newRotation = landingAngle * - spinCurves.Evaluate(spinTimer / spinTime);
                wheelItemHolder.localEulerAngles = new Vector3(0, 0, newRotation + startAngle);
                var currentRotationSpeed =
                    rotationSpeed * Mathf.Pow(1f - (spinTimer / spinTime), decelerationRate);
                var deltaTime = Time.time - lastFrameTime;
                lastFrameTime = Time.time;
                var rotationDelta = currentRotationSpeed * deltaTime;
                spinTimer += rotationDelta;
                yield return null; 
            }
            yield return new WaitForSeconds(1);
            AnimateReward(); //for Fake 
             //SendWinCoupon();
        }
        public void CreateSampleAnimation()
        {
            var sampleKeyframes = new Keyframe[2];
            sampleKeyframes[0] = new Keyframe(0, 0);
            sampleKeyframes[1] = new Keyframe(1, 1);
            var sampleCurve = new AnimationCurve(sampleKeyframes);
            spinCurves = sampleCurve;
        }
        
        private void SendWinCoupon()
        {
           
                AnimateReward();
            
        }
        
        private void AnimateReward()
        {
            /*wheelObjects.gameObject.SetActive(false);
            winObjects.gameObject.SetActive(true);
            winValueText.text =$"{_winnerItem.Data.value}%";*/
            wheelObjects.gameObject.SetActive(false);
            winObjects.gameObject.SetActive(true);
            if (_winnerItem.Data.image != null)
            {
                //winValueImage.sprite = _winnerItem.Data.image;
                winValueText.gameObject.SetActive(false);
            }
            else
            {
                winValueText.gameObject.SetActive(true);
               // winValueImage.sprite = couponSprite;
            }

            _winnerItem.Data.numbersOfItems--;
            winValueText.text =$"{_winnerItem.Data.value}%";
        }

        
        private void ResetRotation()
        {
            wheelObjects.gameObject.SetActive(false);
            winObjects.gameObject.SetActive(false);
            wheelItemHolder.localEulerAngles = new Vector3(0, 0, _childToSpinInitialRotation);
            _wheelItems.Clear();
        }
        
        private void RemoveItems()
        {
            for (int i = 0; i < _numberOfItems; i++)
            {
                var wheelItem = _wheelItems[i].transform;
                Destroy(wheelItem.gameObject);
            }
        }

    }
}
