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
		if (CurState == BossState.Blocking ||
            CurState == BossState.Atk_Chargeup)
        {
            FacePlayer();
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
                //animate the sprite position from the top of the screen to the center (w/ special animation at the end to finish it off)
                _spriteAnim.Play(WALK);
                break;
            case BossState.Blocking:
                _spriteAnim.Play(BLOCK);
                //disable heart collider (deflict projectiles too)
                StartCoroutine(AtkCountdownTimerRtn());
                break;
            case BossState.Atk_Chargeup:
                _curAtkCount--;
                _myAnim.SetTrigger("attack");
                _spriteAnim.Play(CHARGE);
                //enable heart collider
                break;
            case BossState.Attacking:
                _spriteAnim.Play(ATTACK);
                //weapon collider gets enabled via animation
                //heart collider stays active
                break;
            case BossState.Idle:
                _spriteAnim.Play(IDLE);
                //heart collider stays active
                StartCoroutine(EndIdleTimerRtn());
                break;
            case BossState.Atk_Cooldown:
                _spriteAnim.Play(IDLE);
                //heart collider stays active
                break;
            case BossState.Summoning:
                _myAnim.SetTrigger("rise");
                //set sprite anim direction to S
                _spriteAnim.SetFloat("Horizontal", 0);
                _spriteAnim.SetFloat("Vertical", -1);
                _spriteAnim.Play(SUMMON);
                //turn off body collider
                //?can only be hit with projectiles while floating?
                //enable heart collider
                break;
            case BossState.Dying:
                //disable heart collider
                //set dying animation
                //trigger game over (or next wave) when finished
                break;
            default:
                break;
        }
    }

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

    void FacePlayer()
    {
        Vector2 dirToPlayer = GameManager.Instance.PlayerPosition() - (Vector2)transform.position;
        dirToPlayer.Normalize();

        _spriteAnim.SetFloat("Horizontal", dirToPlayer.x);
        _spriteAnim.SetFloat("Vertical", dirToPlayer.y);
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

    bool PlayerInRange()
    {
        var distanceToPlayer = Vector2.Distance(_spriteCenter.position, GameManager.Instance.PlayerPosition());
        if (distanceToPlayer <= _atkRange)
            return true;
        return false;
    }

    void RandomizeAtkCount()
    {
        _curAtkCount = Random.Range(3, 6);
    }

    void SpawnZombies()
    {
        StartCoroutine(SpawnZombiesRtn());
        //after [timer or zomvbie count]
        //  stop spawning zombies
        //  exit state
        //  turn off float animation
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

    #endregion

    public void AttackFinished()
    {
        ChangeState(BossState.Idle);
    }

}