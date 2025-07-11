using UnityEngine;
using UnityEditor;

namespace BKPureNature
{
    [ExecuteInEditMode]
    public class GodraysController : MonoBehaviour
    {
        public Light directionalLight;
        public bool affectAllChildren = false;
        [SerializeField] private ParticleSystem[] particleSystems;

        private Quaternion lastLightRotation;

        void Start()
        {
            if (directionalLight != null)
            {
                lastLightRotation = directionalLight.transform.rotation;
            }

            if (affectAllChildren)
            {
                FindChildParticles();
            }
        }

        void Update()
        {
            if (directionalLight != null && directionalLight.transform.rotation != lastLightRotation)
            {
                if (affectAllChildren)
                {
                    FindChildParticles();
                }
                UpdateParticles();
                lastLightRotation = directionalLight.transform.rotation;
            }
        }

        void FindChildParticles()
        {
            particleSystems = GetComponentsInChildren<ParticleSystem>();
        }

        void UpdateParticles()
        {
            foreach (var ps in particleSystems)
            {
                if (ps != null)
                {
                    UpdateRotation(ps);
                    UpdateAlpha(ps);
                }
            }
        }

        void UpdateRotation(ParticleSystem ps)
        {
            Quaternion lightRotation = directionalLight.transform.rotation;
            Quaternion offsetRotation = Quaternion.Euler(-90, 0, 0);
            ps.transform.rotation = lightRotation * offsetRotation;
        }

        void UpdateAlpha(ParticleSystem ps)
        {
            float lightRotationX = directionalLight.transform.eulerAngles.x;

            if (lightRotationX > 180)
            {
                lightRotationX -= 360;
            }

            float alpha = 0f;
            if (lightRotationX >= 20 && lightRotationX <= 170)
            {
                alpha = Mathf.Clamp01((170 - Mathf.Abs(lightRotationX)) / 150);
            }

            var mainModule = ps.main;
            var startColor = mainModule.startColor;
            startColor.color = new Color(startColor.color.r, startColor.color.g, startColor.color.b, alpha);
            mainModule.startColor = startColor;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GodraysController))]
    public class GodraysControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GodraysController script = (GodraysController)target;

            serializedObject.Update();

            script.directionalLight = (Light)EditorGUILayout.ObjectField("Directional Light", script.directionalLight, typeof(Light), true);

            script.affectAllChildren = EditorGUILayout.Toggle("Affect All Children", script.affectAllChildren);

            EditorGUI.BeginDisabledGroup(script.affectAllChildren);
            SerializedProperty particleSystemsProp = serializedObject.FindProperty("particleSystems");
            EditorGUILayout.PropertyField(particleSystemsProp, true);
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}