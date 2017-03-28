using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lit.Unity
{
    
    [AddComponentMenu("PoolManager/SpawnPool")]
    public sealed class SpawnPool : MonoBehaviour
    {
        #region Inspector Parameters

        public string poolName = "";

        public bool matchPoolScale = false;

        public bool matchPoolLayer = false;

        public bool dontReparent = false;

        public bool _dontDestroyOnLoad = false;
		
        public bool logMessages = false;

        public List<PrefabPool> _perPrefabPoolOptions = new List<PrefabPool>();

        public Dictionary<object, bool> prefabsFoldOutStates = new Dictionary<object, bool>();

        public bool dontDestroyOnLoad
        {
            get
            {
                return this._dontDestroyOnLoad;
            }

            set
            {
                this._dontDestroyOnLoad = value;

                if (this.group != null)
                    Object.DontDestroyOnLoad(this.group.gameObject);
            }
        }
        
        #endregion Inspector Parameters


        #region Public Code-only Parameters
   
        public float maxParticleDespawnTime = 300;

        public Transform group { get; private set; }

        public PrefabsDict prefabs = new PrefabsDict();

        public Dictionary<object, bool> _editorListItemStates = new Dictionary<object, bool>();

        public Dictionary<string, PrefabPool> prefabPools
        {
            get
            {
                var dict = new Dictionary<string, PrefabPool>();

                for (int i = 0; i < this._prefabPools.Count; i++)
                    dict[this._prefabPools[i].prefabGO.name] = this._prefabPools[i];

                return dict;
            }
        }
        #endregion Public Code-only Parameters


        #region Private Properties
        private List<PrefabPool> _prefabPools = new List<PrefabPool>();
        internal List<Transform> _spawned = new List<Transform>();
        #endregion Private Properties


        #region Constructor and Init
        private void Awake()
        {
            if (this._dontDestroyOnLoad) Object.DontDestroyOnLoad(this.gameObject);

            this.group = this.transform;

            if (this.poolName == "")
            {
                this.poolName = this.group.name.Replace("Pool", "");
                this.poolName = this.poolName.Replace("(Clone)", "");
            }

            LogInfo(string.Format("SpawnPool {0}: Initializing..", this.poolName));

            for (int i = 0; i < this._perPrefabPoolOptions.Count; i++)
            {
                if (this._perPrefabPoolOptions[i].prefab == null)
                {
                    Debug.LogWarning(string.Format("Initialization Warning: Pool '{0}' " +
                              "contains a PrefabPool with no prefab reference. Skipping.",
                               this.poolName));
                    continue;
                }

                this._perPrefabPoolOptions[i].inspectorInstanceConstructor();
                this.CreatePrefabPool(this._perPrefabPoolOptions[i]);
            }
            PoolManager.Pools.Add(this);
        }


        private void OnDestroy()
        {
            LogInfo(string.Format("SpawnPool {0}: Destroying...", this.poolName));

            PoolManager.Pools.Remove(this);
            this.StopAllCoroutines();
            this._spawned.Clear();
            foreach (PrefabPool pool in this._prefabPools) pool.SelfDestruct();
            this._prefabPools.Clear();
            this.prefabs._Clear();
        }



        public void CreatePrefabPool(PrefabPool prefabPool)
        {
            bool isAlreadyPool = this.GetPrefabPool(prefabPool.prefab) == null ? false : true;
            if (!isAlreadyPool)
            {
                prefabPool.spawnPool = this;
                this._prefabPools.Add(prefabPool);
                this.prefabs._Add(prefabPool.prefab.name, prefabPool.prefab);
            }
            if (prefabPool.preloaded != true)
            {
                LogInfo(string.Format("SpawnPool {0}: Preloading {1} {2}",this.poolName,prefabPool.preloadAmount,prefabPool.prefab.name));
                prefabPool.PreloadInstances();
            }
        }


        public void Add(Transform instance, string prefabName, bool despawn, bool parent)
        {
            for (int i = 0; i < this._prefabPools.Count; i++)
            {
                if (this._prefabPools[i].prefabGO == null)
                {
                    Debug.LogError("Unexpected Error: PrefabPool.prefabGO is null");
                    return;
                }

                if (this._prefabPools[i].prefabGO.name == prefabName)
                {
                    this._prefabPools[i].AddUnpooled(instance, despawn);
                    LogInfo("SpawnPool {0}: Adding previously unpooled instance {1}", this.poolName, instance.name);
                    if (parent) instance.parent = this.group;
                    if (!despawn) this._spawned.Add(instance);
                    return;
                }
            }
            Debug.LogErrorFormat("SpawnPool {0}: PrefabPool {1} not found.", this.poolName,prefabName);

        }
        #endregion Constructor and Init


        #region Pool Functionality

        public Transform Spawn(Transform prefab, Vector3 pos, Quaternion rot, Transform parent)
        {
            Transform inst;

            #region Use from Pool
            for (int i = 0; i < this._prefabPools.Count; i++)
            {
                if (this._prefabPools[i].prefabGO == prefab.gameObject)
                {
                    inst = this._prefabPools[i].SpawnInstance(pos, rot);

                    if (inst == null) return null;
					
					if (parent != null)
					{
						inst.parent = parent;
					}
                    else if (!this.dontReparent && inst.parent != this.group)  // Auto organize?
					{
                        inst.parent = this.group;
					}
                    this._spawned.Add(inst);

                    //TODO ÓÐ´ýÓÅ»¯
	                inst.gameObject.BroadcastMessage(
						"OnSpawned",
						this,
						SendMessageOptions.DontRequireReceiver
					);
                    return inst;
                }
            }
            #endregion Use from Pool


            #region New PrefabPool

            PrefabPool newPrefabPool = new PrefabPool(prefab);
            this.CreatePrefabPool(newPrefabPool);

            inst = newPrefabPool.SpawnInstance(pos, rot);
			
			if (parent != null)
			{
				inst.parent = parent;
			}
            else
			{
            	inst.parent = this.group;  
			}
            this._spawned.Add(inst);
            #endregion New PrefabPool

            inst.gameObject.BroadcastMessage("OnSpawned",this,SendMessageOptions.DontRequireReceiver);

            return inst;
        }
        public Transform Spawn(Transform prefab, Vector3 pos, Quaternion rot)
        {
            Transform inst = this.Spawn(prefab, pos, rot, null);

            if (inst == null) return null;

            return inst;
        }

        public Transform Spawn(Transform prefab)
        {
            return this.Spawn(prefab, Vector3.zero, Quaternion.identity);
        }

        public Transform Spawn(Transform prefab, Transform parent)
        {
            return this.Spawn(prefab, Vector3.zero, Quaternion.identity, parent);
        }

        public Transform Spawn(string prefabName)
        {
            Transform prefab = this.prefabs[prefabName];
            return this.Spawn(prefab);
        }
        public Transform Spawn(string prefabName, Transform parent)
        {
            Transform prefab = this.prefabs[prefabName];
            return this.Spawn(prefab, parent);
        }
        public Transform Spawn(string prefabName, Vector3 pos, Quaternion rot)
        {
            Transform prefab = this.prefabs[prefabName];
            return this.Spawn(prefab, pos, rot);
        }
        public Transform Spawn(string prefabName, Vector3 pos, Quaternion rot, 
                               Transform parent)
        {
            Transform prefab = this.prefabs[prefabName];
            return this.Spawn(prefab, pos, rot, parent);
        }


        public AudioSource Spawn(AudioSource prefab, Vector3 pos, Quaternion rot)
        {
            return this.Spawn(prefab, pos, rot, null);
        }


        public AudioSource Spawn(AudioSource prefab)
        {
            return this.Spawn( prefab,  Vector3.zero, Quaternion.identity, null);
        }
		
	 	
		public AudioSource Spawn(AudioSource prefab, Transform parent)
        {
            return this.Spawn( prefab, Vector3.zero, Quaternion.identity, parent );
        }
		
		
        public AudioSource Spawn(AudioSource prefab, Vector3 pos, Quaternion rot,Transform parent)
        {
            Transform inst = Spawn(prefab.transform, pos, rot, parent);

            if (inst == null) return null;
            var src = inst.GetComponent<AudioSource>();
            src.Play();
            this.StartCoroutine(this.ListForAudioStop(src));
            return src;
        }

        public ParticleSystem Spawn(ParticleSystem prefab, Vector3 pos, Quaternion rot)
        {
            return Spawn(prefab, pos, rot, null);
        }

        public ParticleSystem Spawn(ParticleSystem prefab,Vector3 pos, Quaternion rot,Transform parent)
        {
            Transform inst = this.Spawn(prefab.transform, pos, rot, parent);

            if (inst == null) return null;
            var emitter = inst.GetComponent<ParticleSystem>();
            this.StartCoroutine(this.ListenForEmitDespawn(emitter));
            return emitter;
        }

        public ParticleEmitter Spawn(ParticleEmitter prefab,Vector3 pos, Quaternion rot)
        {
            Transform inst = this.Spawn(prefab.transform, pos, rot);

            if (inst == null) return null;
            var animator = inst.GetComponent<ParticleAnimator>();
            if (animator != null) animator.autodestruct = false;
            var emitter = inst.GetComponent<ParticleEmitter>();
            emitter.emit = true;
            this.StartCoroutine(this.ListenForEmitDespawn(emitter));
            return emitter;
        }

        public ParticleEmitter Spawn(ParticleEmitter prefab, Vector3 pos, Quaternion rot, string colorPropertyName, Color color)
        {
            Transform inst = this.Spawn(prefab.transform, pos, rot);

            if (inst == null) return null;
            var animator = inst.GetComponent<ParticleAnimator>();
            if (animator != null) animator.autodestruct = false;
            var emitter = inst.GetComponent<ParticleEmitter>();
            emitter.GetComponent<Renderer>().material.SetColor(colorPropertyName, color);
            emitter.emit = true;
            this.StartCoroutine(ListenForEmitDespawn(emitter));

            return emitter;
        }

        public void Despawn(Transform instance)
        {
            bool despawned = false;
            for (int i = 0; i < this._prefabPools.Count; i++)
            {
                if (this._prefabPools[i]._spawned.Contains(instance))
                {
                    despawned = this._prefabPools[i].DespawnInstance(instance);
                    break;
                }
                else if (this._prefabPools[i]._despawned.Contains(instance))
                {
                    LitLogger.WarningFormat("SpawnPool {0}: {1} has already been despawned. " +
                                       "You cannot despawn something more than once!",
                                        this.poolName,
                                        instance.name);
                    return;
                }
            }
            if (!despawned)
            {
                Debug.LogError(string.Format("SpawnPool {0}: {1} not found in SpawnPool",
                               this.poolName,
                               instance.name));
                return;
            }
            this._spawned.Remove(instance);
        }

        public void Despawn(Transform instance, Transform parent)
        {
            instance.parent = parent;
            this.Despawn(instance);
        }

        public void Despawn(Transform instance, float seconds)
        {
            this.StartCoroutine(this.DoDespawnAfterSeconds(instance, seconds, false, null));
        }

        public void Despawn(Transform instance, float seconds, Transform parent)
        {
            this.StartCoroutine(this.DoDespawnAfterSeconds(instance, seconds, true, parent));
        }

        private IEnumerator DoDespawnAfterSeconds(Transform instance, float seconds, bool useParent, Transform parent)
        {
            GameObject go = instance.gameObject;
            while (seconds > 0)
            {
                yield return null;
                if (!go.activeInHierarchy)
                    yield break;
                
                seconds -= Time.deltaTime;
            }

            if (useParent)
                this.Despawn(instance, parent);
            else
                this.Despawn(instance);
        }

        public void DespawnAll()
        {
            var spawned = new List<Transform>(this._spawned);
            for (int i = 0; i < spawned.Count; i++)
                this.Despawn(spawned[i]);
        }

        public bool IsSpawned(Transform instance)
        {
            return this._spawned.Contains(instance);
        }

        #endregion Pool Functionality



        #region Utility Functions
        public PrefabPool GetPrefabPool(Transform prefab)
        {
            for (int i = 0; i < this._prefabPools.Count; i++)
            {
                if (this._prefabPools[i].prefabGO == null)
                    Debug.LogError(string.Format("SpawnPool {0}: PrefabPool.prefabGO is null",
                                                 this.poolName));

                if (this._prefabPools[i].prefabGO == prefab.gameObject)
                    return this._prefabPools[i];
            }
            return null;
        }


        /// <summary>
        /// Returns the prefab pool for a given prefab.
        /// </summary>
        /// <param name="prefab">The GameObject of an instance</param>
        /// <returns>PrefabPool</returns>
        public PrefabPool GetPrefabPool(GameObject prefab)
        {
            for (int i = 0; i < this._prefabPools.Count; i++)
            {
                if (this._prefabPools[i].prefabGO == null)
                    Debug.LogError(string.Format("SpawnPool {0}: PrefabPool.prefabGO is null",
                                                 this.poolName));

                if (this._prefabPools[i].prefabGO == prefab)
                    return this._prefabPools[i];
            }

            // Nothing found
            return null;
        }


        /// <summary>
        /// Returns the prefab used to create the passed instance. 
        /// This is provided for convienince as Unity doesn't offer this feature.
        /// </summary>
        /// <param name="instance">The Transform of an instance</param>
        /// <returns>Transform</returns>
        public Transform GetPrefab(Transform instance)
        {
            for (int i = 0; i < this._prefabPools.Count; i++)
                if (this._prefabPools[i].Contains(instance))
                    return this._prefabPools[i].prefab;

            // Nothing found
            return null;
        }


        /// <summary>
        /// Returns the prefab used to create the passed instance. 
        /// This is provided for convienince as Unity doesn't offer this feature.
        /// </summary>
        /// <param name="instance">The GameObject of an instance</param>
        /// <returns>GameObject</returns>
        public GameObject GetPrefab(GameObject instance)
        {
            for (int i = 0; i < this._prefabPools.Count; i++)
                if (this._prefabPools[i].Contains(instance.transform))
                    return this._prefabPools[i].prefabGO;

            // Nothing found
            return null;
        }


        private IEnumerator ListForAudioStop(AudioSource src)
        {
            // Safer to wait a frame before testing if playing.
            yield return null;

            while (src.isPlaying)
                yield return null;

            this.Despawn(src.transform);
        }


        /// <summary>
        /// Used to determine when a particle emiter should be despawned
        /// </summary>
        /// <param name="emitter">ParticleEmitter to process</param>
        /// <returns></returns>
        private IEnumerator ListenForEmitDespawn(ParticleEmitter emitter)
        {
            // This will wait for the particles to emit. Without this, there will
            //   be no particles in the while test below. I don't know why the extra 
            //   frame is required but should never be noticable. No particles can
            //   fade out that fast and still be seen to change over time.
            yield return null;
            yield return new WaitForEndOfFrame();

            // Do nothing until all particles die or the safecount hits a max value
            float safetimer = 0;   // Just in case! See Spawn() for more info
            while (emitter.particleCount > 0)
            {
                safetimer += Time.deltaTime;
                if (safetimer > this.maxParticleDespawnTime)
                    Debug.LogWarning
                    (
                        string.Format
                        (
                            "SpawnPool {0}: " +
                                "Timed out while listening for all particles to die. " +
                                "Waited for {1}sec.",
                            this.poolName,
                            this.maxParticleDespawnTime
                        )
                    );

                yield return null;
            }

            // Turn off emit before despawning
            emitter.emit = false;
            this.Despawn(emitter.transform);
        }

        // ParticleSystem (Shuriken) Version...
        private IEnumerator ListenForEmitDespawn(ParticleSystem emitter)
        {
            // Wait for the delay time to complete
            // Waiting the extra frame seems to be more stable and means at least one 
            //  frame will always pass
            yield return new WaitForSeconds(emitter.startDelay + 0.25f);

            // Do nothing until all particles die or the safecount hits a max value
            float safetimer = 0;   // Just in case! See Spawn() for more info
            while (emitter.IsAlive(true))
            {
                if (!PoolManagerUtils.activeInHierarchy(emitter.gameObject))
                {
                    emitter.Clear(true);
                    yield break;  // Do nothing, already despawned. Quit.
                }

                safetimer += Time.deltaTime;
                if (safetimer > this.maxParticleDespawnTime)
                    Debug.LogWarning
                    (
                        string.Format
                        (
                            "SpawnPool {0}: " +
                                "Timed out while listening for all particles to die. " +
                                "Waited for {1}sec.",
                            this.poolName,
                            this.maxParticleDespawnTime
                        )
                    );

                yield return null;
            }

            // Turn off emit before despawning
            //emitter.Clear(true);
            this.Despawn(emitter.transform);
        }

        #endregion Utility Functions


        private void LogInfo(string msgFormat, params object[] ps)
        {
            if (logMessages)
                LitLogger.LogFormat(msgFormat, ps);
        }

        public override string ToString()
        {
            var name_list = new List<string>();
            foreach (Transform item in this._spawned)
                name_list.Add(item.name);
            return System.String.Join(", ", name_list.ToArray());
        }

        public Transform this[int index]
        {
            get { return this._spawned[index]; }
        }

        public void CopyTo(Transform[] array, int arrayIndex)
        {
            this._spawned.CopyTo(array, arrayIndex);
        }

        public int Count{ get { return this._spawned.Count; } }

    }



    [System.Serializable]
    public class PrefabPool
    {

        #region Public Properties Available in the Editor

        public Transform prefab;
        internal GameObject prefabGO;
        public int preloadAmount = 1;
        public bool preloadTime = false;
        public int preloadFrames = 2;
        public float preloadDelay = 0;
        public bool limitInstances = false;
        public int limitAmount = 100;
        public bool limitFIFO = false;
        public bool cullDespawned = false;
        public int cullAbove = 50;
        public int cullDelay = 60;
        public int cullMaxPerPass = 5;
        public bool _logMessages = false;
        private bool forceLoggingSilent = false;
        public bool logMessages
        {
            get
            {
                if (forceLoggingSilent) return false;

                if (this.spawnPool.logMessages)
                    return this.spawnPool.logMessages;
                else
                    return this._logMessages;
            }
        }


        public SpawnPool spawnPool;
        #endregion Public Properties Available in the Editor


        #region Constructor and Self-Destruction
        /// <description>
        ///	Constructor to require a prefab Transform
        /// </description>
        public PrefabPool(Transform prefab)
        {
            this.prefab = prefab;
            this.prefabGO = prefab.gameObject;
        }

        /// <description>
        ///	Constructor for Serializable inspector use only
        /// </description>
        public PrefabPool() { }

        /// <description>
        ///	A pseudo constructor to init stuff not init by the serialized inspector-created
        ///	instance of this class.
        /// </description>
        internal void inspectorInstanceConstructor()
        {
            this.prefabGO = this.prefab.gameObject;
            this._spawned = new List<Transform>();
            this._despawned = new List<Transform>();
        }


        /// <summary>
        /// Run by a SpawnPool when it is destroyed
        /// </summary>
        internal void SelfDestruct()
        {
            // Probably overkill but no harm done
            this.prefab = null;
            this.prefabGO = null;
            this.spawnPool = null;

            // Go through both lists and destroy everything
            foreach (Transform inst in this._despawned)
                if (inst != null)
                    Object.Destroy(inst.gameObject);

            foreach (Transform inst in this._spawned)
                if (inst != null)
                    Object.Destroy(inst.gameObject);

            this._spawned.Clear();
            this._despawned.Clear();
        }
        #endregion Constructor and Self-Destruction


        #region Pool Functionality
    
        private bool cullingActive = false;
        private bool _preloaded = false;

        public List<Transform> _spawned = new List<Transform>();
        public List<Transform> _despawned = new List<Transform>();
        public List<Transform> spawned { get { return new List<Transform>(this._spawned); } }
        public List<Transform> despawned { get { return new List<Transform>(this._despawned); } }
        public bool preloaded
        {
            get { return this._preloaded; }
            private set { this._preloaded = value; }
        }

        public int totalCount
        {
            get
            {
                int count = 0;
                count += this._spawned.Count;
                count += this._despawned.Count;
                return count;
            }
        }

        public bool DespawnInstance(Transform xform)
        {
            return DespawnInstance(xform, true);
        }

        internal bool DespawnInstance(Transform xform, bool sendEventMessage)
        {
            if (this.logMessages)
                Debug.Log(string.Format("SpawnPool {0} ({1}): Despawning '{2}'",
                                       this.spawnPool.poolName,
                                       this.prefab.name,
                                       xform.name));

            this._spawned.Remove(xform);
            this._despawned.Add(xform);

            if (sendEventMessage)
                xform.gameObject.BroadcastMessage(
					"OnDespawned",
					this.spawnPool,
                    SendMessageOptions.DontRequireReceiver
				);

            PoolManagerUtils.SetActive(xform.gameObject, false);

            if (!this.cullingActive && this.cullDespawned && this.totalCount > this.cullAbove)
            {
                this.cullingActive = true;
                this.spawnPool.StartCoroutine(CullDespawned());
            }
            return true;
        }



        internal IEnumerator CullDespawned()
        {
            if (this.logMessages)
                Debug.Log(string.Format("SpawnPool {0} ({1}): CULLING TRIGGERED! " +
                                          "Waiting {2}sec to begin checking for despawns...",
                                        this.spawnPool.poolName,
                                        this.prefab.name,
                                        this.cullDelay));

            yield return new WaitForSeconds(this.cullDelay);

            while (this.totalCount > this.cullAbove)
            {
                // Attempt to delete an amount == this.cullMaxPerPass
                for (int i = 0; i < this.cullMaxPerPass; i++)
                {
                    // Break if this.cullMaxPerPass would go past this.cullAbove
                    if (this.totalCount <= this.cullAbove)
                        break;  // The while loop will stop as well independently

                    // Destroy the last item in the list
                    if (this._despawned.Count > 0)
                    {
                        Transform inst = this._despawned[0];
                        this._despawned.RemoveAt(0);
                        MonoBehaviour.Destroy(inst.gameObject);

                        if (this.logMessages)
                            Debug.Log(string.Format("SpawnPool {0} ({1}): " +
                                                    "CULLING to {2} instances. Now at {3}.",
                                                this.spawnPool.poolName,
                                                this.prefab.name,
                                                this.cullAbove,
                                                this.totalCount));
                    }
                    else if (this.logMessages)
                    {
                        Debug.Log(string.Format("SpawnPool {0} ({1}): " +
                                                    "CULLING waiting for despawn. " +
                                                    "Checking again in {2}sec",
                                                this.spawnPool.poolName,
                                                this.prefab.name,
                                                this.cullDelay));

                        break;
                    }
                }

                // Check again later
                yield return new WaitForSeconds(this.cullDelay);
            }

            if (this.logMessages)
                Debug.Log(string.Format("SpawnPool {0} ({1}): CULLING FINISHED! Stopping",
                                        this.spawnPool.poolName,
                                        this.prefab.name));

            // Reset the singleton so the feature can be used again if needed.
            this.cullingActive = false;
            yield return null;
        }



        /// <summary>
        /// Move an instance from despawned to spawned, set the position and 
        /// rotation, activate it and all children and return the transform.
        /// 
        /// If there isn't an instance available, a new one is made.
        /// </summary>
        /// <returns>
        /// The new instance's Transform. 
        /// 
        /// If the Limit option was used for the PrefabPool associated with the
        /// passed prefab, then this method will return null if the limit is
        /// reached.
        /// </returns>    
        internal Transform SpawnInstance(Vector3 pos, Quaternion rot)
        {
            // Handle FIFO limiting if the limit was used and reached.
            //   If first-in-first-out, despawn item zero and continue on to respawn it
            if (this.limitInstances && this.limitFIFO &&
                this._spawned.Count >= this.limitAmount)
            {
                Transform firstIn = this._spawned[0];

                if (this.logMessages)
                {
                    Debug.LogErrorFormat( "SpawnPool {0} ({1}): LIMIT REACHED! FIFO=True. Calling despawning for {2}...",
                        this.spawnPool.poolName,
                        this.prefab.name,
                        firstIn
                    );
                }

                this.DespawnInstance(firstIn);

                // Because this is an internal despawn, we need to re-sync the SpawnPool's
                //  internal list to reflect this
                this.spawnPool._spawned.Remove(firstIn);
            }

            Transform inst;

            // If nothing is available, create a new instance
            if (this._despawned.Count == 0)
            {
                // This will also handle limiting the number of NEW instances
                inst = this.SpawnNew(pos, rot);
            }
            else
            {
                // Switch the instance we are using to the spawned list
                // Use the first item in the list for ease
                inst = this._despawned[0];
                this._despawned.RemoveAt(0);
                this._spawned.Add(inst);

                // This came up for a user so this was added to throw a user-friendly error
                if (inst == null)
                {
                    var msg = "Make sure you didn't delete a despawned instance directly.";
                    throw new MissingReferenceException(msg);
                }

                if (this.logMessages)
                    Debug.Log(string.Format("SpawnPool {0} ({1}): respawning '{2}'.",
                                            this.spawnPool.poolName,
                                            this.prefab.name,
                                            inst.name));

                // Get an instance and set position, rotation and then 
                //   Reactivate the instance and all children
                inst.position = pos;
                inst.rotation = rot;
                PoolManagerUtils.SetActive(inst.gameObject, true);

            }
			
			//
			// NOTE: OnSpawned message broadcast was moved to main Spawn() to ensure it runs last
			//
			
            return inst;
        }



        /// <summary>
        /// Spawns a NEW instance of this prefab and adds it to the spawned list.
        /// The new instance is placed at the passed position and rotation
        /// </summary>
        /// <param name="pos">Vector3</param>
        /// <param name="rot">Quaternion</param>
        /// <returns>
        /// The new instance's Transform. 
        /// 
        /// If the Limit option was used for the PrefabPool associated with the
        /// passed prefab, then this method will return null if the limit is
        /// reached.
        /// </returns>
        public Transform SpawnNew() { return this.SpawnNew(Vector3.zero, Quaternion.identity); }
        public Transform SpawnNew(Vector3 pos, Quaternion rot)
        {
            // Handle limiting if the limit was used and reached.
            if (this.limitInstances && this.totalCount >= this.limitAmount)
            {
                if (this.logMessages)
                {
                    Debug.Log(string.Format
                    (
                        "SpawnPool {0} ({1}): " +
                                "LIMIT REACHED! Not creating new instances! (Returning null)",
                            this.spawnPool.poolName,
                            this.prefab.name
                    ));
                }

                return null;
            }

            // Use the SpawnPool group as the default position and rotation
            if (pos == Vector3.zero) pos = this.spawnPool.group.position;
            if (rot == Quaternion.identity) rot = this.spawnPool.group.rotation;

            var inst = (Transform)Object.Instantiate(this.prefab, pos, rot);
            this.nameInstance(inst);  // Adds the number to the end

            if (!this.spawnPool.dontReparent)
                inst.parent = this.spawnPool.group;  // The group is the parent by default

            if (this.spawnPool.matchPoolScale)
                inst.localScale = Vector3.one;

            if (this.spawnPool.matchPoolLayer)
                this.SetRecursively(inst, this.spawnPool.gameObject.layer);

            // Start tracking the new instance
            this._spawned.Add(inst);

            if (this.logMessages)
                Debug.Log(string.Format("SpawnPool {0} ({1}): Spawned new instance '{2}'.",
                                        this.spawnPool.poolName,
                                        this.prefab.name,
                                        inst.name));

            return inst;
        }


        /// <summary>
        /// Sets the layer of the passed transform and all of its children
        /// </summary>
        /// <param name="xform">The transform to process</param>
        /// <param name="layer">The new layer</param>
        private void SetRecursively(Transform xform, int layer)
        {
            xform.gameObject.layer = layer;
            foreach (Transform child in xform)
                SetRecursively(child, layer);
        }


        /// <summary>
        /// Used by a SpawnPool to add an existing instance to this PrefabPool.
        /// This is used during game start to pool objects which are not 
        /// instantiated at runtime
        /// </summary>
        /// <param name="inst">The instance to add</param>
        /// <param name="despawn">True to despawn on add</param>
        internal void AddUnpooled(Transform inst, bool despawn)
        {
            this.nameInstance(inst);   // Adds the number to the end

            if (despawn)
            {
                // Deactivate the instance and all children
                PoolManagerUtils.SetActive(inst.gameObject, false);

                // Start Tracking as despawned
                this._despawned.Add(inst);
            }
            else
                this._spawned.Add(inst);
        }


        /// <summary>
        /// Preload PrefabPool.preloadAmount instances if they don't already exist. In 
        /// otherwords, if there are 7 and 10 should be preloaded, this only creates 3.
        /// This is to allow asynchronous Spawn() usage in Awake() at game start
        /// </summary>
        /// <returns></returns>
        internal void PreloadInstances()
        {
            // If this has already been run for this PrefabPool, there is something
            //   wrong!
            if (this.preloaded)
            {
                Debug.Log(string.Format("SpawnPool {0} ({1}): " +
                                          "Already preloaded! You cannot preload twice. " +
                                          "If you are running this through code, make sure " +
                                          "it isn't also defined in the Inspector.",
                                        this.spawnPool.poolName,
                                        this.prefab.name));

                return;
            }

            if (this.prefab == null)
            {
                Debug.LogError(string.Format("SpawnPool {0} ({1}): Prefab cannot be null.",
                                             this.spawnPool.poolName,
                                             this.prefab.name));

                return;
            }

            // Protect against preloading more than the limit amount setting
            //   This prevents an infinite loop on load if FIFO is used.
            if (this.limitInstances && this.preloadAmount > this.limitAmount)
            {
                Debug.LogWarning
                (
                    string.Format
                    (
                        "SpawnPool {0} ({1}): " +
                            "You turned ON 'Limit Instances' and entered a " +
                            "'Limit Amount' greater than the 'Preload Amount'! " +
                            "Setting preload amount to limit amount.",
                         this.spawnPool.poolName,
                         this.prefab.name
                    )
                );

                this.preloadAmount = this.limitAmount;
            }

            // Notify the user if they made a mistake using Culling
            //   (First check is cheap)
            if (this.cullDespawned && this.preloadAmount > this.cullAbove)
            {
                Debug.LogWarning(string.Format("SpawnPool {0} ({1}): " +
                    "You turned ON Culling and entered a 'Cull Above' threshold " +
                    "greater than the 'Preload Amount'! This will cause the " +
                    "culling feature to trigger immediatly, which is wrong " +
                    "conceptually. Only use culling for extreme situations. " +
                    "See the docs.",
                    this.spawnPool.poolName,
                    this.prefab.name
                ));
            }

            if (this.preloadTime)
            {
                if (this.preloadFrames > this.preloadAmount)
                {
                    Debug.LogWarning(string.Format("SpawnPool {0} ({1}): " +
                        "Preloading over-time is on but the frame duration is greater " +
                        "than the number of instances to preload. The minimum spawned " +
                        "per frame is 1, so the maximum time is the same as the number " +
                        "of instances. Changing the preloadFrames value...",
                        this.spawnPool.poolName,
                        this.prefab.name
                    ));

                    this.preloadFrames = this.preloadAmount;
                }

                this.spawnPool.StartCoroutine(this.PreloadOverTime());
            }
            else
            {
                // Reduce debug spam: Turn off this.logMessages then set it back when done.
                this.forceLoggingSilent = true;

                Transform inst;
                while (this.totalCount < this.preloadAmount) // Total count will update
                {
                    // Preload...
                    // This will parent, position and orient the instance
                    //   under the SpawnPool.group
                    inst = this.SpawnNew();
                    this.DespawnInstance(inst, false);
                }

                // Restore the previous setting
                this.forceLoggingSilent = false;
            }
        }

        private IEnumerator PreloadOverTime()
        {
            yield return new WaitForSeconds(this.preloadDelay);

            Transform inst;

            // subtract anything spawned by other scripts, just in case
            int amount = this.preloadAmount - this.totalCount;
            if (amount <= 0)
                yield break;

            // Doesn't work for Windows8...
            //  This does the division and sets the remainder as an out value.
            //int numPerFrame = System.Math.DivRem(amount, this.preloadFrames, out remainder);
            int remainder = amount % this.preloadFrames;
            int numPerFrame = amount / this.preloadFrames;

            // Reduce debug spam: Turn off this.logMessages then set it back when done.
            this.forceLoggingSilent = true;

            int numThisFrame;
            for (int i = 0; i < this.preloadFrames; i++)
            {
                // Add the remainder to the *last* frame
                numThisFrame = numPerFrame;
                if (i == this.preloadFrames - 1)
                {
                    numThisFrame += remainder;
                }

                for (int n = 0; n < numThisFrame; n++)
                {
                    // Preload...
                    // This will parent, position and orient the instance
                    //   under the SpawnPool.group
                    inst = this.SpawnNew();
                    if (inst != null)
                        this.DespawnInstance(inst, false);

                    yield return null;
                }

                // Safety check in case something else is making instances. 
                //   Quit early if done early
                if (this.totalCount > this.preloadAmount)
                    break;
            }

            // Restore the previous setting
            this.forceLoggingSilent = false;
        }

        #endregion Pool Functionality


        #region Utilities
        /// <summary>
        /// If this PrefabPool spawned or despawned lists contain the given 
        /// transform, true is returned. Othrewise, false is returned
        /// </summary>
        /// <param name="transform">A transform to test.</param>
        /// <returns>bool</returns>
        public bool Contains(Transform transform)
        {
            if (this.prefabGO == null)
                Debug.LogError(string.Format("SpawnPool {0}: PrefabPool.prefabGO is null",
                                             this.spawnPool.poolName));

            bool contains;

            contains = this.spawned.Contains(transform);
            if (contains)
                return true;

            contains = this.despawned.Contains(transform);
            if (contains)
                return true;

            return false;
        }
        
        /// <summary>
        /// Appends a number to the end of the passed transform. The number
        /// will be one more than the total objects in this PrefabPool, so 
        /// name the object BEFORE adding it to the spawn or depsawn lists.
        /// </summary>
        /// <param name="instance"></param>
        private void nameInstance(Transform instance)
        {
            // Rename by appending a number to make debugging easier
            //   ToString() used to pad the number to 3 digits. Hopefully
            //   no one has 1,000+ objects.
            instance.name += (this.totalCount + 1).ToString("#000");
        }
        #endregion Utilities

    }



    public class PrefabsDict : IDictionary<string, Transform>
    {
        #region Public Custom Memebers
        /// <summary>
        /// Returns a formatted string showing all the prefab names
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // Get a string[] array of the keys for formatting with join()
            var keysArray = new string[this._prefabs.Count];
            this._prefabs.Keys.CopyTo(keysArray, 0);

            // Return a comma-sperated list inside square brackets (Pythonesque)
            return string.Format("[{0}]", System.String.Join(", ", keysArray));
        }
        #endregion Public Custom Memebers


        #region Internal Dict Functionality
        // Internal Add and Remove...
        internal void _Add(string prefabName, Transform prefab)
        {
            this._prefabs.Add(prefabName, prefab);
        }

        internal bool _Remove(string prefabName)
        {
            return this._prefabs.Remove(prefabName);
        }

        internal void _Clear()
        {
            this._prefabs.Clear();
        }
        #endregion Internal Dict Functionality


        #region Dict Functionality
        // Internal (wrapped) dictionary
        private Dictionary<string, Transform> _prefabs = new Dictionary<string, Transform>();

        /// <summary>
        /// Get the number of SpawnPools in PoolManager
        /// </summary>
        public int Count { get { return this._prefabs.Count; } }

        /// <summary>
        /// Returns true if a prefab exists with the passed prefab name.
        /// </summary>
        /// <param name="prefabName">The name to look for</param>
        /// <returns>True if the prefab exists, otherwise, false.</returns>
        public bool ContainsKey(string prefabName)
        {
            return this._prefabs.ContainsKey(prefabName);
        }

        /// <summary>
        /// Used to get a prefab when the user is not sure if the prefabName is used.
        /// This is faster than checking Contains(prefabName) and then accessing the dict
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string prefabName, out Transform prefab)
        {
            return this._prefabs.TryGetValue(prefabName, out prefab);
        }

        #region Not Implimented

        public void Add(string key, Transform value)
        {
            throw new System.NotImplementedException("Read-Only");
        }

        public bool Remove(string prefabName)
        {
            throw new System.NotImplementedException("Read-Only");
        }

        public bool Contains(KeyValuePair<string, Transform> item)
        {
            string msg = "Use Contains(string prefabName) instead.";
            throw new System.NotImplementedException(msg);
        }

        public Transform this[string key]
        {
            get
            {
                Transform prefab;
                try
                {
                    prefab = this._prefabs[key];
                }
                catch (KeyNotFoundException)
                {
                    string msg = string.Format("A Prefab with the name '{0}' not found. " +
                                                "\nPrefabs={1}",
                                                key, this.ToString());
                    throw new KeyNotFoundException(msg);
                }

                return prefab;
            }
            set
            {
                throw new System.NotImplementedException("Read-only.");
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return this._prefabs.Keys;
            }
        }


        public ICollection<Transform> Values
        {
            get
            {
                return this._prefabs.Values;
            }
        }


        #region ICollection<KeyValuePair<string, Transform>> Members
        private bool IsReadOnly { get { return true; } }
        bool ICollection<KeyValuePair<string, Transform>>.IsReadOnly { get { return true; } }

        public void Add(KeyValuePair<string, Transform> item)
        {
            throw new System.NotImplementedException("Read-only");
        }

        public void Clear() { throw new System.NotImplementedException(); }

        private void CopyTo(KeyValuePair<string, Transform>[] array, int arrayIndex)
        {
            string msg = "Cannot be copied";
            throw new System.NotImplementedException(msg);
        }

        void ICollection<KeyValuePair<string, Transform>>.CopyTo(KeyValuePair<string, Transform>[] array, int arrayIndex)
        {
            string msg = "Cannot be copied";
            throw new System.NotImplementedException(msg);
        }

        public bool Remove(KeyValuePair<string, Transform> item)
        {
            throw new System.NotImplementedException("Read-only");
        }
        #endregion ICollection<KeyValuePair<string, Transform>> Members
        #endregion Not Implimented




        #region IEnumerable<KeyValuePair<string, Transform>> Members
        public IEnumerator<KeyValuePair<string, Transform>> GetEnumerator()
        {
            return this._prefabs.GetEnumerator();
        }
        #endregion



        #region IEnumerable Members
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._prefabs.GetEnumerator();
        }
        #endregion

        #endregion Dict Functionality

    }

}