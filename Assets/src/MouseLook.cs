using System;
using UnityEngine;

namespace game
{
    public class MouseLook
    {
        float XSensitivity = 1.1f;
        float YSensitivity = 1.1f;
        float MinimumX = -45F;
        float MaximumX = 45F;
        float smoothTime = 11f;

        Quaternion m_CharacterTargetRot;
        Quaternion m_CameraTargetRot;
        bool m_cursorIsLocked = true;

        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }


        public void LookRotation(Transform character, Transform camera)
        {
            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

            if(m_cursorIsLocked)
            {
                UpdateRotation(ref character, m_CharacterTargetRot);
                UpdateRotation(ref camera, m_CameraTargetRot);
            }

            UpdateCursorLock();
        }

        void UpdateRotation(ref Transform source, Quaternion target)
        {
            source.localRotation = Quaternion.Slerp(
                source.localRotation, 
                target,
                smoothTime * Time.deltaTime
            );
        }

        void UpdateCursorLock()
        {
            m_cursorIsLocked = !Input.GetKey(KeyCode.LeftAlt);

            Cursor.lockState = m_cursorIsLocked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !m_cursorIsLocked;
        }

        void FixY()
        {
            
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
            angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }
    }
}
