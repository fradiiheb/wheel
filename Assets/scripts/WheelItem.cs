using Envast.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Envast.Views.PlayerViews
{
    public class WheelItem : MonoBehaviour
    {
        [SerializeField] private Text itemText;
        
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image itemImage;
        [SerializeField] private Color sectionFirstColor;
        [SerializeField] private Color sectionSecondColor;
        
        [SerializeField] private Color textFirstColor;
        [SerializeField] private Color textSecondColor;
        private RectTransform _rect;
        private bool _isCollected;
        private bool _isInitialized;
        private CouponModel _couponModel;
        public CouponModel Data => _couponModel;
        public RectTransform RectTransform
        {
            get
            {
                if (_rect == null) _rect = GetComponent<RectTransform>();
                return _rect;
            }
        }
        protected  void ReleaseReferences()
        {
            itemText = null;
            _couponModel = null;
            backgroundImage = null;
            itemImage = null;
        }
        public void Initialize(int inSectionWidth,bool isFirstColor)
        {
            if (_isInitialized) return;
            _isInitialized = true;
            backgroundImage.rectTransform.sizeDelta = new Vector2(inSectionWidth, 300);
            backgroundImage.color = isFirstColor ? sectionFirstColor : sectionSecondColor;
            itemText.color = isFirstColor ? textFirstColor : textSecondColor;
            // RectTransform.localScale = Vector3.one;
        }
        public void BindData(CouponModel inCouponModel)
        {
            itemImage.gameObject.SetActive(true);
            itemText.gameObject.SetActive(true);
            _couponModel = inCouponModel;
            itemText.text =$"{_couponModel.value}\n%";
            itemImage.sprite = inCouponModel.image;
            if(itemImage.sprite == null) itemImage.gameObject.SetActive(false);
            else itemText.gameObject.SetActive(false);
        }
    }
}
