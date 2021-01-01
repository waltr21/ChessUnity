using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UISlider : MonoBehaviour
    {
        public Vector3 NormalPos, OffScreenPos, DesiredPos;
        private RectTransform TransformObject;
        public float SlideSpeed = 5.0f;

        public void SetTransformObject(RectTransform t)
        {
            TransformObject = t;
            Vector3 temp = this.TransformObject.localPosition;
            SetPositions(temp, new Vector3(temp.x, 900f, temp.z));
            DesiredPos = NormalPos;
            return;
        }

        public virtual void Update()
        {
            if (TransformObject != null)
                this.TransformObject.localPosition = Vector3.Lerp(this.TransformObject.localPosition, DesiredPos, SlideSpeed * Time.deltaTime);
        }

        public void ShowCanvas(bool b)
        {
            if (b)
            {
                DesiredPos = NormalPos;
                return;
            }
            DesiredPos = OffScreenPos;
        }

        

        public void SetPositions(Vector3 normal, Vector3 offScreen)
        {
            if (normal != null)
                NormalPos = normal;
            if (offScreen != null)
                OffScreenPos = offScreen;
        }

        public void SetOffScreen(Vector3 v)
        {
            OffScreenPos = v;
        }
    }
}
