
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lit.Unity
{

    public class SpawnPoolsDict
    {

        public delegate void OnCreatedDelegate(SpawnPool pool);

        public Dictionary<string, OnCreatedDelegate> onCreatedDelegates =
             new Dictionary<string, OnCreatedDelegate>();

        private Dictionary<string, SpawnPool> _pools = new Dictionary<string, SpawnPool>();


        #region Event Handling
        public void AddOnCreatedDelegate(string poolName, OnCreatedDelegate createdDelegate)
        {
            if (!this.onCreatedDelegates.ContainsKey(poolName))
            {
                this.onCreatedDelegates.Add(poolName, createdDelegate);
                return;
            }

            this.onCreatedDelegates[poolName] += createdDelegate;
        }

        public void RemoveOnCreatedDelegate(string poolName, OnCreatedDelegate createdDelegate)
        {
            if (!this.onCreatedDelegates.ContainsKey(poolName))
            {
                LitLogger.WarningFormat("No OnCreatedDelegates found for pool name: <{0}>", poolName);
                return;
            }
            this.onCreatedDelegates[poolName] -= createdDelegate;
        }

        private void TrrigerCreateEvent(string poolName)
        {
            if (!this.onCreatedDelegates.ContainsKey(poolName))
            {
                LitLogger.WarningFormat("No OnCreatedDelegates found for pool name: <{0}>", poolName);
                return;
            }
            this.onCreatedDelegates[poolName](this[poolName]);
        }

        #endregion Event Handling



        public SpawnPool this[string key]
        {
            get
            {
                SpawnPool pool;
                try
                {
                    pool = this._pools[key];
                }
                catch (KeyNotFoundException)
                {
                    LitLogger.WarningFormat("A Pool with the name '{0}' not found. " + "\nPools={1}", key, this.ToString());
                    return null;
                }
                return pool;
            }
        }



        public SpawnPool Create(string poolName)
        {
            var owner = new GameObject(poolName + "Pool");
            owner.transform.SetParent(PoolMgr.GetInstance().transform);
            return owner.GetOrAddComponent<SpawnPool>();
        }


        public SpawnPool Create(string poolName, GameObject owner)
        {
            string ownerName = owner.gameObject.name;
            try
            {
                owner.gameObject.name = poolName;
                return owner.GetOrAddComponent<SpawnPool>();
            }
            finally
            {
                owner.gameObject.name = ownerName;
            }
        }

        public override string ToString()
        {
            var keysArray = new string[this._pools.Count];
            this._pools.Keys.CopyTo(keysArray, 0);
            return string.Format("[{0}]", System.String.Join(", ", keysArray));
        }


        public bool Destroy(string poolName)
        {
            SpawnPool spawnPool;
            if (!this._pools.TryGetValue(poolName, out spawnPool))
            {
                Debug.LogError(
                    string.Format("PoolManager: Unable to destroy '{0}'. Not in PoolManager",
                                  poolName));
                return false;
            }
            if (this._pools.Remove(poolName))
                UnityEngine.Object.Destroy(spawnPool.gameObject);
            else
                LitLogger.ErrorFormat("Destroy Pool Error: {0}", poolName);
            return true;
        }


        public void DestroyAll()
        {
            foreach (KeyValuePair<string, SpawnPool> pair in this._pools)
                UnityEngine.Object.Destroy(pair.Value);
            this._pools.Clear();
        }



        internal void Add(SpawnPool spawnPool)
        {
            if (this.ContainsKey(spawnPool.poolName))
            {
                LitLogger.WarningFormat(string.Format("A pool with the name '{0}' already exists. ", spawnPool.poolName));
                return;
            }

            this._pools.Add(spawnPool.poolName, spawnPool);
            TrrigerCreateEvent(spawnPool.name);
        }

        internal bool Remove(SpawnPool spawnPool)
        {
            if (!this.ContainsKey(spawnPool.poolName))
            {
                LitLogger.WarningFormat(string.Format("Pool not in PoolManager: {0}", spawnPool.poolName));
                return false;
            }

            this._pools.Remove(spawnPool.poolName);
            return true;
        }


        public int Count { get { return this._pools.Count; } }


        public bool ContainsKey(string poolName)
        {
            return this._pools.ContainsKey(poolName);
        }

        public bool TryGetValue(string poolName, out SpawnPool spawnPool)
        {
            return this._pools.TryGetValue(poolName, out spawnPool);
        }

    }

    public static class PoolManagerUtils
    {
        internal static void SetActive(GameObject obj, bool state)
        {
            obj.SetActive(state);
        }

        public static bool activeInHierarchy(GameObject obj)
        {
            return obj.activeInHierarchy;
        }
    }
}