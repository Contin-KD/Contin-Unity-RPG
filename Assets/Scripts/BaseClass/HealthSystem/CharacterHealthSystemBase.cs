using System;
using UGG.Combat;
using UGG.Move;
using UnityEngine;

namespace UGG.Health
{
    public abstract class CharacterHealthSystemBase : MonoBehaviour, IDamagar
    {
        
        //引用
        protected Animator _animator;
        protected CharacterMovementBase _movement;
        protected CharacterCombatSystemBase _combatSystem;
        protected AudioSource _audioSource;
        
        //攻击者
        protected Transform currentAttacker;
        
        //AnimationID
        protected int animationMove = Animator.StringToHash("AnimationMove");
        
        //HitAnimationMoveSpeedMult
        public float hitAnimationMoveMult;


        protected virtual void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _movement = GetComponent<CharacterMovementBase>();
            _combatSystem = GetComponentInChildren<CharacterCombatSystemBase>();
            _audioSource = _movement.GetComponentInChildren<AudioSource>();
        }


        protected virtual void Update()
        {
            HitAnimaitonMove();
        }
        
        
        /// <summary>
        /// 设置攻击者
        /// </summary>
        /// <param name="attacker">攻击者</param>
        public virtual void SetAttacker(Transform attacker)
        {
            if (currentAttacker != attacker || currentAttacker == null)
                currentAttacker = attacker;
        }

        protected virtual void HitAnimaitonMove()
        {
            if(!_animator.CheckAnimationTag("Hit")) return;
            _movement.CharacterMoveInterface(transform.forward,_animator.GetFloat(animationMove) * hitAnimationMoveMult,true);
        }

        #region 接口

        public virtual void TakeDamager(float damager)
        {
            throw new NotImplementedException();
        }

        public virtual void TakeDamager(string hitAnimationName)
        {
            _animator.Play(hitAnimationName,0,0f);
            GameAssets.Instance.PlaySoundEffect(_audioSource,SoundAssetsType.hit);
        }

        public virtual void TakeDamager(float damager, string hitAnimationName)
        {
            throw new NotImplementedException();
        }

        public virtual void TakeDamager(float damagar, string hitAnimationName, Transform attacker)
        {
            SetAttacker(attacker);
            
        }

        #endregion
        
        
        
        
    }
}

