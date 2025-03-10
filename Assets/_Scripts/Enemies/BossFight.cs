#region Using Statements
using Narzioth.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

/// <summary>
/// State Machine and animation controller for boss fight.
/// </summary>
public class BossFight : MonoBehaviour 
{
#region Variables

    [field: SerializeField]
    public BossState CurState { get; private set; } = BossState.Idle;

    [SerializeField] Animator _myAnim; //animates transform properties
    [SerializeField] Animator _spriteAnim; //animates sprite properties
    [SerializeField] BoxCollider2D _spriteBodyCollider;
    [SerializeField] SpriteRenderer _spriteRend;
    [SerializeField] BossHeart _heart;

    int _curAtkCount = 3;
    bool _readyToAtk;
    [SerializeField] float _atkRange = 1f;
    [SerializeField] Transform _spriteCenter;

    //Sprite Animation States
    const string IDLE = "Idle_Blend Tree";
    const string WALK = "Boss_Walk_S";
    const string RAISE_AXE = "Boss_RaiseAxe_S";
    const string BLOCK = "Block_Blend Tree";
    const string CHARGE = "AttackCharge_Blend Tree";
    const string ATTACK = "BossAttack_Blend Tree";
    const string SUMMON = "Summon_Blend Tree";
    const string DEATH = "Boss_Death_S";

    [SerializeField] GameObject _zombiePref;
    [SerializeField] Transform _zombieContainer;

#endregion
#region Base Methods

    void Start () 
    {
        RandomizeAtkCount();
        ChangeState(BossState.Spawning);
	}
	
	void Update () 
    {
		if (CurState == BossState.Blocking)
        {
            Sprite_FacePlayer();
        }

        if (_readyToAtk)
        {
            if (PlayerInRange())
            {
                _readyToAtk = false;
                ChangeState(BossState.Atk_Chargeup);
            }
        }
	}

#if UNITY_EDITOR 
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_spriteCenter.position, _atkRange);
    }
#endif

#endregion

    //================================================================================================
    #region State Changing
    public void ChangeState(BossState newState)
    {
        if (CurState != newState)
        {
            CurState = newState;
            ActivateNewState();
        }
    }

    void ActivateNewState()
    {
        switch (CurState)
        {
            case BossState.Spawning:
                _spriteAnim.Play(WALK);
                break;

            case BossState.Blocking:
                _spriteAnim.Play(BLOCK);
                _heart.Blocking_On();
                StartCoroutine(AtkCountdownTimerRtn());
                break;

            case BossState.Atk_Chargeup:
                _curAtkCount--;
                _myAnim.SetTrigger("attack");
                _spriteAnim.Play(CHARGE);
                _heart.Blocking_Off();
                break;

            case BossState.Attacking:
                _spriteAnim.Play(ATTACK);
                break;

            case BossState.Idle:
                _spriteAnim.Play(IDLE);
                _heart.Blocking_Off();
                _heart.Floating_Off();
                StartCoroutine(EndIdleTimerRtn());
                break;

            case BossState.Atk_Cooldown:
                _spriteAnim.Play(IDLE);
                break;

            case BossState.Summoning:
                _myAnim.SetTrigger("rise");
                Sprite_FaceSouth();
                _spriteAnim.Play(SUMMON);
                _spriteBodyCollider.enabled = false;
                _spriteRend.sortingLayerName = "Player";
                _heart.Floating_On();

                SpawnManager.Instance.StartSpawningPowerups_Boss();
                break;

            case BossState.Summoning_Cooldown:
                _myAnim.SetTrigger("fall");
                _spriteAnim.StopPlayback();
                Sprite_FaceSouth();

                SpawnManager.Instance.StopSpawningPowerups_Boss();
                break;

            case BossState.Dying:
                StopAllCoroutines();
                CancelInvoke("CompleteSummoningCooldown"); //Make sure state isn't reset while I'm dying
                _heart.gameObject.SetActive(false);
                _spriteBodyCollider.enabled = false;
                _spriteAnim.Play(DEATH);
                _myAnim.SetTrigger("death");
                SpawnManager.Instance.StopSpawningPowerups_Boss();
                Events.OnBossDeath?.Invoke();
                break;
        }
    }
    #endregion
    //================================================================================================

    IEnumerator AtkCountdownTimerRtn()
    {
        yield return HM.WaitTime(3);
        _readyToAtk = true;
    }
    IEnumerator EndIdleTimerRtn()
    {
        yield return HM.WaitTime(2);
        if (_curAtkCount < 1)
            ChangeState(BossState.Summoning);
        else ChangeState(BossState.Blocking);
    }

    void Sprite_FacePlayer()
    {
        Vector2 dirToPlayer = GameManager.Instance.PlayerPosition() - (Vector2)transform.position;
        dirToPlayer.Normalize();

        _spriteAnim.SetFloat("Horizontal", dirToPlayer.x);
        _spriteAnim.SetFloat("Vertical", dirToPlayer.y);
    }
    void Sprite_FaceSouth()
    {
        _spriteAnim.SetFloat("Horizontal", 0);
        _spriteAnim.SetFloat("Vertical", -1);
    }
    IEnumerator SpinAnimDirectionRtn()
    {
        float wait = 0.35f;

        while (CurState == BossState.Summoning)
        {
            //W
            yield return HM.WaitTime(wait);
            _spriteAnim.SetFloat("Horizontal", -1);
            _spriteAnim.SetFloat("Vertical", 0);
            //N
            yield return HM.WaitTime(wait);
            _spriteAnim.SetFloat("Horizontal", 0);
            _spriteAnim.SetFloat("Vertical", 1);
            //E
            yield return HM.WaitTime(wait);
            _spriteAnim.SetFloat("Horizontal", 1);
            _spriteAnim.SetFloat("Vertical", 0);
            //S
            yield return HM.WaitTime(wait);
            _spriteAnim.SetFloat("Horizontal", 0);
            _spriteAnim.SetFloat("Vertical", -1);
        }
    }

    void RandomizeAtkCount()
    {
        _curAtkCount = Random.Range(3, 6);
    }

    void SpawnZombies()
    {
        StartCoroutine(FinishSpawningZombiesRtn());
    }
    IEnumerator FinishSpawningZombiesRtn()
    {
        yield return StartCoroutine(SpawnZombiesRtn());

        ChangeState(BossState.Summoning_Cooldown);
    }
    IEnumerator SpawnZombiesRtn()
    {
        var spawnCount = Random.Range(10, 20);

        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(_zombiePref, RandomSpawnPosition(), Quaternion.identity, _zombieContainer);
            yield return HM.WaitTime(1);
        }
    }
    Vector2 RandomSpawnPosition()
    {
        float xBound = 9.96f;
        float randomX;
        float yBound = 5.24f;
        float randomY;
        Vector2 randomPos;
        do
        {
            randomX = Random.Range(-xBound, xBound);
            randomY = Random.Range(-yBound, yBound);
            randomPos = new Vector2(randomX, randomY);

        } while (Vector2.Distance(randomPos, Vector2.zero) <= _atkRange);

        return randomPos;
    }

    #region Methods called via Animator keyframe

    /// <summary>
    /// Called via Animator keyframe in Entrance animation.
    /// </summary>
    public void PlayRaiseAxe()
    {
        _spriteAnim.Play(RAISE_AXE);
    }
    /// <summary>
    /// Called via Animator keyframe in Entrance animation.
    /// </summary>
    public void FinishSpawning()
    {
        ChangeState(BossState.Blocking);
    }
    /// <summary>
    /// Called via Animator in Attack_Charge animations
    /// </summary>
    public void AttackChargeupFinished()
    {
        ChangeState(BossState.Attacking);
    }
    /// <summary>
    /// Called via keyframe in Summoning_Rise animation
    /// </summary>
    public void SummoningRiseFinished()
    {
        StartCoroutine(SpinAnimDirectionRtn());
        SpawnZombies();
    }
    /// <summary>
    /// Called via keyframe in Summoning_Fall animation
    /// </summary>
    public void SummoningFallFinished()
    {
        _spriteAnim.Play(IDLE);
        _spriteBodyCollider.enabled = true;
        _spriteRend.sortingLayerName = "Foreground";
        _heart.Floating_Off();
        Invoke("CompleteSummoningCooldown", 5);
    }
    /// <summary>
    /// Called after a delay when the summoning fall animation is finished. 
    /// Sets state back to idle, preparing for attack.
    /// </summary>
    void CompleteSummoningCooldown()
    {
        RandomizeAtkCount();
        ChangeState(BossState.Idle);
    }

    #endregion

    public void DeathAnimationFinished()
    {
        GameManager.Instance.BossKilled();
        Destroy(this.gameObject);
    }
    public void AttackFinished()
    {
        ChangeState(BossState.Idle);
    }

    bool PlayerInRange()
    {
        var distanceToPlayer = Vector2.Distance(_spriteCenter.position, GameManager.Instance.PlayerPosition());
        if (distanceToPlayer <= _atkRange)
            return true;
        return false;
    }

}