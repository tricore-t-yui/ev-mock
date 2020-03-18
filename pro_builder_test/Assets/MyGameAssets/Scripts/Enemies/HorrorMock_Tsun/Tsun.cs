using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ツン。ほとんど見た目が変わった影人間
/// </summary>
public class Tsun : EnemyBase
{
    [SerializeField]
    AudioSource detectedVoice;
    [SerializeField]
    AudioSource spawnedVoice;
    [SerializeField]
    float detectVoiceInterval = 10.0f;
    [SerializeField]
    Transform randomVoiceRoot;
    [SerializeField]
    float randomVoiceInterval;
    [SerializeField]
    float randomVoiceIntervalRandomMin = 3;
    [SerializeField]
    float randomVoiceIntervalRandomMax = 5;

    float lastDetectVoiceTime;
    List<AudioSource> randomVoiceList = new List<AudioSource>();
    float lastRandomVoiceTime;
    Coroutine randomVoiceCoroutine = null;
    AudioSource playingRandomVoice;
    bool randomVoicePlaying;

    /// <summary>
    /// 初期処理
    /// </summary>
    void Awake()
    {
        randomVoiceList.AddRange(randomVoiceRoot.GetComponentsInChildren<AudioSource>(true));
        // ステートクラスのインスタンスを生成
        StateBase[] states = new StateBase[]
        {
            new TsunStateNormal(),
            new TsunStateCaution(),
            new TsunStateFighting(),
        };

        // ステートマシンの初期化
        Initialize(states);

        lastDetectVoiceTime = Time.timeSinceLevelLoad;
        lastRandomVoiceTime = Time.timeSinceLevelLoad + Random.Range(randomVoiceIntervalRandomMin, randomVoiceIntervalRandomMax);
    }

    public override void Update()
    {
        // 時間が経ってたらランダムボイス
        if(!randomVoicePlaying && Time.timeSinceLevelLoad - lastRandomVoiceTime > randomVoiceInterval)
        {
            foreach (var item in randomVoiceList)
            {
                item.gameObject.SetActive(false);
            }
            var idx = Random.Range(0, randomVoiceList.Count * 100 - 1) / 100;
            playingRandomVoice = randomVoiceList[idx];
            randomVoiceCoroutine = StartCoroutine (RandomVoiceCoroutine());
        }
        base.Update();
    }

    public override void OnEnterViewRange(Collider other)
    {
        if (parameter.IsStaticState) return;
        // プレイヤーのみ
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }

        // みいつけた
        if(Time.timeSinceLevelLoad - lastDetectVoiceTime > detectVoiceInterval)
        {
            detectedVoice.Play();
            lastDetectVoiceTime = Time.timeSinceLevelLoad;
            lastRandomVoiceTime = Time.timeSinceLevelLoad + Random.Range(randomVoiceIntervalRandomMin, randomVoiceIntervalRandomMax);
            StopTimeVoice();
        }

        states[(int)currentState].OnDetectedPlayer(other.gameObject);
    }

    public void OnSpawned()
    {
        // 笑い声
        spawnedVoice.Play();
        StopTimeVoice();
    }

    void StopTimeVoice()
    {
        foreach (var item in randomVoiceList)
        {
            item.gameObject.SetActive(false);
            item.Stop();
        }
        if(randomVoiceCoroutine != null) 
            StopCoroutine(randomVoiceCoroutine);
        randomVoicePlaying = false;
    }

    IEnumerator RandomVoiceCoroutine()
    {
        randomVoicePlaying = true;
        playingRandomVoice.gameObject.SetActive(true);
        playingRandomVoice.Play();
        while (playingRandomVoice.isPlaying)
        {
            yield return null;
        }
        playingRandomVoice.gameObject.SetActive(false);
        lastRandomVoiceTime = Time.timeSinceLevelLoad + Random.Range(randomVoiceIntervalRandomMin, randomVoiceIntervalRandomMax);
        randomVoicePlaying = false;
    }
}
