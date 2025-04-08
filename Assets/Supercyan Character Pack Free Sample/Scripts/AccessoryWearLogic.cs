using UnityEngine;
using Fusion;
//using System;

namespace Supercyan.FreeSample
{
    public class AccessoryWearLogic : NetworkBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer m_characterRenderer;

        [SerializeField] private AccessoryLogic[] m_accessoriesToAttach = null;

        private Transform[] m_characterBones;

        private bool m_initialized = false;

        private void Initialize(GameObject character)
        {
            if (m_characterRenderer == null)
            {
                m_characterRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

                if (m_characterRenderer == null)
                {
                    Debug.LogWarning("Missing character components.");
                    return;
                }
            }
            if (m_characterRenderer.rootBone == null)
            {
                Debug.LogWarning("Missing character root bone.");
                return;
            }

            m_characterBones = m_characterRenderer.bones;
            m_initialized = true;
        }

        private void Awake()
        {
            Initialize(gameObject);
            foreach (AccessoryLogic a in m_accessoriesToAttach) { Attach(a); }
        }

        public void Attach(AccessoryLogic accessory)
        {
            if (!m_initialized)
            {
                Initialize(gameObject);
                if (!m_initialized)
                {
                    Debug.LogWarning("AccessoryWearLogic not initialized correctly, can't attach accessories.");
                    return;
                }
            }
            else if (accessory == null)
            {
                Debug.LogWarning("Trying to attach null accessory.");
                return;
            }
            else if (accessory.Renderer == null)
            {
                Debug.LogWarning("Trying to attach accessory with missing renderer.");
                return;
            }
            else if (accessory.Renderer.rootBone == null)
            {
                Debug.LogWarning("Trying to attach accessory with missing root bone.");
                return;
            }

            Transform[] newBones = GetBones(accessory.Renderer.bones, m_characterBones);
            if (newBones == null)
            {
                Debug.LogWarning("Trying to attach accessory with incompatible rig.");
                return;
            }

            accessory.Renderer.bones = newBones;
            accessory.Renderer.rootBone = m_characterRenderer.rootBone;
        }

        private Transform[] GetBones(Transform[] accessoryBones, Transform[] characterBones)
        {
            Transform[] newBones = new Transform[accessoryBones.Length];

            for (int i = 0; i < accessoryBones.Length; i++)
            {
                Transform bone = FindBone(m_characterRenderer.rootBone, accessoryBones[i].name);
                if (bone == null) { return null; }
                newBones[i] = bone;
            }

            return newBones;
        }

        private Transform FindBone(Transform rootBone, string name)
        {
            if (rootBone.name == name) { return rootBone; }
            else
            {
                Transform found = null;
                for (int i = 0; i < rootBone.childCount; i++)
                {
                    found = FindBone(rootBone.GetChild(i), name);
                    if (found != null) { return found; }
                }
                return null;
            }
        }

        [SerializeField]
        private GameObject m_hat;
        [SerializeField]
        private GameObject m_backpack;
        [SerializeField]
        private Texture [] m_texture;

        private void Update ()
        {
            if(HasStateAuthority && Input.GetKeyDown(KeyCode.Q))
            {
                RPC_Backpack(true);
            }

            if (HasStateAuthority && Input.GetKeyDown(KeyCode.E))
            {
                RPC_HatColor(true, Random.Range(0, 3));
            }
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_Backpack(bool isPack)
        {
            m_backpack.SetActive(isPack);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RPC_HatColor(bool isHat, int i)
        {
            m_hat.SetActive(isHat);
            m_hat.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials[0].mainTexture = m_texture[i];
        }
    }
}